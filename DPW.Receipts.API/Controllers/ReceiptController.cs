using DPW.Receipts.API.Models;
using DPW.Receipts.Core.Entities;
using DPW.Receipts.Core.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;

namespace DPW.Receipts.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptController : ControllerBase
    {     
        public ReceiptController()
        {            
        }

        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".csv")
            {
                return StatusCode(400,
                    new ProblemDetails{ Detail=$"{ext} is not supported", Status=400}
                    );
            }
            ReceiptModel receipt =new ReceiptModel();
            try
            {
                var receipts = new List<Receipt>();
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    receipts = FileProcessor.ReadCsv<Receipt, ReceiptMap>(stream);
                }
                 receipt = ReceiptAggregation(receipts);
            }
            catch (CsvHelper.BadDataException)
            {
                return BadRequest(
                    new ProblemDetails { Detail = $"The file is not in the correct format", Status = 415 }
                    );
            }
            catch (Exception)
            {

                return StatusCode(500,
                    new ProblemDetails { Detail = $"Unexpected server error", Status = 500 }
                    );
            }
            
            return Ok(receipt);

        }

        private static ReceiptModel ReceiptAggregation(List<Receipt> receipts)
        {
            var group = receipts.GroupBy(r => new { r.BusinessUnit, r.ReceiptMethodID });
            var receipt = new ReceiptModel();
            foreach (var item in group)
            {
                receipt.BusinessUnit = item.Key.BusinessUnit;
                receipt.ReceiptMethodID = item.Key.ReceiptMethodID;
                receipt.Transactions = item.Select(r => new Transaction
                {
                    RemittanceBank = r.RemittanceBank,
                    ReceiptNumber = r.ReceiptNumber,
                    ReceiptAmount = r.ReceiptAmount,
                    Invoicenumberreference = r.Invoicenumberreference,
                    InvoiceAmount = r.InvoiceAmount
                });
                break;
            }

            return receipt;
        }
    }
}
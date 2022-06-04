using DPW.Receipts.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using DPW.Receipts.Core.Entities;
using DPW.Receipts.Core.Services;

namespace DPW.Receipts.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment  _hostingEnvironment;
        
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UploadCsv(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".csv")
            {
                ViewBag.ErrorMessage = $"{ext} is not supported. Only csv files are supported";
                return View("Error");
            }
            string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ext;
            var filesDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "files");
            if (!Directory.Exists(filesDirectory))
            {
                Directory.CreateDirectory(filesDirectory);
            }
            var path = Path.Combine(filesDirectory,fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            TempData["filePath"] = path;
            return RedirectToAction("PreviewCsv");
        }
        public IActionResult PreviewCsv()
        {
            var dataTable = new DataTable();
            try
            {
                string filePath = TempData.Peek("filePath").ToString();
                List<Receipt> receipts = FileProcessor.ReadCsv<Receipt, ReceiptMap>(filePath);
                dataTable = FileProcessor.GetDataTable(receipts);
            }
            catch (Exception )
            {
                ViewBag.ErrorMessage = "An error occured while processing the file. Kindly make sure that the file is in correct format.";
                return View("Error");

            }
            
            return View(dataTable);
        }
        public IActionResult ExportExcel()
        {
            try
            {
                string filePath = TempData.Peek("filePath").ToString();
                List<Receipt> receipts = FileProcessor.ReadCsv<Receipt, ReceiptMap>(filePath);
                var excelFilePath = filePath.Replace(".csv", ".xlsx");
                FileProcessor.ExportExcel(receipts, excelFilePath);
                var excelwebPath = excelFilePath.Replace(_hostingEnvironment.WebRootPath, "").Replace(@"\", "/");
                ViewBag.excelwebPath = excelwebPath;
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error occured during preparing the excel file";
                return View("Error");
            }
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
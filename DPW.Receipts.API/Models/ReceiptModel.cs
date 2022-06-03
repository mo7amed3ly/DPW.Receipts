namespace DPW.Receipts.API.Models
{
    public class ReceiptModel
    {
        public string? BusinessUnit { get; set; }
        public decimal ReceiptMethodID { get; set; }
        public IEnumerable<Transaction>? Transactions { get; set; }
    }
}

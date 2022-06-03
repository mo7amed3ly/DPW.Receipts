namespace DPW.Receipts.API.Models
{
    public class Transaction
    {
        public string? RemittanceBank { get; set; }       
        public decimal RemittanceBankAccount { get; set; }        
        public decimal ReceiptNumber { get; set; }        
        public decimal ReceiptAmount { get; set; }
        public decimal Invoicenumberreference { get; set; }       
        public decimal InvoiceAmount { get; set; }
    }
}

using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPW.Receipts.Core.Entities
{
    public class Receipt
    {
        [Ordinal(0)]
        public string? BusinessUnit { get; set; }
        [Ordinal(1)]
        public decimal ReceiptMethodID { get; set; }
        [Ordinal(2)]
        public string? RemittanceBank { get; set; }
        [Ordinal(3)]
        public decimal RemittanceBankAccount { get; set; }
        [Ordinal(4)]
        public decimal ReceiptNumber { get; set; }
        [Ordinal(5)]
        public decimal ReceiptAmount { get; set; }
        [Ordinal(6)]
        public string? ReceiptDate { get; set; }
        [Ordinal(7)]
        public string? AccountingDate { get; set; }
        [Ordinal(8)]
        public string? ConversionDate { get; set; }
        [Ordinal(9)]
        public string? Currency { get; set; }
        [Ordinal(10)]
        public string? ConversionRateType { get; set; }
        [Ordinal(11)]
        public string? ConversionRate { get; set; }
        [Ordinal(12)]
        public string? CustomerName { get; set; }
        [Ordinal(13)]
        public string? CustomerAccountNumber { get; set; }
        [Ordinal(14)]
        public string? CustomerSiteNumber { get; set; }
        [Ordinal(15)]
        public decimal Invoicenumberreference { get; set; }
        [Ordinal(16)]
        public decimal InvoiceAmount { get; set; }
        [Ordinal(17)]
        public string? Comments { get; set; }
    }
}

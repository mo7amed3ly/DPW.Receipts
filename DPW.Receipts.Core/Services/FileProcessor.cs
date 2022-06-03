using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DPW.Receipts.Core.Entities;
using FastMember;
using System.Data;
using System.Globalization;

namespace DPW.Receipts.Core.Services
{
    public class FileProcessor
    {
        public static List<Receipt> ReadCsv(Stream stream)
        {

            List<Receipt> records = new List<Receipt>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Replace(" ", "").ToLowerInvariant(),
            };

            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<ReceiptMap>();

                records = csv.GetRecords<Receipt>().ToList();
            }
            return records;
        }
        public static List<Receipt> ReadCsv(string filePath)
        {

            List<Receipt> records = new List<Receipt>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Replace(" ", "").ToLowerInvariant(),
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<ReceiptMap>();

                records = csv.GetRecords<Receipt>().ToList();
            }
            return records;
        }
        public static void ExportExcel(List<Receipt> receipts, string filePath)
        {

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("Receipts").FirstCell().InsertTable(receipts, false);
                wb.SaveAs(filePath);
            }
        }
        public static DataTable GetDataTable<T>(List<T> data)
        {
            using (DataTable table = new DataTable())
            {
                using (var reader = ObjectReader.Create(data))
                {
                    table.Load(reader);
                }
                return table;
            }
        }
    }
    public class ExponentialDecimalConverter<T> : DecimalConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return decimal.Parse(text, NumberStyles.Float);
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return ((decimal)value).ToString();
        }
    }
    public class ReceiptMap : ClassMap<Receipt>
    {
        public ReceiptMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.ReceiptMethodID).TypeConverter<ExponentialDecimalConverter<decimal>>();
            Map(m => m.RemittanceBankAccount).TypeConverter<ExponentialDecimalConverter<decimal>>();

        }
    }
}

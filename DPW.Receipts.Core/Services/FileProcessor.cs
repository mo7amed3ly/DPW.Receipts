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
        public static List<T> ReadCsv<T, TMap>(Stream stream) where TMap : ClassMap
        {
            List<T> records = new List<T>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Replace(" ", "").ToLowerInvariant(),
            };

            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<TMap>();

                records = csv.GetRecords<T>().ToList();
            }
            return records;
        }
        public static List<T> ReadCsv<T, TMap>(string filePath) where TMap : ClassMap
        {

            List<T> records = new List<T>();            
            using (var reader = new FileStream(filePath,FileMode.Open))
            {
                records = ReadCsv<T, TMap>(reader);
            }
            return records;
        }
        public static void ExportExcel<T>(List<T> items, string filePath)
        {

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet("Data").FirstCell().InsertTable(items, false);
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

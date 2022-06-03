// See https://aka.ms/new-console-template for more information
using DPW.Receipts.Core;
using DPW.Receipts.Core.Services;

Console.WriteLine("Hello, World!");
var receipts=FileProcessor.ReadCsv(@"C:\Users\moham\Downloads\Receipts.csv");
FileProcessor.ExportExcel(receipts, @"C:\Users\moham\Downloads\Receipts.xlsx");
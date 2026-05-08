using ExtractorExcelToExcel.App;
using ExtractorExcelToExcel.DataStructures;
using ClosedXML.Excel;
using System.Text.RegularExpressions;

namespace ExtractorExcelToExcel.DataAccess;

internal class DiskRepository : IRepository
{
    public Record[] ReadRecordsFromRepository(string pathInputExcel, AppMode appMode, string sheetName, string columnPositions, string columnTexts, string columnTextsOverlay, string rowRange, string? cellIgnoringMark)
    {
        if(!File.Exists(pathInputExcel))
            throw new FileNotFoundException($"File with name '{pathInputExcel}' ({Path.GetFullPath(pathInputExcel)}) does not exist");

        using(var fileStream = new FileStream(pathInputExcel, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
            var workbook = new XLWorkbook(fileStream);
            if(workbook.TryGetWorksheet(sheetName, out IXLWorksheet worksheet)) {
                IEnumerable<Record> recordsFlow = appMode switch {
                    AppMode.extractOneColumn  => ReadExcelColumn(worksheet, columnPositions, columnTexts, rowRange, cellIgnoringMark),
                    AppMode.combineTwoColumns => ReadExcelTwoColumnsCombined(worksheet, columnPositions, columnTexts, columnTextsOverlay, rowRange, cellIgnoringMark),
                    _ => throw new ArgumentException("Reading from repository using unsupported mode: " + appMode),
                };

                return recordsFlow.OrderBy(record => record.Position)
                                  .ToArray();
            } else {
                throw new ArgumentException($"The file '{pathInputExcel}' doesn't contain a worksheet with name '{sheetName}'");
            }
        }
    }


    private IEnumerable<Record> ReadExcelColumn(IXLWorksheet worksheet, string columnPositions, string columnTexts, string rowRange, string? cellIgnoringMark = "")
    {
        string pattern = @"\d+";
        string searchTexts = Regex.Replace(rowRange, pattern, columnTexts + "$&");   //= Regex.Replace(rowRange, pattern, m => columnTexts + m.Value);

        var ieTexts = worksheet.Cells(searchTexts)
                               .Select(cell => cell.Value.ToString());

        if(columnPositions == "auto") {
            return ieTexts.Select((st, index) => new Record(index + 1, st))
                          .Where(record => record.Text != cellIgnoringMark);
        } else {
            string searchPositions = Regex.Replace(rowRange, pattern, columnPositions + "$&");

            IEnumerable<int> iePos = worksheet.Cells(searchPositions)
                                              .Select(cell => int.Parse(cell.Value.ToString()));

            return ieTexts.Zip(iePos, (st, pos) => new Record(pos, st))
                .Where(record => record.Text != cellIgnoringMark);
        }
    }


    private IEnumerable<Record> ReadExcelTwoColumnsCombined(IXLWorksheet worksheet, string columnPositions, string columnTexts, string columnOverlaps, string rowRange, string? cellIgnoringMark)
    {
        string pattern = @"\d+";
        string searchTexts = Regex.Replace(rowRange, pattern, columnTexts + "$&");   //= Regex.Replace(rowRange, pattern, m => columnTexts + m.Value);

        var ieTexts = worksheet.Cells(searchTexts)
                               .Select(cell => cell.Value.ToString());

        string searchOverlaps = Regex.Replace(rowRange, pattern, columnOverlaps + "$&");
        var ieOverlaps = worksheet.Cells(searchOverlaps)
                                  .Select(cell => cell.Value.ToString());

        if(columnPositions == "auto") {
            return ieTexts.Zip(ieOverlaps, (stOriginal, stOverlap) => (stOverlap == cellIgnoringMark) ? stOriginal : stOverlap)
                          .Select((st, index) => new Record(index + 1, st))
                          .Where(record => record.Text != cellIgnoringMark);
        } else {
            string searchPositions = Regex.Replace(rowRange, pattern, columnPositions + "$&");

            IEnumerable<int> iePos = worksheet.Cells(searchPositions)
                                              .Select(cell => int.Parse(cell.Value.ToString()));

            return ieTexts.Zip(ieOverlaps, (stOriginal, stOverlap) => (stOverlap == cellIgnoringMark) ? stOriginal : stOverlap)
                          .Zip(iePos, (st, pos) => new Record(pos, st))
                          .Where(record => record.Text != cellIgnoringMark);
        }
    }


    public void WriteRecordsToRepository(IEnumerable<Record> records, string pathOutputExcel, string sheetNameOutput, string columnTextsOutput, int headerDepth)
    {
        if(!File.Exists(pathOutputExcel))
            throw new FileNotFoundException($"File with name '{pathOutputExcel}' ({Path.GetFullPath(pathOutputExcel)}) does not exist");

        using(var fileStream = new FileStream(pathOutputExcel, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)) {
            var workbook = new XLWorkbook(fileStream);
            if(workbook.TryGetWorksheet(sheetNameOutput, out IXLWorksheet worksheet)) {
                foreach(var record in records)
                    worksheet.Cell($"{columnTextsOutput}{record.Position + headerDepth}").Value = record.Text;
                workbook.Save();
            } else {
                throw new ArgumentException($"The file '{pathOutputExcel}' doesn't contain a worksheet with name '{sheetNameOutput}'");
            }
        }
    }
}
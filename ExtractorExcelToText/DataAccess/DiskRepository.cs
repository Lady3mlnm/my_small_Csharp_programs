using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using ExtractorExcelToText.DataStructures;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtractorExcelToText.DataAccess;

internal class DiskRepository : IRepository
{
    public XLWorkbook GetXLWorkbook(string pathInputExcel)
    {
        if(!File.Exists(pathInputExcel))
            throw new FileNotFoundException($"\nFile with name '{pathInputExcel}'\n({Path.GetFullPath(pathInputExcel)})\ndoes not exist");

        return new XLWorkbook(pathInputExcel);
    }


    public IOrderedEnumerable<Record> ReadExcelColumn(ref XLWorkbook workbook, string sheetName, string columnPositions, string columnTexts, string rowRange, string? cellIgnoringMark = "")
    {
        var worksheet = workbook.Worksheet(sheetName);

        string pattern = @"\d+";
        string searchTexts = Regex.Replace(rowRange, pattern, columnTexts + "$&");   //= Regex.Replace(rowRange, pattern, m => columnTexts + m.Value);
        var ieTexts = worksheet.Cells(searchTexts)
                               .Select(cell => cell.Value.ToString());

        if(columnPositions == "auto") {
            return ieTexts.Select((st, index) => new Record(index+1, st))
                          .Where(record => record.Text != cellIgnoringMark)
                          .OrderBy(record => record.Position);
        } else {
            string searchPositions = Regex.Replace(rowRange, pattern, columnPositions + "$&");

            IEnumerable<int> iePos = worksheet.Cells(searchPositions)
                                              .Select(cell => int.Parse(cell.Value.ToString()));

            return ieTexts.Zip(iePos, (st, pos) => new Record(pos, st))
                          .Where(record => record.Text != cellIgnoringMark)
                          .OrderBy(record => record.Position);
        }
    }


    public IOrderedEnumerable<Record> ReadExcelTwoColumnsCombined(ref XLWorkbook workbook, string sheetName, string columnPositions, string columnTexts, string columnOverlaps, string rowRange, string? cellIgnoringMark)
    {
        var worksheet = workbook.Worksheet(sheetName);

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
                          .Where(record => record.Text != cellIgnoringMark)
                          .OrderBy(record => record.Position);
        } else {
            string searchPositions = Regex.Replace(rowRange, pattern, columnPositions + "$&");

            IEnumerable<int> iePos = worksheet.Cells(searchPositions)
                                              .Select(cell => int.Parse(cell.Value.ToString()));

            return ieTexts.Zip(ieOverlaps, (stOriginal, stOverlap) => (stOverlap == cellIgnoringMark) ? stOriginal : stOverlap)
                          .Zip(iePos, (st, pos) => new Record(pos, st))
                          .Where(record => record.Text != cellIgnoringMark)
                          .OrderBy(record => record.Position);
        }
    }


    public string[] ReadTxt(string pathInputText, Encoding encoding)
    {
        if(!File.Exists(pathInputText))
            throw new FileNotFoundException($"\nFile with name '{pathInputText}'\n({Path.GetFullPath(pathInputText)})\ndoes not exist");

        return File.ReadAllLines(pathInputText, encoding)
                   .ToArray();
    }


    public void WriteArrayToRepository(string filePath, string[] stringsReady, bool addEmptyLineToEnd, Encoding encoding)
    {
        if(addEmptyLineToEnd)
            File.WriteAllLines(filePath, stringsReady, encoding);    // This creates empty line at the end of the file
        else
            File.WriteAllText(filePath,
                              string.Join(Environment.NewLine, stringsReady),
                              encoding);
    }
}

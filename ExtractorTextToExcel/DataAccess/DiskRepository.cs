using ClosedXML.Excel;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtractorTextToExcel.DataAccess;

internal class DiskRepository : IRepository
{
    public string[] ReadAllStringsFromRepository(string pathTxtInput, string stringRange, Encoding encoding)
    {
        if(!File.Exists(pathTxtInput))
            throw new FileNotFoundException($"File with name '{pathTxtInput}' ({Path.GetFullPath(pathTxtInput)}) does not exist");

        if(stringRange == ":")
            return File.ReadAllLines(pathTxtInput, encoding);
        if(Regex.Match(stringRange, @"^:\d+$").Success) {
            int endLine = int.Parse(stringRange[1..]);
            return File.ReadLines(pathTxtInput, encoding)
                       .Take(endLine)
                       .ToArray();
        }
        if(Regex.Match(stringRange, @"^\d+:$").Success) {
            int startLine = int.Parse(stringRange[..^1]);
            return File.ReadLines(pathTxtInput, encoding)
                       .Skip(startLine-1)
                       .ToArray();
        }
        if(Regex.Match(stringRange, @"^\d+:\d+$").Success) {
            int[] rangeParts = stringRange.Split(':').Select(int.Parse).ToArray();
            (int startLine, int endLine) = (rangeParts[0], rangeParts[1]);
            return File.ReadLines(pathTxtInput, encoding)
                       .Skip(startLine - 1)
                       .Take(endLine - startLine + 1)
                       .ToArray();
        }
        throw new ArgumentException($"The parameter stringRange '{stringRange}' is in an unsupported form. " +
            "It's value has to be only in the form of range, like '5:15', ':20', or ':' for all strings.");
        }


    public void WriteStringsToRepository(string[] strings, string pathOutputExcel, string sheetNameOutput,
        string columnTextsOutput, int headerDepth, string[] stringIgnoringMarks)
    {
        if(!File.Exists(pathOutputExcel))
            throw new FileNotFoundException($"File with name '{pathOutputExcel}' ({Path.GetFullPath(pathOutputExcel)}) does not exist");

        using(var fileStream = new FileStream(pathOutputExcel, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)) {
            var workbook = new XLWorkbook(fileStream);
            if(workbook.TryGetWorksheet(sheetNameOutput, out IXLWorksheet worksheet)) {
                int rowCounter = headerDepth;
                switch(stringIgnoringMarks.Length) {
                    case 0:
                        foreach(var st in strings)
                            worksheet.Cell($"{columnTextsOutput}{++rowCounter}").Value = st;
                        break;
                    case 1:
                        string stringIgnoringMark = stringIgnoringMarks[0];
                        foreach(var st in strings) {
                            rowCounter++;
                            if(st != stringIgnoringMark)
                                worksheet.Cell($"{columnTextsOutput}{rowCounter}").Value = st;
                        }
                        break;
                    default:
                        foreach(var st in strings) {
                            rowCounter++;
                            if(!stringIgnoringMarks.Contains(st))
                                worksheet.Cell($"{columnTextsOutput}{rowCounter}").Value = st;
                        }
                        break;
                }
                workbook.Save();
            } else
                throw new ArgumentException($"The file '{pathOutputExcel}' doesn't contain a worksheet with name '{sheetNameOutput}'");
        }
    }
}
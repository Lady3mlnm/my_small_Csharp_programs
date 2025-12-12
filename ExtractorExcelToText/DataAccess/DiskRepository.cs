using ClosedXML.Excel;
using ExtractorExcelToText.DataStructures;
using System.Text;

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

        var rowBeginEndPositions = rowRange.Split(":");
        string stRangeBegin = rowBeginEndPositions[0];
        string stRangeEnd = rowBeginEndPositions[1];

        IEnumerable<int> ieIds = (columnPositions == "auto")
            ? Enumerable.Range(1, int.Parse(stRangeEnd) - int.Parse(stRangeBegin) + 1)
            : worksheet.Cells($"{columnPositions}{stRangeBegin}:{columnPositions}{stRangeEnd}")
                       .Select(cell => int.Parse(cell.Value.ToString()));

        var ieTexts = worksheet.Cells($"{columnTexts}{stRangeBegin}:{columnTexts}{stRangeEnd}")
                               .Select(cell => cell.Value.ToString());

        return ieIds.Zip(ieTexts, (id, st) => new Record(id, st))
                    .Where(record => record.Text != cellIgnoringMark)
                    .OrderBy(record => record.Position);
    }


    public IOrderedEnumerable<Record> ReadExcelTwoColumnsCombined(ref XLWorkbook workbook, string sheetName, string columnPositions, string columnTexts, string columnOverlay, string rowRange, string? cellIgnoringMark)
    {
        var worksheet = workbook.Worksheet(sheetName);

        var rowBeginEndPositions = rowRange.Split(":");
        string stRangeBegin = rowBeginEndPositions[0];
        string stRangeEnd = rowBeginEndPositions[1];

        IEnumerable<int> ieIds = (columnPositions == "auto")
            ? Enumerable.Range(1, int.Parse(stRangeEnd) - int.Parse(stRangeBegin) + 1)
            : worksheet.Cells($"{columnPositions}{stRangeBegin}:{columnPositions}{stRangeEnd}")
                       .Select(cell => int.Parse(cell.Value.ToString()));

        var ieTexts = worksheet.Cells($"{columnTexts}{stRangeBegin}:{columnTexts}{stRangeEnd}")
                               .Select(cell => cell.Value.ToString());

        var ieOverlaps = worksheet.Cells($"{columnOverlay}{stRangeBegin}:{columnOverlay}{stRangeEnd}")
                                  .Select(cell => cell.Value.ToString());

        return ieTexts.Zip(ieOverlaps, (stOriginal, stOverlap) => (stOverlap == cellIgnoringMark) ? stOriginal : stOverlap)
                      .Zip(ieIds, (st, id) => new Record(id, st))
                      .Where(record => record.Text != cellIgnoringMark)
                      .OrderBy(record => record.Position);
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

using ExtractorExcelToText.App;
using ExtractorExcelToText.DataStructures;
using ClosedXML.Excel;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtractorExcelToText.DataAccess;

public class DiskRepository : IRepository
{
    public (Record[], int nmbLinesIgnoredAtStart) ReadRecordsFromRepository(
        string pathExcelInput, AppMode appMode, string sheetInput,
        string columnPositions, string columnTextsInput, string columnTextsOverlay,
        bool preliminarySortSheetByColumnPositions, int headerDepthInput, string rowRangeInput, string[] cellIgnoringMarks)
    {
        if(!File.Exists(pathExcelInput))
            throw new FileNotFoundException($"File with name '{pathExcelInput}' ({Path.GetFullPath(pathExcelInput)}) does not exist");

        using(var fileStream = new FileStream(pathExcelInput, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
            var workbook = new XLWorkbook(fileStream);
            if(workbook.TryGetWorksheet(sheetInput, out IXLWorksheet worksheet)) {
                if(worksheet.IsEmpty())
                    throw new InvalidOperationException($"Worksheet '{worksheet}' is empty");  // Early termination of the application to avoid unclear error further

                if(preliminarySortSheetByColumnPositions && columnPositions != "auto")
                    preliminarySortOfWorksheet(ref worksheet, headerDepthInput, columnPositions);

                clarifyRowRange(ref rowRangeInput, worksheet, headerDepthInput);

                (IEnumerable<Record> recordsFlow, int nmbLinesIgnoredAtStart) = appMode switch {
                    AppMode.extractOneColumn => ReadExcelColumn(worksheet, columnPositions, columnTextsInput, rowRangeInput, cellIgnoringMarks),
                    AppMode.combineTwoColumns => ReadExcelTwoColumnsCombined(worksheet, columnPositions, columnTextsInput, columnTextsOverlay, rowRangeInput, cellIgnoringMarks),
                    _ => throw new ArgumentException("Reading from repository using unsupported mode: " + appMode),
                };

                return (recordsFlow.OrderBy(record => record.Position)
                                   .ToArray(),
                        nmbLinesIgnoredAtStart);
            } else
                throw new ArgumentException($"The file '{pathExcelInput}' doesn't contain a worksheet with name '{sheetInput}'");
        }
    }


    private void preliminarySortOfWorksheet(ref IXLWorksheet worksheet, int headerDepth, string columnSorting)
    {
        int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
        int lastCol = worksheet.LastColumnUsed()?.ColumnNumber() ?? 0;
        var dataRange = worksheet.Range(
            1 + headerDepth,   // first row
            1,                      // first column
            lastRow,                // last row
            lastCol);               // last column
        dataRange.Sort(columnSorting);
        //PrintWorksheet(worksheet);  // Debugging: check if the sheet is sorted preliminary correctly
    }


    private void clarifyRowRange(ref string rowRange, IXLWorksheet worksheet, int headerDepth)
    {
        if(rowRange.StartsWith(':'))
            rowRange = (headerDepth + 1) + rowRange;
        if(rowRange.EndsWith(':'))
            rowRange = rowRange + (worksheet.LastRowUsed()?.RowNumber() ?? 0);
    }


    private (IEnumerable<Record>, int nmbLinesIgnoredAtStart) ReadExcelColumn(
        IXLWorksheet worksheet, string columnPositions, string columnTexts, string rowRange, string[] cellIgnoringMarks)
    {
        string pattern = @"\d+";
        string searchTexts = Regex.Replace(rowRange, pattern, columnTexts + "$&");   //= Regex.Replace(rowRange, pattern, m => columnTexts + m.Value);

        var ieTexts = worksheet.Cells(searchTexts)
                               .Select(cell => cell.Value.ToString());

        if(columnPositions == "auto") {
            switch(cellIgnoringMarks.Length) {
                case 0:
                    return (ieTexts.Select((st, index) => new Record(index + 1, st)),
                            0);
                case 1:
                    string cellIgnoringMark = cellIgnoringMarks[0];
                    var records = ieTexts.Select((st, index) => new Record(index + 1, st));
                    return (records.Where(record => record.Text != cellIgnoringMark),
                            records.TakeWhile(record => record.Text == cellIgnoringMark).Count());
                default:
                    var records2 = ieTexts.Select((st, index) => new Record(index + 1, st));
                    return (records2.Where(record => !cellIgnoringMarks.Contains(record.Text)),
                            records2.TakeWhile(record => cellIgnoringMarks.Contains(record.Text)).Count());
            }
        } else {
            string searchPositions = Regex.Replace(rowRange, pattern, columnPositions + "$&");

            IEnumerable<int> iePos = worksheet.Cells(searchPositions)
                                              .Select(cell => int.Parse(cell.Value.ToString()));

            switch(cellIgnoringMarks.Length) {
                case 0:
                    return (ieTexts.Zip(iePos, (st, pos) => new Record(pos, st)),
                            0);
                case 1:
                    string cellIgnoringMark = cellIgnoringMarks[0];
                    var records = ieTexts.Zip(iePos, (st, pos) => new Record(pos, st));
                    return (records.Where(record => record.Text != cellIgnoringMark),
                            records.TakeWhile(record => record.Text == cellIgnoringMark).Count());
                default:
                    var records2 = ieTexts.Zip(iePos, (st, pos) => new Record(pos, st));
                    return (records2.Where(record => !cellIgnoringMarks.Contains(record.Text)),
                            records2.TakeWhile(record => cellIgnoringMarks.Contains(record.Text)).Count());
            }
        }
    }


    private (IEnumerable<Record>, int nmbLinesIgnoredAtStart) ReadExcelTwoColumnsCombined(
        IXLWorksheet worksheet, string columnPositions, string columnTexts, string columnOverlaps, string rowRange, string[] cellIgnoringMarks)
    {
        string pattern = @"\d+";
        string searchTexts = Regex.Replace(rowRange, pattern, columnTexts + "$&");   //= Regex.Replace(rowRange, pattern, m => columnTexts + m.Value);

        var ieTexts = worksheet.Cells(searchTexts)
                               .Select(cell => cell.Value.ToString());

        string searchOverlaps = Regex.Replace(rowRange, pattern, columnOverlaps + "$&");
        var ieOverlaps = worksheet.Cells(searchOverlaps)
                                  .Select(cell => cell.Value.ToString());

        if(columnPositions == "auto") {
            switch(cellIgnoringMarks.Length) {
                case 0:
                    return (ieOverlaps.Select((st, index) => new Record(index + 1, st)),
                            0);     // algorithm similar to that in the 'ReadExcelColumn' function
                case 1:
                    string cellIgnoringMark = cellIgnoringMarks[0];
                    var records = ieTexts.Zip(ieOverlaps, (stOriginal, stOverlap) => (stOverlap == cellIgnoringMark) ? stOriginal : stOverlap)
                                         .Select((st, index) => new Record(index + 1, st));
                    return (records.Where(record => record.Text != cellIgnoringMark),
                            records.TakeWhile(record => record.Text == cellIgnoringMark).Count());
                default:
                    var records2 = ieTexts.Zip(ieOverlaps, (stOriginal, stOverlap) => (cellIgnoringMarks.Contains(stOverlap)) ? stOriginal : stOverlap)
                                          .Select((st, index) => new Record(index + 1, st));
                    return (records2.Where(record => !cellIgnoringMarks.Contains(record.Text)),
                            records2.TakeWhile(record => cellIgnoringMarks.Contains(record.Text)).Count());
            }
        } else {
            string searchPositions = Regex.Replace(rowRange, pattern, columnPositions + "$&");

            IEnumerable<int> iePos = worksheet.Cells(searchPositions)
                                              .Select(cell => int.Parse(cell.Value.ToString()));

            switch(cellIgnoringMarks.Length) {
                case 0:
                    return (ieOverlaps.Zip(iePos, (st, pos) => new Record(pos, st)),
                            0);    // algorithm similar to that in the 'ReadExcelColumn' function
                case 1:
                    string cellIgnoringMark = cellIgnoringMarks[0];
                    var records = ieTexts.Zip(ieOverlaps, (stOriginal, stOverlap) => (stOverlap == cellIgnoringMark) ? stOriginal : stOverlap)
                                         .Zip(iePos, (st, pos) => new Record(pos, st));
                    return (records.Where(record => record.Text != cellIgnoringMark),
                            records.TakeWhile(record => record.Text == cellIgnoringMark).Count());
                default:
                    var records2 = ieTexts.Zip(ieOverlaps, (stOriginal, stOverlap) => (cellIgnoringMarks.Contains(stOverlap)) ? stOriginal : stOverlap)
                                          .Zip(iePos, (st, pos) => new Record(pos, st));
                    return (records2.Where(record => !cellIgnoringMarks.Contains(record.Text)),
                            records2.TakeWhile(record => cellIgnoringMarks.Contains(record.Text)).Count());
            }
        }
    }


    public string[] ReadTxt(string pathTxtInput, Encoding encoding)
    {
        if(!File.Exists(pathTxtInput))
            throw new FileNotFoundException($"File with name '{pathTxtInput}' ({Path.GetFullPath(pathTxtInput)}) does not exist");

        return File.ReadAllLines(pathTxtInput, encoding)
                   .ToArray();
    }


    public void WriteArrayToRepository(string pathTxtOutput, string[] stringsReady, bool emptyLineAtEnd, Encoding encoding)
    {
        if(emptyLineAtEnd)
            File.WriteAllLines(pathTxtOutput, stringsReady, encoding);    // This creates empty line at the end of the file
        else
            File.WriteAllText(pathTxtOutput,
                              string.Join(Environment.NewLine, stringsReady),
                              encoding);
    }





    // Two functions below output content of a worksheet and data range to the console as table.
    // These functions are used only for debugging purposes, so I remained them in this class in violation of the Single Responsibility Principle.
    private static void PrintWorksheet(IXLWorksheet ws)
    {
        var usedRange = ws.RangeUsed();   // Get the used range (only cells that have been used)
        if(usedRange == null) {
            Console.WriteLine("Worksheet is empty");
            return;
        }

        PrintWorksheetRange(usedRange);
    }

    private static void PrintWorksheetRange(IXLRange wsRange)
    {
        int rowCount = wsRange.RowCount();
        int columnCount = wsRange.ColumnCount();

        int[] colWidths = new int[columnCount];  // Compute max width per column for nicer alignment
        for(int col = 1; col <= columnCount; col++) {
            int idxCol = col - 1;
            colWidths[idxCol] = 0;

            for(int row = 1; row <= rowCount; row++) {
                var text = wsRange.Cell(row, col).GetFormattedString();
                if(text.Length > colWidths[idxCol])
                    colWidths[idxCol] = text.Length;
            }
        }

        for(int row = 1; row <= rowCount; row++) {       // Print rows
            for(int col = 1; col <= columnCount; col++) {
                var text = wsRange.Cell(row, col).GetFormattedString();
                int idxCol = col - 1;
                Console.Write(text.PadRight(colWidths[idxCol] + 2));  // Pad each cell to its column width + 2 spaces
            }
            Console.WriteLine();
        }
    }

}


/*
Explanation of "row[c]?.Length ?? 0"
    ?. - access property only if object is not null
    ?? - if value on the left is null, use the value on the right
*/


/*  // Alternative way to print a worksheet. This way looks quicker but more memory-consuming.
    private static void PrintWorksheetAlternative(IXLWorksheet worksheet)
    {
        var range = worksheet.RangeUsed();   // Get the used range (only cells that have been used)
        if(range == null) {
            Console.WriteLine("Worksheet is empty");
            return;
        }

        int rowCount    = range.RowCount();
        int columnCount = range.ColumnCount();
        
        var table = new List<string[]>();      // Read all cell values into memory
        for(int row = 1; row <= rowCount; row++) {
            var columnsOfRow = new string[columnCount];
            for(int col = 1; col <= columnCount; col++)
                columnsOfRow[col - 1] = range.Cell(row, col).GetFormattedString();
            table.Add(columnsOfRow);
        }

        int[] colWidths = new int[columnCount];   // Calculate column widths
        for(int c = 0; c < columnCount; c++)
            colWidths[c] = table.Max(row => row[c]?.Length ?? 0);  // Take the length of row[c] if it exists; if row[c] is null, use 0

        foreach(var row in table) {           // Print rows
            for(int c = 0; c < columnCount; c++) {
                string text = row[c] ?? "";
                Console.Write(text.PadRight(colWidths[c] + 2));  // Pad each cell to its column width + 2 spaces
            }
            Console.WriteLine();
        }
    }
*/
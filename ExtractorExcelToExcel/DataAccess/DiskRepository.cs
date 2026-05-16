using ClosedXML.Excel;
using ExtractorExcelToExcel.App;
using ExtractorExcelToExcel.DataStructures;
using System;
using System.Text.RegularExpressions;

namespace ExtractorExcelToExcel.DataAccess;

internal class DiskRepository : IRepository
{
    public Record[] ReadRecordsFromRepository(string pathInputExcel, AppMode appMode, string sheetName,
                                              string columnPositions, string columnTexts, string columnTextsOverlay,
                                              bool preliminarySortSheetByColumnPositions, string rowRange, string? cellIgnoringMark)
    {
        if(!File.Exists(pathInputExcel))
            throw new FileNotFoundException($"File with name '{pathInputExcel}' ({Path.GetFullPath(pathInputExcel)}) does not exist");

        using(var fileStream = new FileStream(pathInputExcel, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
            var workbook = new XLWorkbook(fileStream);
            if(workbook.TryGetWorksheet(sheetName, out IXLWorksheet worksheet)) {
                if(preliminarySortSheetByColumnPositions && columnPositions != "auto") {
                    var lastRow = worksheet.LastRowUsed().RowNumber();
                    var lastCol = worksheet.LastColumnUsed().ColumnNumber();
                    const int headerDepthInput = 1;
                    var dataRange = worksheet.Range(
                        1 + headerDepthInput,   // first row
                        1,                      // first column
                        lastRow,                // last row
                        lastCol);               // last column
                    dataRange.Sort(columnPositions);
                    //PrintWorksheet(worksheet);  // Debugging: check if the sheet is sorted preliminary correctly
                }

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
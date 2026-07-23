using SearchFormulasInExcel.App;
using SearchFormulasInExcel.DataStructures;
using ClosedXML.Excel;

namespace SearchFormulasInExcel.DataAccess;

public class DiskRepository : IRepository
{
    public FileStream GetFileStreamFromStorage(string pathExcel)
    {
        if(!File.Exists(pathExcel))
            throw new FileNotFoundException($"\nFile with name '{pathExcel}' ({Path.GetFullPath(pathExcel)}) does not exist");
        return new FileStream(pathExcel, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }


    public IEnumerable<(string col, Record[]? arFormulas, Record[]? arTextStartingWithEqual)> GetEntitiesFromExcelColumn(
        FileStream fileStream, string sheetName, string[] columns, int headerDepth, SearchedEntity searchedEntity)
    {
        var workbook = new XLWorkbook(fileStream);
        if(workbook.TryGetWorksheet(sheetName, out IXLWorksheet worksheet)) {
            if(worksheet.IsEmpty())
                throw new InvalidOperationException($"Worksheet '{worksheet}' is empty");  // Early termination of the application to avoid unclear error further

            int firstRow = headerDepth + 1;
            int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;

            foreach(var col in columns) {
                string searchRange = $"{col}{firstRow}:{col}{lastRow}";

                Record[]? arFormulas = (searchedEntity == SearchedEntity.formula || searchedEntity == SearchedEntity.bothFormulaAndText)
                    ? worksheet.Cells(searchRange)
                               .Select(cell => cell.HasFormula
                                                   ? '=' + cell.FormulaA1
                                                   : null)
                               .Select((text, index) => new Record(firstRow + index, text))
                               .Where(record => record.Text is not null)
                               .ToArray()
                    : null;

                Record[]? arTextStartingWithEqual = (searchedEntity == SearchedEntity.textStartingWithEqual || searchedEntity == SearchedEntity.bothFormulaAndText)
                    ? worksheet.Cells(searchRange)
                               .Select(cell => cell.HasFormula
                                                   ? null
                                                   : cell.GetString())
                               .Select((text, index) => new Record(firstRow + index, text))
                               .Where(record => record.Text is not null && record.Text.StartsWith('='))
                               .ToArray()
                    : null;

                yield return (col, arFormulas, arTextStartingWithEqual);
            }
        } else
            throw new ArgumentException($"The Excel file doesn't contain a worksheet with name '{sheetName}'");
    }
}
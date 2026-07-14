using ClosedXML.Excel;
using SearchFormulasInExcel.DataStructures;

namespace SearchFormulasInExcel.DataAccess;

public class DiskRepository : IRepository
{
    public IEnumerable<(string columnNume, IEnumerable<RecordWithFormula> formulasInColumn)> ReadColumnFromExcel(
    string pathExcel, string sheet, string[] columns, int headerDepth)
    {
        if(!File.Exists(pathExcel))
            throw new FileNotFoundException($"File with name '{pathExcel}' ({Path.GetFullPath(pathExcel)}) does not exist");

        using(var fileStream = new FileStream(pathExcel, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
            var workbook = new XLWorkbook(fileStream);
            if(workbook.TryGetWorksheet(sheet, out IXLWorksheet worksheet)) {
                if(worksheet.IsEmpty())
                    throw new InvalidOperationException($"Worksheet '{worksheet}' is empty");  // Early termination of the application to avoid unclear error further

                int firstRow = headerDepth + 1;
                int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
                int startingPoint = headerDepth + 1;

                foreach(var col in columns) {
                    string searchRange = $"{col}{firstRow}:{col}{lastRow}";

                    yield return (col, worksheet.Cells(searchRange)
                                                .Select(cell => cell.FormulaA1)
                                                .Select((formulaBody, index) => new RecordWithFormula(startingPoint + index, formulaBody))
                                                .Where(record => record.FormulaBody != ""));
                }
            }
        }
    }
}
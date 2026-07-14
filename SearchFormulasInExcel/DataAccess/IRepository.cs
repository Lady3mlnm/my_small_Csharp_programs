using SearchFormulasInExcel.DataStructures;

namespace SearchFormulasInExcel.DataAccess;

public interface IRepository
{
    IEnumerable<(string columnNume, IEnumerable<RecordWithFormula> formulasInColumn)> ReadColumnFromExcel(
        string pathExcel, string sheet, string[] columns, int headerDepth);
}
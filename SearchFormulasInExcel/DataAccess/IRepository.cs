using SearchFormulasInExcel.App;
using SearchFormulasInExcel.DataStructures;

namespace SearchFormulasInExcel.DataAccess;

public interface IRepository
{
    FileStream GetFileStreamFromStorage(string pathExcel);

    IEnumerable<(string col, Record[]? arFormulas, Record[]? arTextStartingWithEqual)> GetEntitiesFromExcelColumn(
        FileStream fileStream, string sheetName, string[] columns, int headerDepth, SearchedEntity searchedEntity);
}
using SearchFormulasInExcel.DataAccess;
using SearchFormulasInExcel.DataStructures;

namespace SearchFormulasInExcel.App;

public class SearchFormulasInExcelApp
{
    private readonly IRepository _repository;
    private readonly IUserInteraction _userInteraction;

    public SearchFormulasInExcelApp(IRepository repository, IUserInteraction userInteraction)
    {
        _repository = repository;
        _userInteraction = userInteraction;
    }

    public void Run()
    {
        string pathExcel;
        string sheetName;
        string[] columns;
        int headerDepth;
        SearchedEntity searchedEntity;
        bool closeAppIfFormulasNotFound;

        (pathExcel, sheetName, headerDepth, columns, searchedEntity, closeAppIfFormulasNotFound) =
            _userInteraction.GetParameters();

        int foundTotal = 0;

        using(FileStream fileStream = _repository.GetFileStreamFromStorage(pathExcel)) {
            switch(searchedEntity) {
                case SearchedEntity.formula:
                    _userInteraction.ShowMessage('\n' + "Searching for formulas in columns:");
                    foreach((string column, Record[]? arFormulas, var _)
                            in _repository.GetEntitiesFromExcelColumn(fileStream, sheetName, columns, headerDepth, searchedEntity)) {
                        _userInteraction.ShowMessage($"--- column {column} ---", ConsoleColor.Cyan);
                        int foundInColumn = arFormulas?.Length ?? 0;
                        if(foundInColumn == 0)
                            _userInteraction.ShowMessage("-", ConsoleColor.DarkGray);
                        else {
                            foreach(var formula in arFormulas!)
                                _userInteraction.ShowMessage(formula.ToString(), ConsoleColor.DarkYellow);
                            foundTotal += foundInColumn;
                        }
                        _userInteraction.ShowMessage();
                    }
                    break;

                case SearchedEntity.textStartingWithEqual:
                    _userInteraction.ShowMessage('\n' + "Searching for texts starting with '=' in columns:");
                    foreach((string column, var _, Record[]? arTextStartingWithEqual)
                            in _repository.GetEntitiesFromExcelColumn(fileStream, sheetName, columns, headerDepth, searchedEntity)) {
                        _userInteraction.ShowMessage($"--- column {column} ---", ConsoleColor.Cyan);
                        int foundInColumn = arTextStartingWithEqual?.Length ?? 0;
                        if(foundInColumn == 0)
                            _userInteraction.ShowMessage("-", ConsoleColor.DarkGray);
                        else {
                            foreach(var formula in arTextStartingWithEqual!)
                                _userInteraction.ShowMessage(formula.ToString(), ConsoleColor.DarkYellow);
                            foundTotal += foundInColumn;
                        }
                        _userInteraction.ShowMessage();
                    }
                    break;


                case SearchedEntity.bothFormulaAndText:
                    _userInteraction.ShowMessage('\n' + "Searching for both: formulas and texts starting with '=' in columns:");
                    foreach((string column, Record[]? arFormulas, Record[]? arTextStartingWithEqual)
                            in _repository.GetEntitiesFromExcelColumn(fileStream, sheetName, columns, headerDepth, searchedEntity)) {
                        _userInteraction.ShowMessage($"--- column {column} ---", ConsoleColor.Cyan);
                        _userInteraction.ShowMessage($"        formulas:");

                        int foundInColumn = arFormulas?.Length ?? 0;
                        if(foundInColumn == 0)
                            _userInteraction.ShowMessage("-", ConsoleColor.DarkGray);
                        else {
                            foreach(var formula in arFormulas!)
                                _userInteraction.ShowMessage(formula.ToString(), ConsoleColor.DarkYellow);
                            foundTotal += foundInColumn;
                        }

                        _userInteraction.ShowMessage($"        texts starting with '=':");
                        foundInColumn = arTextStartingWithEqual?.Length ?? 0;
                        if(foundInColumn == 0)
                            _userInteraction.ShowMessage("-", ConsoleColor.DarkGray);
                        else {
                            foreach(var formula in arTextStartingWithEqual!)
                                _userInteraction.ShowMessage(formula.ToString(), ConsoleColor.DarkYellow);
                            foundTotal += foundInColumn;
                        }
                        _userInteraction.ShowMessage();
                    }
                    break;

                default:
                    throw new ArgumentException($"Unsupported mode for searched entity: '{searchedEntity}'.");
            }
        }

        _userInteraction.ShowMessage("Total number of found entities: " + foundTotal);

        if(!closeAppIfFormulasNotFound || foundTotal > 0)
            _userInteraction.GetCloseAppConfirmation();
    }
}


/*
Explanation of "arFormulas?.Length ?? 0"
    ?. - access property only if object is not null
    ?? - if value on the left is null, use the value on the right
*/
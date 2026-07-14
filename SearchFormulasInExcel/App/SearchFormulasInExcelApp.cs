using SearchFormulasInExcel.DataAccess;

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
        string sheet;
        string[] columns;
        int headerDepth;
        bool closeAppIfFormulasNotFound;


        (pathExcel, sheet, headerDepth, columns, closeAppIfFormulasNotFound) =
            _userInteraction.GetParameters();

        int foundTotal = 0;

        _userInteraction.ShowMessage('\n' + "Searching for formulas in columns:",
                                         ConsoleColor.Cyan);

        foreach((var columnName, var formulasInColumn) in _repository.ReadColumnFromExcel(pathExcel, sheet, columns, headerDepth)) {
            _userInteraction.ShowMessage($"--- column {columnName} ---",
                                         ConsoleColor.Cyan);
            int foundInColumn = formulasInColumn.Count();
            if(foundInColumn == 0)
                _userInteraction.ShowMessage("-");
            else {
                foreach(var formula in formulasInColumn)
                    _userInteraction.ShowMessage(formula.ToString());
                foundTotal += foundInColumn;
            }
            _userInteraction.ShowMessage("");
        }

        _userInteraction.ShowMessage("Total number of found formalas: " + foundTotal,
                                     ConsoleColor.Cyan);

        if(!closeAppIfFormulasNotFound || foundTotal > 0) {
            _userInteraction.GetCloseAppConfirmation();
        }
    }
}

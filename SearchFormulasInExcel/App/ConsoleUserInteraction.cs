namespace SearchFormulasInExcel.App;

public class ConsoleUserInteraction : IUserInteraction
{
    private string _pathExcel = @"Data\Test_Input.xlsx";
    private string _sheet = "TestSheet";
    private int _headerDepth = 0;
    private string[] _columns = ["A", "B", "C"];
    private SearchedEntity _searchedEntity = SearchedEntity.bothFormulaAndText;   // formula;  textStartingWithEqual;  bothFormulaAndText;
    private bool _closeAppIfFormulasNotFound = false;

    static Dictionary<string, string> ParseArguments(string[] args)
    {
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach(var arg in args) {
            var parts = arg.Split('=', 2);
            var key = parts[0].TrimStart('-', '/');
            var value = (parts.Length > 1) ? parts[1] : "true";   // for flags like --verbose
            dict[key] = value;
        }

        return dict;
    }


    public ConsoleUserInteraction(string[] args, string appTitle = "ExtractorExcelToText")
    {
        Console.Title = appTitle;

        var options = ParseArguments(args);

        if(options.TryGetValue("pathExcel", out var pathExcel)) {
            _pathExcel = pathExcel;
            ShowMessage("Name of Excel file: " + _pathExcel, ConsoleColor.Green);
        } else
            ShowMessage("Name of Excel file is not given. Used default: " + _pathExcel, ConsoleColor.DarkGray);

        if(options.TryGetValue("sheet", out var sheet)) {
            _sheet = sheet;
            ShowMessage("Name of sheet in the Excel file: " + _sheet, ConsoleColor.Green);
        } else
            ShowMessage("Name of sheet in the Excel file is not given. Used default: " + _sheet, ConsoleColor.DarkGray);

        if(options.TryGetValue("headerDepth", out var headerDepth)) {
            if(int.TryParse(headerDepth, out int headerDepthInt) && headerDepthInt >= 0) {
                _headerDepth = headerDepthInt;
                ShowMessage($"Number of rows in header of the input Excel: {_headerDepth}", ConsoleColor.Green);
            } else
                throw new ArgumentException($"Invalid value for parameter 'headerDepth': {headerDepth}. It should be a non-negative integer.");
        } else
            ShowMessage($"Number of rows in header of the input Excel is not given. Used default: {_headerDepth}", ConsoleColor.DarkGray);

        if(options.TryGetValue("columns", out string? columns)) {
            _columns = columns.Split(",")
                              .Select(st => st.Trim())
                              .ToArray();
            ShowMessage("Columns on the sheet of the Excel file: " + string.Join(",", _columns), ConsoleColor.Green);
        } else
            ShowMessage("Columns on the sheet of the Excel file are not given. Used default: " + string.Join(",", _columns), ConsoleColor.DarkGray);

        if(options.TryGetValue("searchedEntity", out var searchedEntity)) {
            if(Enum.TryParse<SearchedEntity>(searchedEntity, true, out SearchedEntity parsedMode) && Enum.IsDefined(typeof(SearchedEntity), parsedMode)) {
                _searchedEntity = parsedMode;
                ShowMessage("Searched entity (mode): " + _searchedEntity, ConsoleColor.Green);
            } else
                throw new ArgumentException($"Invalid value for parameter 'searchedEntity': {searchedEntity}. " +
                                            $"Use one of the following values: {string.Join(" / ", Enum.GetNames(typeof(SearchedEntity)))}.");
        } else
            ShowMessage("Searched entity (mode) is not given. Used default: " + _searchedEntity, ConsoleColor.DarkGray);

        if(options.TryGetValue("closeAppIfFormulasNotFound", out var closeAppIfFormulasNotFound)) {
            if(bool.TryParse(closeAppIfFormulasNotFound, out bool parsedValue)) {
                _closeAppIfFormulasNotFound = parsedValue;
                ShowMessage(@"Close the application if formulas are not found without waiting for user confirmation: " + _closeAppIfFormulasNotFound, ConsoleColor.Green);
            } else
                throw new ArgumentException($"Invalid value for parameter 'closeAppIfFormulasNotFound': {closeAppIfFormulasNotFound}. It should be a boolean.");
        } else
            ShowMessage(@"Parameter whether to close the application if formulas are not found is not given. Used default: " + _closeAppIfFormulasNotFound, ConsoleColor.DarkGray);
    }


    public (string pathExcel, string sheet, int headerDepth, string[] columns, SearchedEntity searchedEntity, bool closeAppIfFormulasNotFound)
        GetParameters() =>
        (_pathExcel, _sheet, _headerDepth, _columns, _searchedEntity, _closeAppIfFormulasNotFound);


    public void ShowMessage()
    {
        Console.WriteLine();
    }


    public void ShowMessage(string message, bool isLinebreakAdded = true)
    {
        if(isLinebreakAdded)
            Console.WriteLine(message);
        else
            Console.Write(message);
    }


    public void ShowMessage(string message, ConsoleColor color, bool isLinebreakAdded = true)
    {
        Console.ForegroundColor = color;
        if(isLinebreakAdded)
            Console.WriteLine(message);
        else
            Console.Write(message);
        Console.ResetColor();
    }


    public void ShowMessage(IEnumerable<string> listMessages)
    {
        Console.WriteLine(string.Join('\n', listMessages));
    }


    public void ShowMessage(IEnumerable<string> listMessages, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(string.Join('\n', listMessages));
        Console.ResetColor();
    }


    public void GetCloseAppConfirmation()
    {
        Console.WriteLine("Press any key to close this application.");
        Console.ReadKey();
    }
}
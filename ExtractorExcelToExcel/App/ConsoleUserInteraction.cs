namespace ExtractorExcelToExcel.App;

public class ConsoleUserInteraction : IUserInteraction
{
    private AppMode _appMode = AppMode.extractOneColumn;    // AppMode.extractOneColumn / AppMode.combineTwoColumns
    private string _pathInputExcel = @"Data\Test_Input.xlsx";
    private string _sheetName = "TestSheet";                // "Amino Acids"
    private string _columnPositions = "auto";               // "A", "auto", "autoNumbering", "default", "-"
    private string _columnTexts = "C";
    private string _columnTextsOverlay = "H";
    private string _rowRange = "2:11";                      // "2:11", "3:5,10,14:16";
    private string? _cellIgnoringMark = "";
    private string _pathOutputExcel = @"Data\Test_Output.xlsx";
    private string _sheetNameOutput = "copyInputSheet";     // "Storage", "copyInputSheet"
    private string _columnTextsOutput = "copyInputColumn";  // "copyInputColumn"
    private int _headerDepth = 1;      

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

        if(options.TryGetValue("appMode", out var appMode)) {
            _appMode = (AppMode)Enum.Parse(typeof(AppMode), appMode);
            ShowMessage("Global mode of the application: " + _appMode, ConsoleColor.Green);
        } else
            ShowMessage("Global mode of the application is not given. Used default: " + _appMode, ConsoleColor.Red);

        if(options.TryGetValue("pathInputExcel", out var pathInputExcel)) {
            _pathInputExcel = pathInputExcel;
            ShowMessage("Name of Excel file: " + _pathInputExcel, ConsoleColor.Green);
        } else
            ShowMessage("Name of Excel file is not given. Used default: " + _pathInputExcel, ConsoleColor.Red);

        if(options.TryGetValue("sheetName", out var sheetName)) {
            _sheetName = sheetName;
            ShowMessage("Name of sheet in the Excel file: " + _sheetName, ConsoleColor.Green);
        } else
            ShowMessage("Name of sheet in the Excel file is not given. Used default: " + _sheetName, ConsoleColor.Red);

        if(options.TryGetValue("columnPositions", out var columnPositions)) {
            if(columnPositions is "auto" or "-" or "autoNumbering" or "auto-numbering" or "default") {
                ShowMessage($"Column with string positions: {columnPositions} => the application will use auto-numbering", ConsoleColor.Green);
                _columnPositions = "auto";
            } else {
                _columnPositions = columnPositions;
                ShowMessage("Column with ids: " + _columnPositions, ConsoleColor.Green);
            }
        } else
            ShowMessage("Column with string positions is not given. Used default: " + _columnPositions, ConsoleColor.Red);

        string refinementOfPhrase = (_appMode == AppMode.combineTwoColumns) ? "original" : "extracted";
        if(options.TryGetValue("columnTexts", out var columnTexts)) {
            _columnTexts = columnTexts;
            ShowMessage($"Column with {refinementOfPhrase} texts: {_columnTexts}", ConsoleColor.Green);
        } else
            ShowMessage($"Column with {refinementOfPhrase} texts is not given. Used default: {_columnTexts}", ConsoleColor.Red);

        if(_appMode == AppMode.combineTwoColumns) {
            if(options.TryGetValue("columnTextsOverlay", out var columnTextsOverlay)) {
                _columnTextsOverlay = columnTextsOverlay;
                ShowMessage("Column with overlay texts: " + _columnTextsOverlay, ConsoleColor.Green);
            } else
                ShowMessage("Column with overlay texts is not given. Used default: " + _columnTextsOverlay, ConsoleColor.Red);
        } else
            _columnTextsOverlay = "";

        if(options.TryGetValue("rowRange", out var rowRange)) {
            _rowRange = rowRange;
            ShowMessage("Range of rows to process: " + _rowRange, ConsoleColor.Green);
        } else
            ShowMessage("Range of rows is not given. Used default: " + _rowRange, ConsoleColor.Red);

        if(options.TryGetValue("cellIgnoringMark", out var cellIgnoringMark)) {
            if(_cellIgnoringMark == "doNotUseCellIgnoring") {
                ShowMessage($"Parameter '{cellIgnoringMark}' => Option for ignoring of cells with certain contents will not be used", ConsoleColor.Green);
                _cellIgnoringMark = null;
            } else {
                _cellIgnoringMark = cellIgnoringMark;
                ShowMessage($"The contents of a cell indicating that the cell has to be ignored: >{_cellIgnoringMark}<", ConsoleColor.Green);
            }
        } else
            ShowMessage($"The contents of a cell indicating that the cell has to be ignored is not given. Used default: >{_cellIgnoringMark}<", ConsoleColor.Red);

        if(options.TryGetValue("pathOutputExcel", out var pathOutputExcel)) {
            _pathOutputExcel = pathOutputExcel;
            ShowMessage("Name of output Excel file: " + _pathOutputExcel, ConsoleColor.Green);
        } else
            ShowMessage("Name of output Excel file is not given. Used default: " + _pathOutputExcel, ConsoleColor.Red);

        if(options.TryGetValue("sheetNameOutput", out var sheetNameOutput)) {
            _sheetNameOutput = sheetNameOutput;
            ShowMessage("Name of sheet in the output Excel file: " + _sheetNameOutput, ConsoleColor.Green);
        } else
            ShowMessage("Name of sheet in the output Excel file is not given. Used default: " + _sheetNameOutput, ConsoleColor.Red);
        if(_sheetNameOutput == "copyInputSheet") {
            _sheetNameOutput = _sheetName;
            ShowMessage($"     (That means their will be used the sheet '{_sheetNameOutput}')");
        }

        if(options.TryGetValue("columnTextsOutput", out var columnTextsOutput)) {
            _columnTextsOutput = columnTextsOutput;
            ShowMessage($"Column in the output Excel for extracted texts: {_columnTextsOutput}", ConsoleColor.Green);
        } else
            ShowMessage($"Column in the output Excel for extracted texts is not given. Used default: {_columnTextsOutput}", ConsoleColor.Red);
        if(_columnTextsOutput == "copyInputColumn") {
            _columnTextsOutput = _columnTexts;
            ShowMessage($"     (That means their will be used the column '{_columnTextsOutput}')");
        }

        if(options.TryGetValue("headerDepth", out var headerDepth)) {
            if(int.TryParse(headerDepth, out var headerDepthInt) && headerDepthInt >= 0) {
                _headerDepth = headerDepthInt;
                ShowMessage($"Number of rows in header of the output Excel: {_headerDepth}", ConsoleColor.Green);
            } else
                ShowMessage($"Invalid value for 'headerDepth': {headerDepth}. Used default: {_headerDepth}", ConsoleColor.Red);
        } else
            ShowMessage($"Number of rows in header of the output Excel is not given. Used default: {_headerDepth}", ConsoleColor.Red);
    }


    public (AppMode appMode, string pathInputExcel, string sheetName, string columnPositions,
        string columnTexts, string columnTextsOverlay, string rowRange, string? cellIgnoringMark,
        string pathOutputExcel, string sheetNameOutput, string columnTextsOutput, int headerDepth)
        GetParameters() =>
        (_appMode, _pathInputExcel, _sheetName, _columnPositions, _columnTexts, _columnTextsOverlay,
        _rowRange, _cellIgnoringMark, _pathOutputExcel, _sheetNameOutput, _columnTextsOutput, _headerDepth);


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
using ExtractorExcelToText.DataAccess;
using System.Text;

namespace ExtractorExcelToText.App;

public class ConsoleUserInteraction : IUserInteraction
{
    private AppMode _appMode = AppMode.extractOneColumn;   //  AppMode.extractOneColumn / AppMode.combineTwoColumns
    private string _pathInputExcel = @"Data\Test_Excel.xlsx";
    private string _sheetName = "TestSheet";               // "Amino Acids"
    private string _columnPositions = "auto";
    private string _columnTexts = "C";
    private string _columnTextsOverlay = "H";
    private string _rowRange = "2:11";                     // "3:5,10,14:16";
    private string? _cellIgnoringMark = "";
    private WritingMode _writingMode = WritingMode.modeCreateNew;  // WritingMode.modeCreateNew / WritingMode.modeOverlay;
    private string _pathTxt = @"Data\Test_Output.txt";
    private bool _emptyLineAtEnd = true;
    private Encoding _encoding = Encoding.Default;


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

        if(options.TryGetValue("writingMode", out var writingMode)) {
            _writingMode = (WritingMode)Enum.Parse(typeof(WritingMode), writingMode);
            ShowMessage("Writng mode: " + _writingMode, ConsoleColor.Green);
        } else
            ShowMessage("Writng mode is not given. Used default: " + _writingMode, ConsoleColor.Red);

        refinementOfPhrase = (_writingMode == WritingMode.modeCreateNew) ? "to be created" : "to impose result";
        if(options.TryGetValue("pathTxt", out var pathTxt)) {
            _pathTxt = pathTxt;
            ShowMessage($"Name of text file {refinementOfPhrase}:" + _pathTxt, ConsoleColor.Green);
        } else
            ShowMessage($"Name of text file {refinementOfPhrase} is not given. Used default: " + _pathTxt, ConsoleColor.Red);

        if(options.TryGetValue("emptyLineAtEnd", out var emptyLineAtEnd)) {
            _emptyLineAtEnd = bool.Parse(emptyLineAtEnd);
            ShowMessage(@"Add an additional empty line to the end of the file?: " + _emptyLineAtEnd, ConsoleColor.Green);
        } else
            ShowMessage(@"Parameter whether to add an additional empty line to the end is not given. Used default: " + _emptyLineAtEnd, ConsoleColor.Red);

        if(options.TryGetValue("encoding", out var encoding)) {
            if(encoding is "default" or "-" or "auto" or "defaultEncoding") {
                _encoding = Encoding.Default;
                ShowMessage($"Encoding for the text file: {encoding} => the application will used default encoding ({_encoding})", ConsoleColor.Green);
            } else {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                _encoding = int.TryParse(encoding, out int codingAsInt)
                    ? Encoding.GetEncoding(codingAsInt)
                    : Encoding.GetEncoding(encoding);
                ShowMessage($"Encoding for the text file: {encoding} => {_encoding}", ConsoleColor.Green);
            }
        } else
            ShowMessage("Encoding for the text file is not given. Used default: " + _encoding, ConsoleColor.Red);
    }


    public (AppMode appMode, string pathInputExcel, string sheetName, string columnPositions, string columnTexts, string columnTextsOverlay,
        string rowRange, string? cellIgnoringMark, WritingMode writingMode, string pathTxt, bool emptyLineAtEnd, Encoding encoding)
        GetParameters() =>
        (_appMode, _pathInputExcel, _sheetName, _columnPositions, _columnTexts, _columnTextsOverlay,
        _rowRange, _cellIgnoringMark, _writingMode, _pathTxt, _emptyLineAtEnd, _encoding);


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
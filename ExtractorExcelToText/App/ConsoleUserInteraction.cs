using ExtractorExcelToText.DataAccess;
using System.Text;

namespace ExtractorExcelToText.App;

public class ConsoleUserInteraction : IUserInteraction
{
    private AppMode _appMode = AppMode.extractOneColumn;   // AppMode.extractOneColumn / AppMode.combineTwoColumns
    private string _pathExcelInput = @"Data\Test_Input.xlsx";
    private string _sheetInput = "TestSheet";              // "Amino Acids"
    private string _columnPositions = "auto";              // "auto", "autoNumbering", "default", "-"
    private string _columnTextsInput = "C";
    private string _columnTextsOverlay = "H";
    private bool _preliminarySortSheetByColumnPositions = false;
    private int _headerDepthInput = 1;
    private string _rowRangeInput = "2:11";                // "2:11", "3:5,10,14:16", ":", ":6"
    private List<string> _cellIgnoringMarksAsList = [""];  // [""],  for "doNotUseCellIgnoring" set new List<string>()
    private WritingMode _writingMode = WritingMode.modeCreateNew;  // WritingMode.modeCreateNew / WritingMode.modeOverlay;
    private string _pathTxtOutput = @"Data\Test_Output.txt";
    private int _headerDepthOutput = 0;
    private OutputOrderMode _outputOrderMode = OutputOrderMode.outputOrderAccordingToPositions;  //outputOrderAccordingToPositions, outputOrderShiftToHeader, outputOrderCompressed
    private bool _considerStartingIgnoredCellsAsPositionsShift = true;
    private bool _emptyLineAtEnd = true;
    private Encoding _encoding = Encoding.Default;
    private bool _closeAppAfterExecution = false;

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
            if(Enum.TryParse<AppMode>(appMode, true, out AppMode parsedMode) && Enum.IsDefined(typeof(AppMode), parsedMode)) {
                _appMode = parsedMode;
                ShowMessage("Global mode of the application: " + _appMode, ConsoleColor.Green);
            } else
                throw new ArgumentException($"Invalid value for parameter 'appMode': {appMode}. " +
                                            $"Use one of the following values: {string.Join(" / ", Enum.GetNames(typeof(AppMode)))}.");
        } else
            ShowMessage("Global mode of the application is not given. Used default: " + _appMode, ConsoleColor.DarkGray);

        if(options.TryGetValue("pathExcelInput", out var pathExcelInput)) {
            _pathExcelInput = pathExcelInput;
            ShowMessage("Name of Excel file: " + _pathExcelInput, ConsoleColor.Green);
        } else
            ShowMessage("Name of Excel file is not given. Used default: " + _pathExcelInput, ConsoleColor.DarkGray);

        if(options.TryGetValue("sheetInput", out var sheetInput)) {
            _sheetInput = sheetInput;
            ShowMessage("Name of sheet in the Excel file: " + _sheetInput, ConsoleColor.Green);
        } else
            ShowMessage("Name of sheet in the Excel file is not given. Used default: " + _sheetInput, ConsoleColor.DarkGray);

        if(options.TryGetValue("columnPositions", out var columnPositions)) {
            if(columnPositions is "auto" or "-" or "autoNumbering" or "auto-numbering" or "default") {
                ShowMessage($"Column with string positions: {columnPositions} => the application will use auto-numbering", ConsoleColor.Green);
                _columnPositions = "auto";
            } else {
                _columnPositions = columnPositions;
                ShowMessage("Column with string positions: " + _columnPositions, ConsoleColor.Green);
            }
        } else
            ShowMessage("Column with string positions is not given. Used default: " + _columnPositions, ConsoleColor.DarkGray);

        string refinementOfPhrase = (_appMode == AppMode.combineTwoColumns) ? "original" : "extracted";
        if(options.TryGetValue("columnTextsInput", out var columnTextsInput)) {
            _columnTextsInput = columnTextsInput;
            ShowMessage($"Column with {refinementOfPhrase} texts: {_columnTextsInput}", ConsoleColor.Green);
        } else
            ShowMessage($"Column with {refinementOfPhrase} texts is not given. Used default: {_columnTextsInput}", ConsoleColor.DarkGray);

        if(_appMode == AppMode.combineTwoColumns) {
            if(options.TryGetValue("columnTextsOverlay", out var columnTextsOverlay)) {
                _columnTextsOverlay = columnTextsOverlay;
                ShowMessage("Column with overlay texts: " + _columnTextsOverlay, ConsoleColor.Green);
            } else
                ShowMessage("Column with overlay texts is not given. Used default: " + _columnTextsOverlay, ConsoleColor.DarkGray);
        } else
            _columnTextsOverlay = "";

        if(options.TryGetValue("preliminarySortSheetByColumnPositions", out var preliminarySortSheetByColumnPositions)) {
            _preliminarySortSheetByColumnPositions = bool.Parse(preliminarySortSheetByColumnPositions);
            ShowMessage(@"Sort the sheet by columnPositions before taking rowRangeInput: " + _preliminarySortSheetByColumnPositions, ConsoleColor.Green);
        } else
            ShowMessage(@"Parameter whether to sort the sheet by columnPositions before taking rowRangeInput is not given. Used default: " + _preliminarySortSheetByColumnPositions, ConsoleColor.DarkGray);

        if(options.TryGetValue("headerDepthInput", out var headerDepthInput)) {
            if(int.TryParse(headerDepthInput, out int headerDepthInt) && headerDepthInt >= 0) {
                _headerDepthInput = headerDepthInt;
                ShowMessage($"Number of rows in header of the input Excel: {_headerDepthInput}", ConsoleColor.Green);
            } else
                throw new ArgumentException($"Invalid value for parameter 'headerDepthInput': {headerDepthInput}. It should be a non-negative integer.");
        } else
            ShowMessage($"Number of rows in header of the input Excel is not given. Used default: {_headerDepthInput}", ConsoleColor.DarkGray);

        if(options.TryGetValue("rowRangeInput", out var rowRangeInput)) {
            _rowRangeInput = rowRangeInput.Trim();
            ShowMessage("Range of input rows to process: " + _rowRangeInput, ConsoleColor.Green);
        } else
            ShowMessage("Range of input rows is not given. Used default: " + _rowRangeInput, ConsoleColor.DarkGray);

        if(options.TryGetValue("cellIgnoringMark", out var cellIgnoringMark)) {
            if(cellIgnoringMark == "doNotUseCellIgnoring") {
                _cellIgnoringMarksAsList = [];
                ShowMessage($"Parameter '{cellIgnoringMark}' => Option for ignoring of cells with certain contents will not be used", ConsoleColor.Green);
            } else {
                _cellIgnoringMarksAsList = [cellIgnoringMark];
                ShowMessage($"The contents of a cell indicating that the cell has to be ignored: >{cellIgnoringMark}<", ConsoleColor.Green);

                for(int i = 2; i <= 100; i++) {
                    if(options.TryGetValue($"cellIgnoringMark{i}", out var additionalIgnoringMark))
                        if(additionalIgnoringMark == "doNotUseCellIgnoring")
                            break;
                        else {
                            _cellIgnoringMarksAsList.Add(additionalIgnoringMark);
                            ShowMessage($"    Additional cell contents for ignoring (#{i}): >{additionalIgnoringMark}<", ConsoleColor.Green);
                        }
                    else
                        break;
                }
            }
        } else {
            string defaultMark = (_cellIgnoringMarksAsList.Count()) switch {
                0 => "doNotUseCellIgnoring",
                1 => ">" + _cellIgnoringMarksAsList[0] + "<",
                _ => "[>" + string.Join("<, >", _cellIgnoringMarksAsList) + "<]"
            };
            ShowMessage($"The contents of a cell indicating that the cell has to be ignored is not given. Used default: " + defaultMark, ConsoleColor.DarkGray);
        }

        if(options.TryGetValue("writingMode", out var writingMode)) {
            if(Enum.TryParse<WritingMode>(writingMode, true, out WritingMode parsedMode) && Enum.IsDefined(typeof(WritingMode), parsedMode)) {
                _writingMode = parsedMode;
                ShowMessage("Writng mode: " + _writingMode, ConsoleColor.Green);
            } else
                throw new ArgumentException($"Invalid value for parameter 'writingMode': {writingMode}. " +
                                            $"Use one of the following values: {string.Join(" / ", Enum.GetNames(typeof(WritingMode)))}.");
        } else
            ShowMessage("Writng mode is not given. Used default: " + _writingMode, ConsoleColor.DarkGray);

        refinementOfPhrase = (_writingMode == WritingMode.modeCreateNew) ? "to be created" : "to impose result";
        if(options.TryGetValue("pathTxtOutput", out var pathTxtOutput)) {
            _pathTxtOutput = pathTxtOutput;
            ShowMessage($"Name of text file {refinementOfPhrase}:" + _pathTxtOutput, ConsoleColor.Green);
        } else
            ShowMessage($"Name of text file {refinementOfPhrase} is not given. Used default: " + _pathTxtOutput, ConsoleColor.DarkGray);

        if(options.TryGetValue("headerDepthOutput", out var headerDepthOutput)) {
            if(int.TryParse(headerDepthOutput, out int headerDepthInt)) {
                _headerDepthOutput = headerDepthInt;
                ShowMessage($"Number of rows in header of the output text file: {_headerDepthOutput}", ConsoleColor.Green);
            } else
                throw new ArgumentException($"Invalid value for parameter 'headerDepthOutput': {headerDepthOutput}. It should be an integer.");
        } else
            ShowMessage($"Number of rows in header of the output text file is not given. Used default: {_headerDepthOutput}", ConsoleColor.DarkGray);

        if(options.TryGetValue("outputOrderMode", out var outputOrderMode)) {
            if(Enum.TryParse<OutputOrderMode>(outputOrderMode, true, out OutputOrderMode parsedMode) && Enum.IsDefined(typeof(OutputOrderMode), parsedMode)) {
                _outputOrderMode = parsedMode;
                ShowMessage("Mode determing order of line output: " + _outputOrderMode, ConsoleColor.Green);
            } else
                throw new ArgumentException($"Invalid value for parameter 'outputOrderMode': {outputOrderMode}. " +
                                            $"Use one of the following values: {string.Join(" / ", Enum.GetNames(typeof(OutputOrderMode)))}.");
        } else
            ShowMessage("Mode determing order of line output is not given. Used default: " + _outputOrderMode, ConsoleColor.DarkGray);

        if(_outputOrderMode == OutputOrderMode.outputOrderShiftToHeader || _outputOrderMode == OutputOrderMode.outputOrderCompressed) {
            if(options.TryGetValue("considerStartingIgnoredCellsAsPositionsShift", out var considerStartingIgnoredCellsAsPositionsShift)) {
                if(bool.TryParse(considerStartingIgnoredCellsAsPositionsShift, out bool parsedValue)) {
                    _considerStartingIgnoredCellsAsPositionsShift = parsedValue;
                    ShowMessage(@"Consider starting ignored cells as positions shift: " + _considerStartingIgnoredCellsAsPositionsShift, ConsoleColor.Green);
                } else
                    throw new ArgumentException($"Invalid value for parameter 'considerStartingIgnoredCellsAsPositionsShift': {considerStartingIgnoredCellsAsPositionsShift}. It should be a boolean.");
            } else
                ShowMessage(@"Parameter whether to consider starting ignored cells as positions shift is not given. Used default: " + _considerStartingIgnoredCellsAsPositionsShift, ConsoleColor.DarkGray);
        }

        if(options.TryGetValue("emptyLineAtEnd", out var emptyLineAtEnd)) {
            if(bool.TryParse(emptyLineAtEnd, out bool parsedValue)) {
                _emptyLineAtEnd = parsedValue;
                ShowMessage(@"Add an additional empty line to the end of the file?: " + _emptyLineAtEnd, ConsoleColor.Green);
            } else
                throw new ArgumentException($"Invalid value for parameter 'emptyLineAtEnd': {emptyLineAtEnd}. It should be a boolean.");
        } else
            ShowMessage(@"Parameter whether to add an additional empty line to the end is not given. Used default: " + _emptyLineAtEnd, ConsoleColor.DarkGray);

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
            ShowMessage("Encoding for the text file is not given. Used default: " + _encoding, ConsoleColor.DarkGray);

        if(options.TryGetValue("closeAppAfterExecution", out var closeAppAfterExecution)) {
            if(bool.TryParse(closeAppAfterExecution, out bool parsedValue)) {
                _closeAppAfterExecution = parsedValue;
                ShowMessage(@"Close the application after execution without waiting for user confirmation: " + _closeAppAfterExecution, ConsoleColor.Green);
            } else
                throw new ArgumentException($"Invalid value for parameter 'closeAppAfterExecution': {closeAppAfterExecution}. It should be a boolean.");
        } else
            ShowMessage(@"Parameter whether to close the application after execution without confirmation is not given. Used default: " + _closeAppAfterExecution, ConsoleColor.DarkGray);
    }


    public (AppMode appMode, string pathExcelInput, string sheetInput, string columnPositions, string columnTextsInput, string columnTextsOverlay,
        bool preliminarySortSheetByColumnPositions, int headerDepthInput, string rowRangeInput, string[] cellIgnoringMarks,
        WritingMode writingMode, string pathTxtOutput, int headerDepthOutput, OutputOrderMode outputOrderMode,
        bool considerStartingIgnoredCellsAsPositionsShift, bool emptyLineAtEnd, Encoding encoding, bool closeAppAfterExecution)
        GetParameters() =>
        (_appMode, _pathExcelInput, _sheetInput, _columnPositions, _columnTextsInput, _columnTextsOverlay,
        _preliminarySortSheetByColumnPositions, _headerDepthInput, _rowRangeInput, _cellIgnoringMarksAsList.ToArray(),
        _writingMode, _pathTxtOutput, _headerDepthOutput, _outputOrderMode, _considerStartingIgnoredCellsAsPositionsShift, _emptyLineAtEnd, _encoding, _closeAppAfterExecution);


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
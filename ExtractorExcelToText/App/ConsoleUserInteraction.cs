using ExtractorExcelToText.DataAccess;
using System.Text;

namespace ExtractorExcelToText.App;

public class ConsoleUserInteraction : IUserInteraction
{
    private AppMode _appMode = AppMode.combineTwoColumns;   //  AppMode.extractOneColumn / AppMode.combineTwoColumns
    private string _pathInputExcel = @"Data\Test_Excel.xlsx";
    private string _sheetName = "Amino Acids";  // "TestSheet"
    private string _columnPositions = "auto";         // "auto" / "A"
    private string _columnTexts = "B";
    private string _columnTextsOverlay = "H";   // "C"
    private string _rowRange = "2:24";          // "2:11"
    private string? _cellIgnoringMark = "";
    private WritingMode _writingMode = WritingMode.modeOverlay;  // WritingMode.modeCreateNew / WritingMode.modeOverlay;
    private string _pathTxt = @"Data\Test_Output.txt";
    private Encoding _encoding = Encoding.Default;

    public ConsoleUserInteraction(string[] args, string appTitle = "ExtractorExcelToText")
    {
        Console.Title = appTitle;

        try {
            _appMode = (AppMode)Enum.Parse(typeof(AppMode), args[0]);
            ShowMessage("Global mode of the application: " + _appMode, ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage("Global mode of the application is not given. Used default: " + _appMode, ConsoleColor.Red);
        }

        try {
            _pathInputExcel = args[1];
            ShowMessage("Name of Excel file: " + _pathInputExcel, ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage("Name of Excel file is not given. Used default: " + _pathInputExcel, ConsoleColor.Red);
        }

        try {
            _sheetName = args[2];
            ShowMessage("Name of sheet in the Excel file: " + _sheetName, ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage("Name of sheet in the Excel file is not given. Used default: " + _sheetName, ConsoleColor.Red);
        }

        try {
            _columnPositions = args[3];
            if (_columnPositions is "auto" or "-" or "default" or "autoNumbering") {
                ShowMessage($"Column with string positions: {_columnPositions} => the application will use auto-numbering", ConsoleColor.Green);
                _columnPositions = "auto";
            } else
                ShowMessage("Column with ids: " + _columnPositions, ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage("Column with string positions is not given. The application will use auto-numbering.", ConsoleColor.Red);
        }

        string refinementOfPhrase = (_appMode == AppMode.combineTwoColumns) ? "original" : "extracted";
        try {
            _columnTexts = args[4];
            ShowMessage($"Column with {refinementOfPhrase} texts: {_columnTexts}", ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage($"Column with {refinementOfPhrase} texts is not given. Used default: {_columnTexts}", ConsoleColor.Red);
        }

        int parameterCount = 4;
        if(_appMode == AppMode.combineTwoColumns) {
            try {
                _columnTextsOverlay = args[++parameterCount];
                ShowMessage("Column with overlay texts: " + _columnTextsOverlay, ConsoleColor.Green);
            } catch(IndexOutOfRangeException) {
                ShowMessage("Column with overlay texts is not given. Used default: " + _columnTextsOverlay, ConsoleColor.Red);
            }
        }

        try {
            _rowRange = args[++parameterCount];
            ShowMessage("Range of rows to process: " + _rowRange, ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage("Range of rows is not given. Used default: " + _rowRange, ConsoleColor.Red);
        }

        try {
            _cellIgnoringMark = args[++parameterCount];
            if(_cellIgnoringMark == "doNotUseCellIgnoring") {
                ShowMessage($"Parameter '{_cellIgnoringMark}' => Option for ignoring of cells with certain contents will not be used", ConsoleColor.Green);
                _cellIgnoringMark = null;
            } else
                ShowMessage($"The contents of a cell indicating that the cell has to be ignored: >{_cellIgnoringMark}<", ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage($"The contents of a cell indicating that the cell has to be ignored is not given. Used default: >{_cellIgnoringMark}<", ConsoleColor.Red);
        }

        try {
            _writingMode = (WritingMode)Enum.Parse(typeof(WritingMode), args[++parameterCount]);
            ShowMessage("Writng mode: " + _writingMode, ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage("Writng mode is not given. Used default: " + _writingMode, ConsoleColor.Red);
        }

        refinementOfPhrase = (_writingMode == WritingMode.modeCreateNew) ? "to be created" : "to impose result";
        try {
            _pathTxt = args[++parameterCount];
            ShowMessage($"Name of text file {refinementOfPhrase}:" + _pathTxt, ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage($"Name of text file {refinementOfPhrase} is not given. Used default: " + _pathTxt, ConsoleColor.Red);
        }

        try {
            string stEncoding = args[++parameterCount];
            if(stEncoding is "default" or "-" or "auto" or "0" or "defaultEncoding") {
                _encoding = Encoding.Default;
                ShowMessage($"Encoding for the text file: {stEncoding} => the application will used default encoding ({_encoding})", ConsoleColor.Green);
            } else {
                _encoding = Encoding.GetEncoding(stEncoding);
                ShowMessage($"Encoding for the text file: {stEncoding} => {_encoding}", ConsoleColor.Green);
            }
        } catch(IndexOutOfRangeException) {
            _encoding = Encoding.Default;
            ShowMessage("Encoding for the text file is not given. Used default: " + _encoding, ConsoleColor.Red);
        }
    }


    public AppMode GetAppMode() =>
        _appMode;


    public (string pathInputExcel, string sheetName, string columnPositions, string columnTexts, string rowRange, string? cellIgnoringMark,
        WritingMode writingMode, string pathTxt, Encoding encoding)
        GetParametersForModeExtractOneColumn() =>
        (_pathInputExcel, _sheetName, _columnPositions, _columnTexts, _rowRange, _cellIgnoringMark, _writingMode, _pathTxt, _encoding);


    public (string pathInputExcel, string sheetName, string columnPositions, string columnTexts, string columnTextsOverlay, string rowRange, string? cellIgnoringMark,
        WritingMode writingMode, string pathTxt, Encoding encoding)
        GetParametersForModeCombineTwoColumns() =>
        (_pathInputExcel, _sheetName, _columnPositions, _columnTexts, _columnTextsOverlay, _rowRange, _cellIgnoringMark, _writingMode, _pathTxt, _encoding);


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



//temp:

//if(_appMode is not AppMode.extractOneColumn or AppMode.combineTwoColumns)
//    throw new ArgumentException("Unsupported global mode: " + _appMode);


//if(_writingMode is not WritingMode.modeCreateNew or WritingMode.modeOverlay)
//    throw new ArgumentException("Unsupported writing mode: " + _appMode);


//Console.WriteLine($"CellIgnoringMark: >{_cellIgnoringMark}<");


//int parRowRange = 5;
//int parCellIgnoringMark = 6;
//int parWritingMode = 7;
//int parPathTxt = 8;
//int parAddEmptyLineToEnd = 9;
//int parEncoding = 10;

//parRowRange += 1;
//parCellIgnoringMark += 1;
//parWritingMode += 1;
//parPathTxt += 1;
//parEncoding += 1;
//parAddEmptyLineToEnd += 1;
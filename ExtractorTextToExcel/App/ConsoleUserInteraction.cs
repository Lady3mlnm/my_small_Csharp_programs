using System.Text;

namespace ExtractorTextToExcel.App;

public class ConsoleUserInteraction : IUserInteraction
{
    private string _pathTxtInput = @"Data\Test_Input.txt";           // @"Data\Test_Input.txt";  @"Data\Test_French_numbers.txt";   
    private string _stringRange = "1:10";                            // "1:10", "2:4,9,13:15", ":", ":6"
    private Encoding _encoding = Encoding.Default;
    private List<string> _stringIgnoringMarksAsList = [""];   // [""],  for "doNotUseStringIgnoring" set new List<string>()
    private string _pathExcelOutput = @"Data\Test_Output.xlsx";
    private string _sheetOutput = "Storage";
    private string _columnTextsOutput = "B";
    private int _headerDepthOutput = 1;
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

        if(options.TryGetValue("pathTxtInput", out var pathTxtInput)) {
            _pathTxtInput = pathTxtInput;
            ShowMessage($"Name of input text file:" + _pathTxtInput, ConsoleColor.Green);
        } else
            ShowMessage($"Name of input text file is not given. Used default: " + _pathTxtInput, ConsoleColor.DarkGray);

        if(options.TryGetValue("stringRange", out var stringRange)) {
            _stringRange = stringRange.Trim();
            ShowMessage("Range of strings to process: " + _stringRange, ConsoleColor.Green);
        } else
            ShowMessage("Range of string is not given. Used default: " + _stringRange, ConsoleColor.DarkGray);

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

        if(options.TryGetValue("stringIgnoringMark", out var stringIgnoringMark)) {
            if(stringIgnoringMark == "doNotUseStringIgnoring") {
                _stringIgnoringMarksAsList = [];
                ShowMessage($"Parameter '{stringIgnoringMark}' => Option for ignoring of strings with certain contents will not be used", ConsoleColor.Green);
            } else {
                _stringIgnoringMarksAsList = [stringIgnoringMark];
                ShowMessage($"The contents of a string indicating that the string has to be ignored: >{stringIgnoringMark}<", ConsoleColor.Green);

                for(int i = 2; i <= 100; i++) {
                    if(options.TryGetValue($"stringIgnoringMark{i}", out var additionalIgnoringMark))
                        if(additionalIgnoringMark == "doNotUseStringIgnoring")
                            break;
                        else {
                            _stringIgnoringMarksAsList.Add(additionalIgnoringMark);
                            ShowMessage($"    Additional string contents for ignoring (#{i}): >{additionalIgnoringMark}<", ConsoleColor.Green);
                        }
                    else
                        break;
                }
            }
        } else {
            string defaultMark = (_stringIgnoringMarksAsList.Count()) switch {
                0 => "doNotUseStringIgnoring",
                1 => ">" + _stringIgnoringMarksAsList[0] + "<",
                _ => "[>" + string.Join("<, >", _stringIgnoringMarksAsList) + "<]"
            };
            ShowMessage($"The contents of a string indicating that the string has to be ignored is not given. Used default: " + defaultMark, ConsoleColor.DarkGray);
        }

        if(options.TryGetValue("pathExcelOutput", out var pathExcelOutput)) {
            _pathExcelOutput = pathExcelOutput;
            ShowMessage("Name of output Excel file: " + _pathExcelOutput, ConsoleColor.Green);
        } else
            ShowMessage("Name of output Excel file is not given. Used default: " + _pathExcelOutput, ConsoleColor.DarkGray);

        if(options.TryGetValue("sheetOutput", out var sheetOutput)) {
            _sheetOutput = sheetOutput;
            ShowMessage("Name of sheet in the output Excel file: " + _sheetOutput, ConsoleColor.Green);
        } else
            ShowMessage("Name of sheet in the output Excel file is not given. Used default: " + _sheetOutput, ConsoleColor.DarkGray);

        if(options.TryGetValue("columnTextsOutput", out var columnTextsOutput)) {
            _columnTextsOutput = columnTextsOutput;
            ShowMessage($"Column in the output Excel for extracted texts: {_columnTextsOutput}", ConsoleColor.Green);
        } else
            ShowMessage($"Column in the output Excel for extracted texts is not given. Used default: {_columnTextsOutput}", ConsoleColor.DarkGray);

        if(options.TryGetValue("headerDepthOutput", out var headerDepthOutput)) {
            if(int.TryParse(headerDepthOutput, out int headerDepthInt) && headerDepthInt >= 0) {
                _headerDepthOutput = headerDepthInt;
                ShowMessage($"Number of rows in header of the output Excel: {_headerDepthOutput}", ConsoleColor.Green);
            } else
                throw new ArgumentException($"Invalid value for parameter 'headerDepthOutput': {headerDepthOutput}. It should be a non-negative integer.");
        } else
            ShowMessage($"Number of rows in header of the output Excel is not given. Used default: {_headerDepthOutput}", ConsoleColor.DarkGray);

        if(options.TryGetValue("closeAppAfterExecution", out var closeAppAfterExecution)) {
            if(bool.TryParse(closeAppAfterExecution, out bool parsedValue)) {
                _closeAppAfterExecution = parsedValue;
                ShowMessage(@"Close the application after execution without waiting for user confirmation: " + _closeAppAfterExecution, ConsoleColor.Green);
            } else
                throw new ArgumentException($"Invalid value for parameter 'closeAppAfterExecution': {closeAppAfterExecution}. It should be a boolean.");
        } else
            ShowMessage(@"Parameter whether to close the application after execution without confirmation is not given. Used default: " + _closeAppAfterExecution, ConsoleColor.DarkGray);
    }


    public (string pathTxtInput, string stringRange, Encoding encoding, string[] stringIgnoringMarks,
        string pathExcelOutput, string sheetOutput, string columnTextsOutput, int headerDepthOutput, bool closeAppAfterExecution)
        GetParameters() =>
        (_pathTxtInput, _stringRange, _encoding, _stringIgnoringMarksAsList.ToArray(),
        _pathExcelOutput, _sheetOutput, _columnTextsOutput, _headerDepthOutput, _closeAppAfterExecution);


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
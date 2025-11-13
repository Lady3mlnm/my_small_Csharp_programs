namespace TextToHexConverter.App;

public class ConsoleUserInteraction : IUserInteraction
{
    private string _pathFileInput = @"Data\Test_Input.txt";
    private string _pathFileOutput = @"Data\Test_Output.txt";
    private string _separatorsHexValues = " ";
    private int _nmbInsertedLinebreaks = 2;
    private bool _isODOAExcludedFromHex = false;

    public ConsoleUserInteraction(string[] args, string appTitle = "TextToHexConverter")
    {
        Console.Title = appTitle;

        try {
            _pathFileInput = args[0];
            ShowMessage("Name of file with original text: " + _pathFileInput, ConsoleColor.Green);
        } catch (IndexOutOfRangeException) {
            ShowMessage("Name of file with original text is not given. Used default: " + _pathFileInput, ConsoleColor.Red);
        }

        try {
            _pathFileOutput = args[1];
            ShowMessage("Name of file to output result: " + _pathFileOutput, ConsoleColor.Green);
        } catch (IndexOutOfRangeException) {
            ShowMessage("Name of file to output result is not given. Used default: " + _pathFileOutput, ConsoleColor.Red);
        }

        try {
            _separatorsHexValues = args[2];
            ShowMessage($"Separator between hex values: \"{_separatorsHexValues}\"", ConsoleColor.Green);
        } catch (IndexOutOfRangeException) {
            ShowMessage($"Separator between hex values is not given. Used default: \"{_separatorsHexValues}\"", ConsoleColor.Red);
        }

        try {
            _nmbInsertedLinebreaks = int.Parse(args[3]);
            ShowMessage("Number of inserted linebreaks: " + _nmbInsertedLinebreaks, ConsoleColor.Green);
        } catch (Exception) {
            ShowMessage("Number of inserted linebreaks is not given. Used default: " + _nmbInsertedLinebreaks, ConsoleColor.Red);
        }

        try {
            _isODOAExcludedFromHex = bool.Parse(args[4]);
            ShowMessage(@"Exclude '\r\n' from text?: " + _isODOAExcludedFromHex, ConsoleColor.Green);
        } catch (Exception) {
            ShowMessage(@"Parameter whether to exclude '\r\n' from text is not given. Used default: " + _isODOAExcludedFromHex, ConsoleColor.Red);
        }
    }


    public (string pathFileInput, string pathFileOutput, string separatorsHexValues, int nmbInsertedLinebreaks, bool isODOAExcludedFromHex) GetParameters()
    {
        return (_pathFileInput, _pathFileOutput, _separatorsHexValues, _nmbInsertedLinebreaks, _isODOAExcludedFromHex);
    }


    public void ShowMessage(string message, bool isLinebreakAdded = true)
    {
        if (isLinebreakAdded)
            Console.WriteLine(message);
        else
            Console.Write(message);
    }


    public void ShowMessage(string message, ConsoleColor color, bool isLinebreakAdded = true)
    {
        Console.ForegroundColor = color;
        if (isLinebreakAdded)
            Console.WriteLine(message);
        else
            Console.Write(message);
        Console.ResetColor();
    }


    public void GetCloseAppConfirmation()
    {
        Console.WriteLine("Press any key to close this application.");
        Console.ReadKey();
    }
}

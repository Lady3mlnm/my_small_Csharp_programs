using BatchSubstitution.DataAccess;

namespace BatchSubstitution.App;

public class ConsoleUserInteraction : IUserInteraction
{
    private string _pathFileInput = @"Data\Test_Input.txt";
    private string _pathFileDictionary = @"Data\Test_Dictionary.tbl";
    private string _separatorsKeyValue = " = ";
    private bool _isOnlyWords = false;
    private DictionaryDirectionFormat _directionInDictionary = DictionaryDirectionFormat.direct;
    private string _pathFileOutput = @"Data\Test_Output.txt";

    public ConsoleUserInteraction(string[] args, string appTitle = "TextToHexConverter")
    {
        Console.Title = appTitle;

        try {
            _pathFileInput = args[0];
            ShowMessage("Name of file with original text: " + _pathFileInput, ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage("Name of file with original text is not given. Used default: " + _pathFileInput, ConsoleColor.Red);
        }

        try {
            _pathFileDictionary = args[1];
            ShowMessage("Name of file with dictionary for replacement: " + _pathFileDictionary, ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage("Name of file to output result is not given. Used default: " + _pathFileDictionary, ConsoleColor.Red);
        }

        try {
            _separatorsKeyValue = args[2];
            ShowMessage($"Separator between keys and values in the dictionary: \"{_separatorsKeyValue}\"", ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage($"Separator between keys and values in the dictionary is not given. Used default: \"{_separatorsKeyValue}\"", ConsoleColor.Red);
        }

        try {
            _isOnlyWords = bool.Parse(args[3]);
            ShowMessage(@"Change only whole words?: " + _isOnlyWords, ConsoleColor.Green);
        } catch(Exception) {
            ShowMessage(@"Parameter whether to change only whole words is not given. Used default: " + _isOnlyWords, ConsoleColor.Red);
        }

        try {
            _directionInDictionary = (DictionaryDirectionFormat)Enum.Parse(typeof(DictionaryDirectionFormat), args[4]);
            ShowMessage($"Direction of replacement: {_directionInDictionary}", ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage($"Direction of replacement is not given. Used default: {_directionInDictionary}", ConsoleColor.Red);
        }

        try {
            _pathFileOutput = args[5];
            ShowMessage("Name of file to output result: " + _pathFileOutput, ConsoleColor.Green);
        } catch(IndexOutOfRangeException) {
            ShowMessage("Name of file to output result is not given. Used default: " + _pathFileOutput, ConsoleColor.Red);
        }
    }


    public (string pathFileInput, string pathFileDictionary, string separatorsKeyValue, bool isOnlyWords,
            DictionaryDirectionFormat directionInDictionary, string pathFileOutput) GetParameters()
    {
        return (_pathFileInput, _pathFileDictionary, _separatorsKeyValue, _isOnlyWords, _directionInDictionary, _pathFileOutput);
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
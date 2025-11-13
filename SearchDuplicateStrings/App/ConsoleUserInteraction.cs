using SearchDuplicateStrings.DataAccess;

namespace SearchDuplicateStrings.App;

public class ConsoleUserInteraction : IUserInteraction
{
    private string _pathFileInput = @"Data\Test_Input.txt";
    private string _pathFileOutput = @"Data\Test_Output.txt";
    private FileOutputFormat _fileOutputFormat = FileOutputFormat.asStrings;

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
            _fileOutputFormat = (FileOutputFormat)Enum.Parse(typeof(FileOutputFormat), args[2]); 
            ShowMessage($"Output file format: {_fileOutputFormat}", ConsoleColor.Green);
        } catch (IndexOutOfRangeException) {
            ShowMessage($"Output file format is not given. Used default: {_fileOutputFormat}", ConsoleColor.Red);
        }
    }


    public (string pathFileInput, string pathFileOutput, FileOutputFormat fileOutputFormat) GetParameters()
    {
        return (_pathFileInput, _pathFileOutput, _fileOutputFormat);
    }


    public void ShowMessage(string message, bool isLinebreakAdded=true)
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

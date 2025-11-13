using SearchDuplicateStrings.DataAccess;
using System.Diagnostics;

namespace SearchDuplicateStrings.App;

public class TextProcessingApp
{
    private readonly IRepository _repository;
    private readonly IUserInteraction _userInteraction;
    private readonly TextProcessingLogic _textProcessingLogic;

    public TextProcessingApp(IRepository repository, IUserInteraction userInteraction, TextProcessingLogic textProcessingLogic)
    {
        _repository = repository;
        _userInteraction = userInteraction;
        _textProcessingLogic = textProcessingLogic;
    }

    public void Run()
    {
        string pathFileInput;
        string pathFileOutput;
        FileOutputFormat fileOutputFormat;

        (pathFileInput, pathFileOutput, fileOutputFormat) = _userInteraction.GetParameters();

        string[] arStrings;
        if (File.Exists(pathFileInput)) {
            arStrings  = _repository.ReadArrayFromRepository(pathFileInput);
        } else {
            _userInteraction.ShowMessage($"\nFile with name '{pathFileInput}'\n({Path.GetFullPath(pathFileInput)})\ndoes not exist",
                                         ConsoleColor.Red);
            _userInteraction.GetCloseAppConfirmation();
            return;
        }

        _userInteraction.ShowMessage('\n' + "First three strings of the loaded original text:");

        _userInteraction.ShowMessage(arStrings[0..Math.Min(3, arStrings.Length)], ConsoleColor.Cyan);

        Stopwatch stopwatch = Stopwatch.StartNew();
        Dictionary<string, int> ddDuplicates = _textProcessingLogic.FindDuplicates(arStrings);
        stopwatch.Stop();

        _userInteraction.ShowMessage('\n' + "Search of duplicate strings complited. ", isLinebreakAdded: false);
        _userInteraction.ShowMessage($"Algorithm runtime : {(double)stopwatch.ElapsedMilliseconds / 1000:F3} sec", ConsoleColor.Yellow);
        _userInteraction.ShowMessage("Number of varieties of duplicate strings found: " + ddDuplicates.Count, ConsoleColor.Yellow);
        _userInteraction.ShowMessage("Three examples of the duplicate strings:");

        var stExampleOfDictionary = ddDuplicates            // approach via LINQ for training, though via foreach loop this would be more readable
            .Take(Math.Min(3, ddDuplicates.Count))
            .Select(pair => $"{pair.Key}   = {pair.Value}");
        _userInteraction.ShowMessage(stExampleOfDictionary, ConsoleColor.Cyan);

        _repository.WriteDictionaryToRepository(pathFileOutput, ddDuplicates, fileOutputFormat);

        _userInteraction.ShowMessage('\n' + "The duplicate strings written to the output file.");
        _userInteraction.GetCloseAppConfirmation();
    }
}



//try {
//    arStrings = _repository.ReadArrayFromRepository(pathFileInput);
//} catch (Exception) {
//    _userInteraction.ShowMessage("\nERROR during reading the file.", ConsoleColor.Red);
//    throw;
//}
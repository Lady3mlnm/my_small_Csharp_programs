using System.Diagnostics;
using TextToHexConverter.DataAccess;

namespace TextToHexConverter.App;

public class TextToHexConverterApp
{
    private readonly IRepository _repository;
    private readonly IUserInteraction _userInteraction;
    private readonly ConversionLogic _conversionLogic;

    public TextToHexConverterApp(IRepository repository, IUserInteraction userInteraction, ConversionLogic conversionLogic)
    {
        _repository = repository;
        _userInteraction = userInteraction;
        _conversionLogic = conversionLogic;
    }

    public void Run()
    {
        string pathFileInput;
        string pathFileOutput;
        string separatorsHexValues;
        int nmbInsertedLinebreaks;
        bool isODOAExcludedFromHex;

        (pathFileInput, pathFileOutput, separatorsHexValues, nmbInsertedLinebreaks, isODOAExcludedFromHex) = _userInteraction.GetParameters();

        string inputContent;
        if (File.Exists(pathFileInput)) {
            inputContent = _repository.ReadRepository(pathFileInput);
        } else {
            _userInteraction.ShowMessage($"\nFile with name '{pathFileInput}'\n({Path.GetFullPath(pathFileInput)})\ndoes not exist",
                                         ConsoleColor.Red);
            _userInteraction.GetCloseAppConfirmation();
            return;
        }

        _userInteraction.ShowMessage('\n' + "First 300 characters of the loaded original text:");
        _userInteraction.ShowMessage(inputContent.Substring(0, Math.Min(inputContent.Length, 300)), ConsoleColor.Cyan);

        Stopwatch stopwatch = Stopwatch.StartNew();
        string outputContent = _conversionLogic.Convert(inputContent, separatorsHexValues, nmbInsertedLinebreaks, isODOAExcludedFromHex);
        stopwatch.Stop();

        _userInteraction.ShowMessage('\n' + "Convertion complited. " , isLinebreakAdded: false);
        _userInteraction.ShowMessage($"Algorithm runtime : {(double)stopwatch.ElapsedMilliseconds / 1000:F3} sec", ConsoleColor.Yellow);
        _userInteraction.ShowMessage("First 300 characters of the transformed string:");
        _userInteraction.ShowMessage(outputContent.Substring(0, Math.Min(outputContent.Length, 300)), ConsoleColor.Cyan);

        _repository.WriteToRepository(pathFileOutput, outputContent);

        _userInteraction.ShowMessage('\n' + "The transformed string written to the output file.");
        _userInteraction.GetCloseAppConfirmation();
    }
}



//try {
//    inputContent = _repository.ReadRepository(pathFileInput);
//} catch (Exception e) {
//    _userInteraction.ShowMessage("\nERROR during reading the file.", ConsoleColor.Red);
//    throw;
//}
using BatchSubstitution.DataAccess;
using Microsoft.VisualBasic;
using System.Diagnostics;

namespace BatchSubstitution.App;

public class BatchSubstitutionApp
{
    private readonly IRepository _repository;
    private readonly IUserInteraction _userInteraction;
    private readonly ConversionLogic _conversionLogic;

    public BatchSubstitutionApp(IRepository repository, IUserInteraction userInteraction, ConversionLogic conversionLogic)
    {
        _repository = repository;
        _userInteraction = userInteraction;
        _conversionLogic = conversionLogic;
    }

    public void Run()
    {
        string pathFileInput;
        string pathFileDictionary;
        string separatorsKeyValue;
        bool isOnlyWords;
        DictionaryDirectionFormat directionInDictionary;
        string pathFileOutput;


        (pathFileInput, pathFileDictionary, separatorsKeyValue, isOnlyWords, directionInDictionary, pathFileOutput) = _userInteraction.GetParameters();

        string inputContent;
        if(File.Exists(pathFileInput)) {
            inputContent = _repository.ReadRepositoryText(pathFileInput);
        } else {
            _userInteraction.ShowMessage($"\nFile with name '{pathFileInput}'\n({Path.GetFullPath(pathFileInput)})\ndoes not exist", ConsoleColor.Red);
            _userInteraction.GetCloseAppConfirmation();
            return;
        }

        _userInteraction.ShowMessage('\n' + "First 300 characters of the loaded original text:");
        _userInteraction.ShowMessage(inputContent.Substring(0, Math.Min(inputContent.Length, 300)), ConsoleColor.Cyan);

        string[] lsDictionaryContent;
        if(File.Exists(pathFileDictionary)) {
            lsDictionaryContent = _repository.ReadRepositoryListStrings(pathFileDictionary);
        } else {
            _userInteraction.ShowMessage($"\nFile with name '{pathFileDictionary}'\n({Path.GetFullPath(pathFileDictionary)})\ndoes not exist", ConsoleColor.Red);
            _userInteraction.GetCloseAppConfirmation();
            return;
        }

        _userInteraction.ShowMessage('\n' + "First three strings of the dictionary file:");
        _userInteraction.ShowMessage(lsDictionaryContent[0..Math.Min(3, lsDictionaryContent.Length)], ConsoleColor.Cyan);


        Stopwatch stopwatch = Stopwatch.StartNew();
        Dictionary<string, string> ddReplacement = _conversionLogic.StringsToDictionary(lsDictionaryContent, separatorsKeyValue, directionInDictionary);

        _userInteraction.ShowMessage('\n' + "First three KeyValuePairs of the extracted replacement dictionary:");

        var stExampleOfDictionary = ddReplacement            // approach via LINQ for training, though via foreach loop this would be more readable
            .Take(Math.Min(3, ddReplacement.Count))
            .Select(pair => $"{pair.Key}: {pair.Value}");
        _userInteraction.ShowMessage(stExampleOfDictionary, ConsoleColor.Cyan);

        string outputContent = _conversionLogic.BatchSubstitutionInText(inputContent, ddReplacement, isOnlyWords);
        stopwatch.Stop();


        _userInteraction.ShowMessage('\n' + "Replacement complited. ", isLinebreakAdded: false);
        _userInteraction.ShowMessage($"Algorithm runtime : {(double)stopwatch.ElapsedMilliseconds / 1000:F3} sec", ConsoleColor.Yellow);
        _userInteraction.ShowMessage("First 300 characters of the transformed text:");
        _userInteraction.ShowMessage(outputContent.Substring(0, Math.Min(outputContent.Length, 300)), ConsoleColor.Cyan);

        _repository.WriteToRepository(pathFileOutput, outputContent);

        _userInteraction.ShowMessage('\n' + "The transformed text written to the output file.");
        _userInteraction.GetCloseAppConfirmation();
    }
}
using ExtractorTextToExcel.DataAccess;
using System.Diagnostics;
using System.Text;

namespace ExtractorTextToExcel.App;

public class ExtractorTextToExcelApp
{
    private readonly IRepository _repository;
    private readonly IUserInteraction _userInteraction;

    public ExtractorTextToExcelApp(IRepository repository, IUserInteraction userInteraction)
    {
        _repository = repository;
        _userInteraction = userInteraction;
    }

    public void Run()
    {
        string pathTxtInput;
        string stringRange;
        Encoding encoding;
        string[] stringIgnoringMarks;
        string pathExcelOutput;
        string sheetOutput;
        string columnTextsOutput;
        int headerDepthOutput;
        bool closeAppAfterExecution;

        Stopwatch stopwatch = Stopwatch.StartNew();

        (pathTxtInput, stringRange, encoding, stringIgnoringMarks,
         pathExcelOutput, sheetOutput, columnTextsOutput, headerDepthOutput, closeAppAfterExecution) =
             _userInteraction.GetParameters();

        string[] stringsFromTxt = _repository.ReadAllStringsFromRepository(pathTxtInput, stringRange, encoding);

        _userInteraction.ShowMessage("\nNumber of strings extracted from the text file (icluding to be ignored): " + stringsFromTxt.Length);
        _userInteraction.ShowMessage("First ten strings:");
        _userInteraction.ShowMessage(stringsFromTxt[0..Math.Min(10, stringsFromTxt.Length)]
                                        .Select((st, index) => $"{index+1}. {st}"),
                                     ConsoleColor.Cyan);

        _repository.WriteStringsToRepository(stringsFromTxt, pathExcelOutput, sheetOutput,
            columnTextsOutput, headerDepthOutput, stringIgnoringMarks);

        _userInteraction.ShowMessage('\n' + "The app completed its work.");

        stopwatch.Stop();
        _userInteraction.ShowMessage($"Total time of the application work : {(double)stopwatch.ElapsedMilliseconds / 1000:F3} sec", ConsoleColor.Yellow);

        if(!closeAppAfterExecution)
            _userInteraction.GetCloseAppConfirmation();
    }
}
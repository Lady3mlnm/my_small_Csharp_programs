using ExtractorExcelToExcel.DataAccess;
using ExtractorExcelToExcel.DataStructures;
using System.Diagnostics;

namespace ExtractorExcelToExcel.App;

public class ExtractorExcelToExcelApp
{
    private readonly IRepository _repository;
    private readonly IUserInteraction _userInteraction;

    public ExtractorExcelToExcelApp(IRepository repository, IUserInteraction userInteraction)
    {
        _repository = repository;
        _userInteraction = userInteraction;
    }

    public void Run()
    {
        AppMode appMode;
        string pathExcelInput;
        string sheetInput;
        string columnPositions;
        string columnTextsInput;
        string columnTextsOverlay;
        bool preliminarySortSheetByColumnPositions;
        int headerDepthInput;
        string rowRange;
        string[] cellIgnoringMarks;
        string pathExcelOutput;
        string sheetOutput;
        string columnTextsOutput;
        int headerDepthOutput;
        OutputOrderMode outputOrderMode;
        bool closeAppAfterExecution;

        Stopwatch stopwatch = Stopwatch.StartNew();

        (appMode, pathExcelInput, sheetInput, columnPositions, columnTextsInput,
         columnTextsOverlay, preliminarySortSheetByColumnPositions, headerDepthInput, rowRange, cellIgnoringMarks,
         pathExcelOutput, sheetOutput, columnTextsOutput, headerDepthOutput, outputOrderMode, closeAppAfterExecution) =
             _userInteraction.GetParameters();

        Record[] records =
            _repository.ReadRecordsFromRepository(pathExcelInput, appMode, sheetInput,
                                                  columnPositions, columnTextsInput, columnTextsOverlay,
                                                  preliminarySortSheetByColumnPositions, headerDepthInput, rowRange, cellIgnoringMarks);

        _userInteraction.ShowMessage("\nNumber of strings extracted from Excel: " + records.Length);
        _userInteraction.ShowMessage("First ten position-string pairs:");
        _userInteraction.ShowMessage(records[0..Math.Min(10, records.Length)].Select(record => record.ToString()),
                                     ConsoleColor.Cyan);

        _repository.WriteRecordsToRepository(records, pathExcelOutput, sheetOutput, columnTextsOutput, headerDepthOutput, outputOrderMode);

        _userInteraction.ShowMessage('\n' + "The app completed its work.");

        stopwatch.Stop();
        _userInteraction.ShowMessage($"Total time of the application work : {(double)stopwatch.ElapsedMilliseconds / 1000:F3} sec", ConsoleColor.Yellow);

        if(!closeAppAfterExecution)
            _userInteraction.GetCloseAppConfirmation();
    }
}
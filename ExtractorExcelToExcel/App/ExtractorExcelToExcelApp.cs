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
        string pathInputExcel;
        string sheetName;
        string columnPositions;
        string columnTexts;
        string columnTextsOverlay;
        bool preliminarySortSheetByColumnPositions;
        string rowRange;
        string? cellIgnoringMark;
        string pathOutputExcel;
        string sheetNameOutput;
        string columnTextsOutput;
        int headerDepth;
        bool closeAppAfterExecution;

        Stopwatch stopwatch = Stopwatch.StartNew();

        (appMode, pathInputExcel, sheetName, columnPositions, columnTexts,
         columnTextsOverlay, preliminarySortSheetByColumnPositions, rowRange, cellIgnoringMark,
         pathOutputExcel, sheetNameOutput, columnTextsOutput, headerDepth, closeAppAfterExecution) =
             _userInteraction.GetParameters();

        Record[] records =
            _repository.ReadRecordsFromRepository(pathInputExcel, appMode, sheetName,
                                                  columnPositions, columnTexts, columnTextsOverlay,
                                                  preliminarySortSheetByColumnPositions, rowRange, cellIgnoringMark);

        _userInteraction.ShowMessage("\nNumber of strings extracted from Excel: " + records.Count());
        _userInteraction.ShowMessage("First ten position-string pairs:");
        _userInteraction.ShowMessage(records[0..Math.Min(10, records.Length)].Select(record => record.ToString()),
                                     ConsoleColor.Cyan);

        _repository.WriteRecordsToRepository(records, pathOutputExcel, sheetNameOutput, columnTextsOutput, headerDepth);

        _userInteraction.ShowMessage('\n' + "The app completed its work.");

        stopwatch.Stop();
        _userInteraction.ShowMessage($"Total time of the application work : {(double)stopwatch.ElapsedMilliseconds / 1000:F3} sec", ConsoleColor.Yellow);

        if(!closeAppAfterExecution)
            _userInteraction.GetCloseAppConfirmation();
    }
}
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
        bool considerStartingIgnoredCellsAsPositionsShift;
        bool closeAppAfterExecution;

        Stopwatch stopwatch = Stopwatch.StartNew();

        (appMode, pathExcelInput, sheetInput, columnPositions, columnTextsInput,
         columnTextsOverlay, preliminarySortSheetByColumnPositions, headerDepthInput, rowRange,
         cellIgnoringMarks,pathExcelOutput, sheetOutput, columnTextsOutput, headerDepthOutput,
         outputOrderMode, considerStartingIgnoredCellsAsPositionsShift, closeAppAfterExecution) =
             _userInteraction.GetParameters();

        (Record[] recordsOrdered, int nmbStartingIgnoredCells) =
            _repository.ReadRecordsFromRepository(pathExcelInput, appMode, sheetInput,
                                                  columnPositions, columnTextsInput, columnTextsOverlay,
                                                  preliminarySortSheetByColumnPositions, headerDepthInput, rowRange, cellIgnoringMarks);

        _userInteraction.ShowMessage("\nNumber of strings extracted from Excel: " + recordsOrdered.Length);
        if(nmbStartingIgnoredCells != 0 && outputOrderMode != OutputOrderMode.outputOrderAccordingToPositions)
            _userInteraction.ShowMessage("Number of lines ignored at start of extraction: " + nmbStartingIgnoredCells);
        _userInteraction.ShowMessage("First ten position-string pairs:");
        _userInteraction.ShowMessage(recordsOrdered[0..Math.Min(10, recordsOrdered.Length)].Select(record => record.ToString()),
                                     ConsoleColor.Cyan);

        _repository.WriteRecordsToRepository(recordsOrdered, pathExcelOutput, sheetOutput, columnTextsOutput,
                                             headerDepthOutput, outputOrderMode, considerStartingIgnoredCellsAsPositionsShift, nmbStartingIgnoredCells);

        _userInteraction.ShowMessage('\n' + "The app completed its work.");

        stopwatch.Stop();
        _userInteraction.ShowMessage($"Total time of the application work : {(double)stopwatch.ElapsedMilliseconds / 1000:F3} sec", ConsoleColor.Yellow);

        if(!closeAppAfterExecution)
            _userInteraction.GetCloseAppConfirmation();
    }
}
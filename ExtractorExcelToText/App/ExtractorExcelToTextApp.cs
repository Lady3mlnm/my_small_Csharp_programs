using ExtractorExcelToText.DataAccess;
using ExtractorExcelToText.DataStructures;
using System.Diagnostics;
using System.Text;

namespace ExtractorExcelToText.App;

public class ExtractorExcelToTextApp
{
    private readonly IRepository _repository;
    private readonly IUserInteraction _userInteraction;
    private readonly ConversionLogic _conversionLogic;

    public ExtractorExcelToTextApp(IRepository repository, IUserInteraction userInteraction, ConversionLogic conversionLogic)
    {
        _repository = repository;
        _userInteraction = userInteraction;
        _conversionLogic = conversionLogic;
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
        WritingMode writingMode;
        string pathTxtOutput;
        bool emptyLineAtEnd;
        Encoding encoding;
        bool closeAppAfterExecution;

        Stopwatch stopwatch = Stopwatch.StartNew();

        (appMode, pathExcelInput, sheetInput, columnPositions, columnTextsInput, columnTextsOverlay,
         preliminarySortSheetByColumnPositions, headerDepthInput, rowRange, cellIgnoringMarks,
         writingMode, pathTxtOutput, emptyLineAtEnd, encoding, closeAppAfterExecution) =
            _userInteraction.GetParameters();

        Record[] recordsFromExcel =
            _repository.ReadRecordsFromRepository(pathExcelInput, appMode, sheetInput,
                                                  columnPositions, columnTextsInput, columnTextsOverlay,
                                                  preliminarySortSheetByColumnPositions, headerDepthInput, rowRange, cellIgnoringMarks);

        _userInteraction.ShowMessage("\nNumber of strings extracted from Excel: " + recordsFromExcel.Length);
        _userInteraction.ShowMessage("First five position-string pairs:");
        _userInteraction.ShowMessage(recordsFromExcel[0..Math.Min(5, recordsFromExcel.Length)].Select(record => record.ToString()),
                                     ConsoleColor.Cyan);

        string[] stringsReady;
        if(writingMode == WritingMode.modeCreateNew) {
            _userInteraction.ShowMessage($"\nMode {writingMode} chosen: create for extracted phrases a new file");
            stringsReady = _conversionLogic.RecordsArrayToStringsArray(recordsFromExcel);
        } else if(writingMode == WritingMode.modeOverlay) {
            _userInteraction.ShowMessage($"\nMode {writingMode} chosen: overlay extracted phrases above contents of the given file");

            string[] stringsFromTxt = _repository.ReadTxt(pathTxtOutput, encoding);

            _userInteraction.ShowMessage($"\nFirst five original strings in the given file:");
            _userInteraction.ShowMessage(stringsFromTxt[0..Math.Min(5, stringsFromTxt.Length)], ConsoleColor.Cyan);

            stringsReady = _conversionLogic.OverlayRecordsToStrings(stringsFromTxt, recordsFromExcel);
        } else
            throw new ArgumentException("Unsupported mode for output of results: " + writingMode);

        _userInteraction.ShowMessage('\n' + "Number of strings to write down to the text file: " + stringsReady.Length);
        _userInteraction.ShowMessage($"\nFirst five strings:");
        _userInteraction.ShowMessage(stringsReady[0..Math.Min(5, stringsReady.Length)], ConsoleColor.Cyan);

        _repository.WriteArrayToRepository(pathTxtOutput, stringsReady, emptyLineAtEnd, encoding);

        _userInteraction.ShowMessage('\n' + "The app completed its work.");

        stopwatch.Stop();
        _userInteraction.ShowMessage($"Total time of the application work : {(double)stopwatch.ElapsedMilliseconds / 1000:F3} sec", ConsoleColor.Yellow);

        if(!closeAppAfterExecution)
            _userInteraction.GetCloseAppConfirmation();
    }
}
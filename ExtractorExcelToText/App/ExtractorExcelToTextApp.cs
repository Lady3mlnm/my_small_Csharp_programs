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
        AppMode appMode = _userInteraction.GetAppMode();
        string pathInputExcel;
        string sheetName;
        string columnPositions;
        string columnTexts;
        string columnTextsOverlay = "";
        string rowRange;
        string? cellIgnoringMark;
        WritingMode writingMode;
        string pathTxt;
        bool addEmptyLineToEnd;
        Encoding encoding;

        bool testMode = false;

        Stopwatch stopwatch = Stopwatch.StartNew();

        if(appMode == AppMode.extractOneColumn) {
            (pathInputExcel, sheetName, columnPositions, columnTexts, rowRange, cellIgnoringMark, writingMode, pathTxt, addEmptyLineToEnd, encoding) =
                _userInteraction.GetParametersForModeExtractOneColumn();
        } else if(appMode == AppMode.combineTwoColumns) {
            (pathInputExcel, sheetName, columnPositions, columnTexts, columnTextsOverlay, rowRange, cellIgnoringMark, writingMode, pathTxt, addEmptyLineToEnd, encoding) =
                _userInteraction.GetParametersForModeCombineTwoColumns();
        } else
            throw new ArgumentException("Unsupported global mode: " + appMode);

        IEnumerable<Record> records = appMode switch {
                AppMode.extractOneColumn => _repository.ReadExcelColumn(pathInputExcel, sheetName, columnPositions, columnTexts, rowRange, cellIgnoringMark),
                AppMode.combineTwoColumns => _repository.ReadExcelTwoColumnsCombined(pathInputExcel, sheetName, columnPositions, columnTexts, columnTextsOverlay, rowRange, cellIgnoringMark),
                _ => throw new ArgumentException("Unsupported mode: " + appMode)
        };

        _userInteraction.ShowMessage("\nNumber of strings extracted from Excel: " + records.Count());
        _userInteraction.ShowMessage("First five pairs position-string:");
        var examplesOfRecords = records.Take(Math.Min(5, records.Count()))
                                       .Select(record => record.ToString());
        _userInteraction.ShowMessage(examplesOfRecords, ConsoleColor.Cyan);


        string[] stringsReady = [];
        if(writingMode == WritingMode.modeCreateNew) {
            _userInteraction.ShowMessage($"\nMode {writingMode} chosen: create for extracted phrases a new file");
            stringsReady = _conversionLogic.RecordsToArrayOfString(records);
        } else if(writingMode == WritingMode.modeOverlay) {
            _userInteraction.ShowMessage($"\nMode {writingMode} chosen: overlay extracted phrases above contents of the given file");

            string[] stringsFromTxt = _repository.ReadTxt(pathTxt, encoding);

            _userInteraction.ShowMessage($"\nFirst five original strings in the given file:");
            _userInteraction.ShowMessage(stringsFromTxt[0..Math.Min(5, stringsFromTxt.Length)], ConsoleColor.Cyan);

            stringsReady = _conversionLogic.OverlayRecordsToArrayOfString(stringsFromTxt, records);
        } else
            throw new ArgumentException("Unsupported mode for output of results: " + writingMode);

        _userInteraction.ShowMessage('\n' + "Number of strings to write down to the file: " + stringsReady.Length);
        _userInteraction.ShowMessage($"\nFirst five strings:");
        _userInteraction.ShowMessage(stringsReady[0..Math.Min(5, stringsReady.Length)], ConsoleColor.Cyan);

        if(testMode)
            _userInteraction.ShowMessage($"The app is in the test mode: no writing the output file.", ConsoleColor.Red);
        else
            _repository.WriteArrayToRepository(pathTxt, stringsReady, addEmptyLineToEnd, encoding);

        _userInteraction.ShowMessage('\n' + "The app completed its work.");

        stopwatch.Stop();
        _userInteraction.ShowMessage($"Total time of the application work : {(double)stopwatch.ElapsedMilliseconds / 1000:F3} sec", ConsoleColor.Yellow);

        _userInteraction.GetCloseAppConfirmation();
    }
}
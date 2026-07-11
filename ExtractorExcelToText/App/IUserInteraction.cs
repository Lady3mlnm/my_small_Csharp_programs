using ExtractorExcelToText.DataAccess;
using System.Text;

namespace ExtractorExcelToText.App;

public interface IUserInteraction
{
    (AppMode appMode, string pathExcelInput, string sheetInput, string columnPositions, string columnTextsInput, string columnTextsOverlay,
        bool preliminarySortSheetByColumnPositions, int headerDepthInput, string rowRangeInput, string[] cellIgnoringMarks,
        WritingMode writingMode, string pathTxtOutput, int headerDepthOutput, OutputOrderMode outputOrderMode,
        bool considerStartingIgnoredCellsAsPositionsShift, bool emptyLineAtEnd, Encoding encoding, bool closeAppAfterExecution)
        GetParameters();
    void ShowMessage(string message, bool isLinebreakAdded = true);
    void ShowMessage(string message, ConsoleColor color, bool isLinebreakAdded = true);
    void ShowMessage(IEnumerable<string> listMessages);
    void ShowMessage(IEnumerable<string> listMessages, ConsoleColor color);
    void GetCloseAppConfirmation();
}
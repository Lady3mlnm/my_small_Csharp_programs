using ExtractorExcelToText.DataAccess;
using System.Text;

namespace ExtractorExcelToText.App;

public interface IUserInteraction
{
    AppMode GetAppMode();
    (string pathInputExcel, string sheetName, string columnPositions, string columnTexts, string rowRange, string? cellIgnoringMark,
        WritingMode writingMode, string pathTxt, bool addEmptyLineToEnd, Encoding encoding)
        GetParametersForModeExtractOneColumn();
    (string pathInputExcel, string sheetName, string columnPositions, string columnTexts, string columnTextsOverlay, string rowRange, string? cellIgnoringMark,
        WritingMode writingMode, string pathTxt, bool addEmptyLineToEnd, Encoding encoding)
        GetParametersForModeCombineTwoColumns();
    void ShowMessage(string message, bool isLinebreakAdded = true);
    void ShowMessage(string message, ConsoleColor color, bool isLinebreakAdded = true);
    void ShowMessage(IEnumerable<string> listMessages);
    void ShowMessage(IEnumerable<string> listMessages, ConsoleColor color);
    void GetCloseAppConfirmation();
}

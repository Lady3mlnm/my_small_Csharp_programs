namespace ExtractorExcelToExcel.App;

public interface IUserInteraction
{
    (AppMode appMode, string pathInputExcel, string sheetName, string columnPositions, string columnTexts,
        string columnTextsOverlay, string rowRange, string? cellIgnoringMark, string pathOutputExcel,
        string sheetNameOutput, string columnTextsOutput, int headerDepth)
        GetParameters();
    void ShowMessage(string message, bool isLinebreakAdded = true);
    void ShowMessage(string message, ConsoleColor color, bool isLinebreakAdded = true);
    void ShowMessage(IEnumerable<string> listMessages);
    void ShowMessage(IEnumerable<string> listMessages, ConsoleColor color);
    void GetCloseAppConfirmation();
}

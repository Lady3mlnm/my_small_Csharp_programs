using System.Text;

namespace ExtractorTextToExcel.App;

public interface IUserInteraction
{
    (string pathTxtInput, string stringRange, Encoding encoding, string[] stringIgnoringMarks,
        string pathExcelOutput, string sheetOutput, string columnTextsOutput, int headerDepthOutput, bool closeAppAfterExecution)
        GetParameters();
    void ShowMessage(string message, bool isLinebreakAdded = true);
    void ShowMessage(string message, ConsoleColor color, bool isLinebreakAdded = true);
    void ShowMessage(IEnumerable<string> listMessages);
    void ShowMessage(IEnumerable<string> listMessages, ConsoleColor color);
    void GetCloseAppConfirmation();
}
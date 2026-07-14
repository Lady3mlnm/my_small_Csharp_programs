namespace SearchFormulasInExcel.App;

public interface IUserInteraction
{
    (string pathExcel, string sheet, int headerDepth, string[] columns, bool closeAppIfFormulasNotFound)
        GetParameters();
    void ShowMessage(string message, bool isLinebreakAdded = true);
    void ShowMessage(string message, ConsoleColor color, bool isLinebreakAdded = true);
    void ShowMessage(IEnumerable<string> listMessages);
    void ShowMessage(IEnumerable<string> listMessages, ConsoleColor color);
    void GetCloseAppConfirmation();
}
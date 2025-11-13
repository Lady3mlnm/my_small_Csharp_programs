using SearchDuplicateStrings.DataAccess;

namespace SearchDuplicateStrings.App
{
    public interface IUserInteraction
    {
        (string pathFileInput, string pathFileOutput, FileOutputFormat fileOutputFormat) GetParameters();
        void ShowMessage(string message, bool isLinebreakAdded = true);
        void ShowMessage(string message, ConsoleColor color, bool isLinebreakAdded = true);
        void ShowMessage(IEnumerable<string> listMessages);
        void ShowMessage(IEnumerable<string> listMessages, ConsoleColor color);
        void GetCloseAppConfirmation();
    }
}
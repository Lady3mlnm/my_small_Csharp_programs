using BatchSubstitution.DataAccess;

namespace BatchSubstitution.App
{
    public interface IUserInteraction
    {
        (string pathFileInput, string pathFileDictionary, string separatorsKeyValue, bool isOnlyWords,
            DictionaryDirectionFormat directionInDictionary, string pathFileOutput) GetParameters();
        void ShowMessage(string message, bool isLinebreakAdded = true);
        void ShowMessage(string message, ConsoleColor color, bool isLinebreakAdded = true);
        void ShowMessage(IEnumerable<string> listMessages);
        void ShowMessage(IEnumerable<string> listMessages, ConsoleColor color);
        void GetCloseAppConfirmation();
    }
}


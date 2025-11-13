namespace TextToHexConverter.App
{
    public interface IUserInteraction
    {
        //(string pathFileInput, string pathFileOutput, string separatorsHexValues, int nmbInsertedLinebreaks) GetInitialParameters(string[] args);
        (string pathFileInput, string pathFileOutput, string separatorsHexValues, int nmbInsertedLinebreaks, bool isODOAExcludedFromHex) GetParameters();
        void ShowMessage(string message, bool isLinebreakAdded = true);
        void ShowMessage(string message, ConsoleColor color, bool isLinebreakAdded = true);
        void GetCloseAppConfirmation();
    }
}
using ExtractorExcelToText.App;
using ExtractorExcelToText.DataAccess;

internal class Program
{
    private static void Main(string[] args)
    {
            const string APPTITLE = "ExtractorExcelToText v0.1.1";
            IRepository repository = new DiskRepository();
            IUserInteraction userInteraction = new ConsoleUserInteraction(args, APPTITLE);
            ConversionLogic conversionLogic = new ConversionLogic();
        try {
            var extractorExcelToTextApp = new ExtractorExcelToTextApp(repository, userInteraction, conversionLogic);

            extractorExcelToTextApp.Run();
        } catch(Exception ex) {
            userInteraction.ShowMessage("\nThe application has experienced an unexpected error.\n" +
                                        "The error message: " + ex.Message, ConsoleColor.Red);
            userInteraction.GetCloseAppConfirmation();
        }
    }
}
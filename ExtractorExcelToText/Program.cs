using ExtractorExcelToText.App;
using ExtractorExcelToText.DataAccess;

internal class Program
{
    private static void Main(string[] args)
    {
        try {
            const string APPTITLE = "ExtractorExcelToText v0.2.2";
            IRepository repository = new DiskRepository();
            IUserInteraction userInteraction = new ConsoleUserInteraction(args, APPTITLE);
            ConversionLogic conversionLogic = new ConversionLogic();
            var extractorExcelToTextApp = new ExtractorExcelToTextApp(repository, userInteraction, conversionLogic);

            extractorExcelToTextApp.Run();
        } catch(Exception ex) {
            Console.WriteLine("\nThe application has experienced an unexpected error.\n" +
                                        "The error message: " + ex.Message);
            Console.ReadKey();
        }
    }
}
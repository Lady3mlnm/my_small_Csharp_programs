using ExtractorExcelToText.App;
using ExtractorExcelToText.DataAccess;

internal class Program
{
    private static void Main(string[] args)
    {
        try {
            const string APPTITLE = "ExtractorExcelToText v1.0.1";
            IRepository repository = new DiskRepository();
            IUserInteraction userInteraction = new ConsoleUserInteraction(args, APPTITLE);
            ConversionLogic conversionLogic = new ConversionLogic();
            var extractorExcelToTextApp = new ExtractorExcelToTextApp(repository, userInteraction, conversionLogic);

            extractorExcelToTextApp.Run();
        } catch(Exception ex) {
            Console.Write("\nThe application has experienced an unexpected error.\n" +
                          "The error message: " + ex.Message);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
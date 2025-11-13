using TextToHexConverter.App;
using TextToHexConverter.DataAccess;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            const string APPTITLE = "TextToHexConverter v0.5.1";
            IRepository repository = new DiskRepository();
            IUserInteraction userInteraction = new ConsoleUserInteraction(args, APPTITLE);
            ConversionLogic conversionLogic = new ConversionLogic();

            var textToHexConverterApp = new TextToHexConverterApp(repository, userInteraction, conversionLogic);
            textToHexConverterApp.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine("The application has experienced an unexpected error.\n" +
                              "The error message: " + ex.Message);
            Console.ReadKey();
        }
    }
}
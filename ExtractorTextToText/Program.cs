using ExtractorTextToExcel.App;
using ExtractorTextToExcel.DataAccess;

internal class Program
{
    private static void Main(string[] args)
    {
        try {
            const string APPTITLE = "ExtractorTextToExcel v1.1";
            IRepository repository = new DiskRepository();
            IUserInteraction userInteraction = new ConsoleUserInteraction(args, APPTITLE);
            var extractorExcelToExcelApp = new ExtractorTextToExcelApp(repository, userInteraction);

            extractorExcelToExcelApp.Run();
        } catch(Exception ex) {
            Console.Write("\nThe application has experienced an unexpected error.\n" +
                          "The error message: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
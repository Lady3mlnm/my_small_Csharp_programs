using SearchFormulasInExcel.App;
using SearchFormulasInExcel.DataAccess;

internal class Program
{
    private static void Main(string[] args)
    {
        try {
            const string APPTITLE = "SearchFormulasInExcel v1.0";
            IRepository repository = new DiskRepository();
            IUserInteraction userInteraction = new ConsoleUserInteraction(args, APPTITLE);
            var extractorExcelToExcelApp = new SearchFormulasInExcelApp(repository, userInteraction);

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
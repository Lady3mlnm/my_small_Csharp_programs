using BatchSubstitution.App;
using BatchSubstitution.DataAccess;

internal class Program
{
    private static void Main(string[] args)
    {
        try {
            const string APPTITLE = "BatchSubstitution v0.1.1";
            IRepository repository = new DiskRepository();
            IUserInteraction userInteraction = new ConsoleUserInteraction(args, APPTITLE);
            ConversionLogic conversionLogic = new ConversionLogic();

            var batchSubstitutionApp = new BatchSubstitutionApp(repository, userInteraction, conversionLogic);
            batchSubstitutionApp.Run();
        } catch(Exception ex) {
            Console.WriteLine("\nThe application has experienced an unexpected error.\n" +
                              "The error message: " + ex.Message);
            Console.ReadKey();
        }
    }
}

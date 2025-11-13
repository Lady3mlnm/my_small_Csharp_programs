using SearchDuplicateStrings.App;
using SearchDuplicateStrings.DataAccess;

namespace SearchDuplicateStrings
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                const string APPTITLE = "SearchDuplicateStrings v0.2.3";
                IRepository repository = new DiskRepository();
                IUserInteraction userInteraction = new ConsoleUserInteraction(args, APPTITLE);
                TextProcessingLogic textProcessingLogic = new TextProcessingLogic();

                var textProcessingApp = new TextProcessingApp(repository, userInteraction, textProcessingLogic);
                textProcessingApp.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("The application has experienced an unexpected error.\n" +
                                  "The error message: " + ex.Message);
                Console.ReadKey();
            }
        }
    }
}
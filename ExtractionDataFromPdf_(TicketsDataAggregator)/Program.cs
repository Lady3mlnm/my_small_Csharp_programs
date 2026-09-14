using TicketsDataAggregator.App;
using TicketsDataAggregator.DataAccess;

string targetFolder = "Tickets";
string outputFile = Path.Combine(targetFolder, "aggregatedTickets.txt");

try {
    var ticketsReader = new TicketsFromPdfsReader();
    var resultWriter = new ResultToDiskWriter();
    var ticketsAnalyzer = new TicketsAnalyzer();
    var notesOutput = new NotesToConsoleOutput();

    new TicketsDataAggregatorApp(
        ticketsReader,
        resultWriter,
        ticketsAnalyzer,
        notesOutput,
        targetFolder,
        outputFile).Run();
} catch(Exception ex) {
    Console.WriteLine("An error occurred. " +
                      "Exception message: " + ex.Message);
}

Console.WriteLine("Press any key to close.");
Console.ReadKey();
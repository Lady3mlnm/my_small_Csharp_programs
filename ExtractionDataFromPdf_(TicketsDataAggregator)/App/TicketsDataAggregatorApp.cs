using System.Globalization;
using TicketsDataAggregator.DataAccess;
using TicketsDataAggregator.DataStructures;

namespace TicketsDataAggregator.App;

public class TicketsDataAggregatorApp
{
    private readonly ITicketsReader _ticketsReader;
    private readonly IResultWriter _resultWriter;
    private readonly ITicketsAnalyzer _ticketsAnalyzer;
    private readonly INotesOutput _notesOutput;
    private readonly string _targetFolder;
    private readonly string _outputFile;

    public TicketsDataAggregatorApp(
        ITicketsReader ticketsReader,
        IResultWriter resultWriter,
        ITicketsAnalyzer ticketsAnalyzer,
        INotesOutput notesOutput,
        string targetFolder,
        string outputFile)
    {
        _ticketsReader = ticketsReader;
        _resultWriter = resultWriter;
        _ticketsAnalyzer = ticketsAnalyzer;
        _notesOutput = notesOutput;
        _targetFolder = targetFolder;
        _outputFile = outputFile;
    }

    public void Run()
    {
        string[] ticketDocuments = _ticketsReader.GetDocumentsInStorage(_targetFolder);

        bool startAggregation = true;
        foreach (string ticketDocument in ticketDocuments) {
            _notesOutput.ShowMessage("processed document:" + ticketDocument);

            string textDocument = _ticketsReader.ReadDocument(ticketDocument);

            CultureInfo culture = _ticketsAnalyzer.ExtractCultureFromText(
                textDocument,
                sitePattern: @"(?<=Visit us:)[\w\.]*$");
            _notesOutput.ShowMessage("culture: " + culture);

            Ticket[] tickets = _ticketsAnalyzer.ExtractTicketsFromText(
                textDocument,
                ticketPattern: @"Title:(?<title>.*?)Date:(?<date>.*?)Time:(?<time>.*?)(?=Title:|Visit us:)",
                culture);
            _notesOutput.ShowMessage("number of tickets: " + tickets.Length);
            _notesOutput.ShowTickets(tickets, isInvariantCulture: true);

            _resultWriter.WriteTickets(tickets, _outputFile, createNewFile: startAggregation);
            Console.WriteLine($"Tickets are {(startAggregation ? "written" : "appended")} to file " + _outputFile);
            if (startAggregation)
                startAggregation = false;

            _notesOutput.ShowMessage("");
        }
    }
}
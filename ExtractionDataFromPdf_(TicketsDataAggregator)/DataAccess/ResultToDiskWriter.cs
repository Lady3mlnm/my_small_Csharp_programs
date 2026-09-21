using TicketsDataAggregator.DataStructures;

namespace TicketsDataAggregator.DataAccess;

public class ResultToDiskWriter : IResultWriter
{
    public void WriteTickets(IEnumerable<Ticket> tickets, string filePath, bool createNewFile = true)
    {
        IEnumerable<string> linesToWrite =
            tickets.Select(ticket => FormattableString.Invariant(
                $"{ticket.Title,-40} | {ticket.Date} | {ticket.Time}"));

        if (createNewFile)                                     // more descriptive approach
            File.WriteAllLines(filePath, linesToWrite);
        else
            File.AppendAllLines(filePath, linesToWrite);
    }
}



//// Alternative, more sophisticated approach to write lines into a file
//Action<string, IEnumerable<string>> saveLinesToFile =
//    createNewFile ? File.WriteAllLines : File.AppendAllLines;

//saveLinesToFile(filePath, linesToWrite);
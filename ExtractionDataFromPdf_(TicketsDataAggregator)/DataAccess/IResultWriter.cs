using TicketsDataAggregator.DataStructures;

namespace TicketsDataAggregator.DataAccess;

public interface IResultWriter
{
    void WriteTickets(IEnumerable<Ticket> tickets, string filePath, bool newFile=true);
}
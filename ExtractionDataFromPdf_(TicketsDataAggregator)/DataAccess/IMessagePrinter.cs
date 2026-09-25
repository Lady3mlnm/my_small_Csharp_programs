using TicketsDataAggregator.DataStructures;

namespace TicketsDataAggregator.DataAccess;

public interface IMessagePrinter
{
    void ShowMessage(string message);
    void ShowTickets(IEnumerable<Ticket> tickets, bool isInvariantCulture=false);
    void ShowTicket(Ticket ticket, bool isInvariantCulture=false);
}
using TicketsDataAggregator.DataStructures;

namespace TicketsDataAggregator.DataAccess;

public class NotesToConsoleOutput : INotesOutput
{
    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void ShowTickets(IEnumerable<Ticket> tickets, bool isInvariantCulture=false)
    {
        foreach (Ticket ticket in tickets)
            ShowTicket(ticket, isInvariantCulture);
    }

    public void ShowTicket(Ticket ticket, bool isInvariantCulture=false)
    {
        if(isInvariantCulture)
            Console.WriteLine(ticket.ToStringInvariant());
        else
            Console.WriteLine(ticket);
    }
}
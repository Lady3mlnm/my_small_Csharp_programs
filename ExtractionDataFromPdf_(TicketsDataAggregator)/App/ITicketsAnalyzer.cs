using System.Globalization;
using TicketsDataAggregator.DataStructures;

namespace TicketsDataAggregator.App;

public interface ITicketsAnalyzer
{
    CultureInfo ExtractCultureFromText(string textDocument, string sitePattern);

    Ticket[] ExtractTicketsFromText(string textDocument, string ticketPattern, CultureInfo culture);
}
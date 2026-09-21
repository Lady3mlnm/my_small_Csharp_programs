using TicketsDataAggregator.DataStructures;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TicketsDataAggregator.App;

public class TicketsAnalyzer : ITicketsAnalyzer
{
    private Dictionary<string, CultureInfo> _domainToCultureMapping = new() {
        ["com"] = new CultureInfo("en-US"),
        ["fr"]  = new CultureInfo("fr-FR"),
        ["jp"]  = new CultureInfo("jp-JP")
    };

    public CultureInfo ExtractCultureFromText(string textDocument, string sitePattern)
    {
        Match matchSite = Regex.Match(textDocument, sitePattern);
        string siteAddress = matchSite.Value;

        Match matchExtension = Regex.Match(siteAddress, @"(?<=\.)[a-z]{2,63}$");
        string siteExtension = matchExtension.Value;

        CultureInfo culture = _domainToCultureMapping[siteExtension];
        return culture;
    }

    public Ticket[] ExtractTicketsFromText(string text, string ticketPattern, CultureInfo culture)
    {
        MatchCollection allMatches = Regex.Matches(text, ticketPattern);

        Ticket[] arTickets = new Ticket[allMatches.Count];
        int count = 0;
        foreach (Match matchTicket in allMatches) {
            if (!matchTicket.Success)
                throw new ArgumentException("A ticket string can't be parsed correctly: " + matchTicket.Value);

            string title = matchTicket.Groups["title"].Value;
            string dateString = matchTicket.Groups["date"].Value;
            string timeString = matchTicket.Groups["time"].Value;

            DateOnly dateParsed = DateOnly.Parse(dateString, culture);
            TimeOnly timeParsed = TimeOnly.Parse(timeString, culture);

            arTickets[count] = new Ticket(title, dateParsed, timeParsed);
            count++;
        }

        return arTickets;
    }
}



/* // overly sophisticated alternative
    return Regex.Matches(text, ticketPattern).Select(matchTicket => retrieveTicketFromMatch(matchTicket, usedCulture))
                                                 .ToArray();
    private Ticket retrieveTicketFromMatch(Match matchTicket, CultureInfo usedCulture)
    {
        if (!matchTicket.Success)
            throw new ArgumentException("A ticket string can't be parsed correctly: " + matchTicket.Value);

        string title = matchTicket.Groups["title"].Value;
        string dateString = matchTicket.Groups["date"].Value;
        string timeString = matchTicket.Groups["time"].Value;

        DateOnly dateParsed = DateOnly.Parse(dateString, usedCulture); 
        TimeOnly timeParsed = TimeOnly.Parse(timeString, usedCulture);

        return new Ticket(title, dateParsed, timeParsed);
    }
*/
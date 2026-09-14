namespace TicketsDataAggregator.DataStructures;

public readonly struct Ticket
{
    public string Title { get; init; }
    public DateOnly Date { get; init; }
    public TimeOnly Time { get; init; }

    public Ticket(string title, DateOnly date, TimeOnly time)
    {
        Title = title;
        Date = date;
        Time = time;
    }

    public override string ToString() =>
        $"Title: {Title}, Date: {Date}, Time: {Time}";

    public string ToStringInvariant() =>
        FormattableString.Invariant($"Title: {Title}, Date: {Date}, Time: {Time}");
}
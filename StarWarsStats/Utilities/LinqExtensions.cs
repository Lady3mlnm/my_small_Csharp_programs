namespace StarWarsStats.Utilities;

public static class LinqExtensions
{
    public static double? Median(this IEnumerable<long?> inputSequence)
    {
        //if(inputSequence == null || !inputSequence.Any())
        //    throw new InvalidOperationException("Cannot compute median for empty sequence.");

        var arSorted = inputSequence.OrderBy(n => n).ToArray();
        int count = arSorted.Length;
        int middle = count / 2;

        return count % 2 == 0
            ? (arSorted[middle - 1] + arSorted[middle]) / 2
            : arSorted[middle];
    }
}
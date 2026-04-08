using System.Globalization;

namespace StarWarsStats.Utilities;

public static class StringExtensions
{
    public static int? ToIntOrNull(this string? input)
    {
        if(int.TryParse(input,
                        NumberStyles.AllowThousands, CultureInfo.InvariantCulture,
                        out int intResultParsed))
            return intResultParsed;

        return float.TryParse(input, out float floatResultParsed)
            ? (int)Math.Round(floatResultParsed, MidpointRounding.AwayFromZero)
            : null;
    }

    public static long? ToLongOrNull(this string? input)
    {
        return long.TryParse(input,
                             NumberStyles.AllowThousands, CultureInfo.InvariantCulture,
                             out long resultParsed)
            ? resultParsed
            : null;
    }

    public static int? ToStarWarsYearIntOrNull(this string? input)
    {
        if(input is null)
            return null;

        if(input.EndsWith("BBY"))
            input = "-" + input[..^3];  // Remove the last 3 characters (the unit)
        else if(input.EndsWith("ABY"))
            input = input[..^3];

        if(int.TryParse(input,
                        NumberStyles.AllowThousands, CultureInfo.InvariantCulture,
                        out int intResultParsed))
            return intResultParsed;

        return float.TryParse(input, out float floatResultParsed)
            ? (int)Math.Round(floatResultParsed, MidpointRounding.AwayFromZero)
            : null;
    }
}
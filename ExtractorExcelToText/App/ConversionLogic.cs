using ExtractorExcelToText.DataStructures;

namespace ExtractorExcelToText.App;

public class ConversionLogic
{
    public string[] RecordsArrayToStringsArray(Record[] recordsOrdered)
    {
        string[] stringsReady = new string[recordsOrdered[^1].Position];
        foreach(Record record in recordsOrdered)
            stringsReady[record.Position - 1] = record.Text;

        return stringsReady;
    }


    public string[] OverlayRecordsToStrings(string[] strings, Record[] recordsOrdered)
    {
        int numberOfStrings = Math.Max(strings.Length,
                                       recordsOrdered.Last().Position);

        if(numberOfStrings > strings.Length)
            Array.Resize(ref strings, numberOfStrings);

        foreach(Record record in recordsOrdered)
            strings[record.Position - 1] = record.Text;

        return strings;
    }
}

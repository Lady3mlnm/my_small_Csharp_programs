using ExtractorExcelToText.DataStructures;

namespace ExtractorExcelToText.App;

public class ConversionLogic
{
    public string[] RecordsToArrayOfString(IOrderedEnumerable<Record> recordsOrdered)
    {
        int numberOfStrings = recordsOrdered.Last().Position;

        string[] stringsReady = new string[numberOfStrings];
        foreach (Record record in recordsOrdered)
            stringsReady[record.Position - 1] = record.Text;

        return stringsReady;
    }


    public string[] OverlayRecordsToArrayOfString(string[] initialTexts, IOrderedEnumerable<Record> recordsOrdered)
    {
        int numberOfStrings = Math.Max(initialTexts.Length,
                                       recordsOrdered.Last().Position);

        if(numberOfStrings > initialTexts.Length)
            Array.Resize(ref initialTexts, numberOfStrings);

        foreach(Record record in recordsOrdered)
            initialTexts[record.Position - 1] = record.Text;

        return initialTexts;
    }
}

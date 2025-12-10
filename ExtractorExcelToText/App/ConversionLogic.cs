using ExtractorExcelToText.DataStructures;

namespace ExtractorExcelToText.App;

public class ConversionLogic
{
    public string[] RecordsToArrayOfString(IEnumerable<Record> records)
    {
        int numberOfStrings = records.Select(record => record.Position).Max();

        string[] stringsReady = new string[numberOfStrings];
        foreach (Record record in records)
            stringsReady[record.Position - 1] = record.Text;

        return stringsReady;
    }


    public string[] OverlayRecordsToArrayOfString(string[] initialTexts, IEnumerable<Record> records)
    {
        int numberOfStrings = Math.Max(initialTexts.Length,
                                       records.Select(record => record.Position).Max());

        if(numberOfStrings > initialTexts.Length)
            Array.Resize(ref initialTexts, numberOfStrings);

        foreach(Record record in records)
            initialTexts[record.Position - 1] = record.Text;

        return initialTexts;
    }
}

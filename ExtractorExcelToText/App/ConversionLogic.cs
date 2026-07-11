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

    public Record[] ShiftPositionsInRecords(
        Record[] recordsOrdered, int headerDepth, OutputOrderMode orderMode, bool useAdditionalIndent, int AdditionalIndentWhenShifting)
    {
        switch(orderMode) {
            case OutputOrderMode.outputOrderAccordingToPositions:
                return recordsOrdered.Select(record => new Record(record.Position + headerDepth, record.Text))
                                     .ToArray();
            case OutputOrderMode.outputOrderShiftToHeader:
                int shift = (useAdditionalIndent)
                    ? recordsOrdered.First().Position - headerDepth - AdditionalIndentWhenShifting - 1
                    : recordsOrdered.First().Position - headerDepth - 1;
                return recordsOrdered.Select(record => new Record(record.Position - shift, record.Text))
                                     .ToArray();
            case OutputOrderMode.outputOrderCompressed:
                int startingPoint = (useAdditionalIndent)
                    ? headerDepth + AdditionalIndentWhenShifting + 1
                    : headerDepth + 1;
                return recordsOrdered.Select((record, index) => new Record(startingPoint + index, record.Text))
                                     .ToArray();
            default:
                throw new ArgumentException("Ouput of text lines using unsupported mode: " + orderMode);
        }
    }
}

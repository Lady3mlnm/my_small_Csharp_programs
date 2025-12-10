namespace ExtractorExcelToText.DataStructures;

public struct Record
{
    public int Position;
    public string Text;

    public Record(int position, string text)
    {
        Position = position;
        Text = text;
    }

    public override string ToString() =>
        $"{Position}. {Text}";
}

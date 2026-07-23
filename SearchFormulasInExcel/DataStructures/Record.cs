namespace SearchFormulasInExcel.DataStructures;

public struct Record(int position, string? text)
{
    public int Position = position;
    public string? Text = text;

    public override string ToString() =>
        $"{Position}. {Text}";
}
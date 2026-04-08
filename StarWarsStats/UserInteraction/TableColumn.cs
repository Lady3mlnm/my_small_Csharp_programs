namespace StarWarsStats.UserInteraction;

public struct TableColumn
{
    public string NameForHuman { get; }
    public string ColumnTitle { get; }
    public int ColumnWidth { get; }
    public string NameOfProperty { get; }
    public TableColumn(string name, string columnTitle, int columnsWidth, string nameOfProperty)
    {
        NameForHuman = name;
        ColumnTitle = columnTitle;
        ColumnWidth = columnsWidth;
        NameOfProperty = nameOfProperty;
    }
}
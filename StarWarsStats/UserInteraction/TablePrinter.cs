using System.Reflection;
using StarWarsStats.Model;

namespace StarWarsStats.UserInteraction;

public static class TablePrinter
{
    public static void Print<T>(IEnumerable<T> items, TableColumn[] columnsOfTable, string? columnSorting = null, string? additionalInfo = null) where T : IModel
    {
        Console.WriteLine();
        Console.WriteLine("Number of records to the table: " +
                               items.Count());
        Console.WriteLine("Sorted by column: " +
                               ((columnSorting is not null) ? columnSorting : "—"));
        if(additionalInfo is not null)
            Console.WriteLine(additionalInfo);

        // Printing the header
        string headerOfTable =
            string.Join("|",
                        columnsOfTable.Select(
                            column => String.Format($"{{0, -{column.ColumnWidth}}}", column.ColumnTitle)[..column.ColumnWidth]));

        Console.WriteLine(headerOfTable);
        Console.WriteLine(
            new string('—', headerOfTable.Length));

        string[] namesOfPropertySelected = columnsOfTable
            .Select(column => column.NameOfProperty)
            .ToArray();

        int[] widthsOfColumns = columnsOfTable
            .Select(column => column.ColumnWidth)
            .ToArray();

        PropertyInfo[] properties = typeof(T)
            .GetProperties()
            .Where(property => namesOfPropertySelected.Contains(property.Name))
            .OrderBy(property => Array.IndexOf(namesOfPropertySelected, property.Name))
            .ToArray();

        // Sorting of the sequence
        PropertyInfo? propertiesSorting = properties.FirstOrDefault(property => property.Name == columnSorting);
        var itemsSorted = (propertiesSorting is not null)
            ? (propertiesSorting.PropertyType == typeof(string))
                ? items.OrderBy(item => propertiesSorting.GetValue(item))
                : items.OrderByDescending(item => propertiesSorting.GetValue(item))
            : items;

        // Printing the sequence
        foreach(var item in itemsSorted) {
            string[] partsOfRecord = properties
                .Zip(widthsOfColumns, (property, width) =>
                   property.PropertyType switch {
                       Type t when t == typeof(int?) =>
                            (property.Name == "BirthYear")
                                ? property.GetValue(item) switch {
                                    null => new string(' ', width),
                                    < 0  => String.Format($"{{0, {width - 3}}}BBY", -(int)property.GetValue(item)!),
                                    > 0  => String.Format($"{{0, {width - 3}}}ABY", property.GetValue(item)),
                                    _    => String.Format($"{{0, {width - 3}}}   ", property.GetValue(item))
                                  }
                                : String.Format($"{{0, {width}:N0}}", property.GetValue(item)),
                       Type t when t == typeof(long?)  => String.Format($"{{0, {width}:N0}}", property.GetValue(item)),
                       Type t when t == typeof(string) => String.Format($"{{0, -{width}}}", property.GetValue(item))[..width],
                       _ => throw new Exception($"TablePrinter class: Unsupported property type: {property.PropertyType}")
                   })
                .ToArray();
            Console.WriteLine(string.Join("|", partsOfRecord));
        }
    }
}
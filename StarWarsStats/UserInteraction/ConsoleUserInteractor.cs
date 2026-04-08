using StarWarsStats.Model;

namespace StarWarsStats.UserInteraction;

public class ConsoleUserInteractor : IUserInteractor
{
    public string? ReadFromUser(string priliminaryMessage = ">")
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(priliminaryMessage);
        string? userResponse = Console.ReadLine();
        Console.ResetColor();
        return userResponse;
    }

    public void ShowMessage(string message = "")
    {
        Console.WriteLine(message);
    }

    public void ShowMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void PrintTable<T>(IEnumerable<T> items, TableColumn[] columnsOfTable, string? columnSorting, string? additionalInfo = null) where T : IModel
    {
        TablePrinter.Print<T>(items, columnsOfTable, columnSorting, additionalInfo);
    }
}
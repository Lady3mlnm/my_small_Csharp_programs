using StarWarsStats.Model;
namespace StarWarsStats.UserInteraction;

public interface IUserInteractor
{
    string? ReadFromUser(string priliminaryMessage = ">");
    void ShowMessage(string message = "");
    void ShowMessage(string message, ConsoleColor color);
    void PrintTable<T>(IEnumerable<T> items, TableColumn[] columnsOfTable, string? columnSorting, string? additionalInfo = null) where T : IModel;
}
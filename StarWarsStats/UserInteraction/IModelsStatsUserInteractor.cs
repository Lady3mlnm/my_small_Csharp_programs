using StarWarsStats.Model;

namespace StarWarsStats.UserInteraction;

public interface IModelsStatsUserInteractor
{
    string? ReadFromUser(string priliminaryMessage = ">");
    ModelType GetColumnSorting();
    int GetYearForCalculatingAge();
    string? GetColumnSorting(TableColumn[] columnsOfTable);
    void ShowModels<T>(IEnumerable<T> models, TableColumn[] columnsOfTable, string? columnSorting, string? additionalInfo = null) where T : IModel;
    string? ChooseStatisticsToBeShown(IEnumerable<string> propertiesThatCanBeChosen);
    void ShowMessage(string message = "");
    void ShowMessage(string message, ConsoleColor color);
}
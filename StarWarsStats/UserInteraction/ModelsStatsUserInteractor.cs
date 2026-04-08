using StarWarsStats.Model;

namespace StarWarsStats.UserInteraction;

public class ModelsStatsUserInteractor : IModelsStatsUserInteractor
{
    private readonly IUserInteractor _userInteractor;

    public ModelsStatsUserInteractor(
        IUserInteractor userInteractor)
    {
        _userInteractor = userInteractor;
    }


    public string? ReadFromUser(string priliminaryMessage = ">")
    {
        return _userInteractor.ReadFromUser(priliminaryMessage);
    }


    public void ShowMessage(string message = "")
    {
        _userInteractor.ShowMessage(message);
    }

    public void ShowMessage(string message, ConsoleColor color)
    {
        _userInteractor.ShowMessage(message, color);
    }

    public ModelType GetColumnSorting()
    {
        string[] namesOfModels = Enum.GetNames(typeof(ModelType));
        do {
            _userInteractor.ShowMessage(
                Environment.NewLine + "Which type of models do you like to explore?" +
                Environment.NewLine + "    " + string.Join(" / ", namesOfModels));
            string? userResponse = ReadFromUser();

            if (Enum.TryParse<ModelType>(userResponse, true, out ModelType selectedModel))
                return selectedModel;
            else
                _userInteractor.ShowMessage("Incorrect input");
        } while(true);
    }


    public int GetYearForCalculatingAge()
    {
        _userInteractor.ShowMessage(Environment.NewLine + "Enter year for calculating age of characters, as integer. Use a negative number for BBY." +
                                    Environment.NewLine + "(years of actions in the classical films: -32, -22, -19  /  0, 3, 4  /  34, 34, 35)");
        string? userResponse = ReadFromUser();

        if(int.TryParse(userResponse, out int inputtedYear))
            return inputtedYear;
        else {
            _userInteractor.ShowMessage("The input can't be casting to integer. The default value '0 BBY/ABY' will be used.");
            return 0;
        }
    }


    public void ShowModels<T>(IEnumerable<T> models, TableColumn[] columnsOfTable, string? columnSorting, string? additionalInfo = null) where T : IModel
    {
        _userInteractor.PrintTable<T>(models, columnsOfTable, columnSorting, additionalInfo);
    }


    public string? GetColumnSorting(TableColumn[] columnsOfTable)
    {
        var arChoices = columnsOfTable
            .Select(record => record.NameForHuman)
            .ToArray();

        _userInteractor.ShowMessage(
            Environment.NewLine + "By which characteristic do you want to see the table sorted?" +
            Environment.NewLine + "    " + string.Join(" / ", arChoices));

        string? userChoice = ReadFromUser();
        if(userChoice is not null &&
            arChoices.Select(name => name.ToLower()).Contains(userChoice.ToLower())) {
            return columnsOfTable
                    .Where(record => record.NameForHuman.ToLower() == userChoice.ToLower())
                    .First()
                    .NameOfProperty;
        } else
            return null;
    }


    public string? ChooseStatisticsToBeShown(IEnumerable<string> propertiesThatCanBeChosen)
    {
        _userInteractor.ShowMessage(
            Environment.NewLine + "The statistics of which quantitative property would you like to see?  ('exit' or 'e' for exit)" +
            Environment.NewLine + "    " + string.Join(" / ", propertiesThatCanBeChosen));

        return _userInteractor.ReadFromUser();
    }
}
using StarWarsStats.Model;
using StarWarsStats.UserInteraction;
using StarWarsStats.Utilities;
using System.Reflection;

namespace StarWarsStats.App;

public class StatisticsAnalyzer : IStatisticsAnalyzer
{
    private readonly IModelsStatsUserInteractor _modelsStatsUserInteractor;

    public StatisticsAnalyzer(
        IModelsStatsUserInteractor modelsStatsUserInteractor)
    {
        _modelsStatsUserInteractor = modelsStatsUserInteractor;
    }


    public void DetermineAgeAtYearForPeople(IEnumerable<Person> people, string? yearForCalculatingAge)
    {
        int? yearForCalculatingAgeAsInt = yearForCalculatingAge.ToStarWarsYearIntOrNull();
        if(yearForCalculatingAgeAsInt is not null)
            DetermineAgeAtYearForPeople(people, yearForCalculatingAgeAsInt.Value);
    }


    public void DetermineAgeAtYearForPeople(IEnumerable<Person> people, int yearForCalculatingAge)
    {
        foreach(var person in people.ToList()) {
            person.DetermineAgeAtYear(yearForCalculatingAge);
        }
    }


    public void DetermineSpeciesOfPeople(IEnumerable<Person> people, IEnumerable<Species> species)
    {
        var idToSpicies = species
            .Select(record => (record.Id, record.Name, record.Lifespan))
            .ToDictionary(record => record.Id, record => (Species: record.Name, Lifespan: record.Lifespan));


        foreach(var person in people.ToList()) {
            string? speciesId = person.Species;
            if(speciesId is not null && speciesId.StartsWith("id_"))
                    speciesId = speciesId[3..];
            if(int.TryParse(speciesId, out int speciesAsInt) && idToSpicies.ContainsKey(speciesAsInt)) {
                person.SetSpecies(idToSpicies[speciesAsInt].Species);
                int? parsedLifespan = idToSpicies[speciesAsInt].Lifespan;
                if(parsedLifespan is not null)
                    person.SetLifespan(parsedLifespan);
            }
        }

        _modelsStatsUserInteractor.ShowMessage("Data of people were complemented based on data of species", ConsoleColor.DarkYellow);
    }


    public void Analyze<T>(IEnumerable<T> models, TableColumn[] columnsOfTable) where T : class, IModel
    {
        string[] namesOfPropertySelected = columnsOfTable
            .Select(column => column.NameOfProperty)
            .ToArray();

        string[] namesPropertiesForAnalysis = typeof(T)
            .GetProperties()
            .Where(property => {
                if(!namesOfPropertySelected.Contains(property.Name))
                    return false;
                Type typeOfProperty = property.PropertyType;
                return typeOfProperty == typeof(int?) || typeOfProperty == typeof(long?);
            })
            .Select(property => property.Name)
            .OrderBy(name => Array.IndexOf(namesOfPropertySelected, name))
            .ToArray();

        string[] namesShown = columnsOfTable
            .Where(record => namesPropertiesForAnalysis.Contains(record.NameOfProperty))
            .Select(record => record.NameForHuman)
            .ToArray();

        string? userChoice;
        do {
            userChoice = _modelsStatsUserInteractor.ChooseStatisticsToBeShown(namesShown);

            if(userChoice is not null && 
               namesShown.Select(name => name.ToLower()).Contains(userChoice.ToLower())) {
                string nameOfPropertySelected = columnsOfTable
                    .Where(record => record.NameForHuman.ToLower() == userChoice.ToLower())
                    .First()
                    .NameOfProperty;

                PropertyInfo propertyInfo = typeof(T).GetProperty(nameOfPropertySelected)!;
                Func<T, long?> propertySelector = model => {
                    var value = propertyInfo.GetValue(model, null);
                    return value switch {
                        int intValue   => (long?)intValue,
                        long longValue => (long?)longValue,
                        _ => null
                    };
                };
                ShowStatistics(models, nameOfPropertySelected, propertySelector);
            }else if(userChoice == "e" || userChoice == "exit") {
                break;
            } else
                _modelsStatsUserInteractor.ShowMessage("Invalid choice");
        } while(true);
    }


    private void ShowStatistics<T>(
        IEnumerable<T> models,
        string propertyName,
        Func<T, long?> propertySelector) where T : class, IModel
    {
        var maxValue = models.Max(propertySelector);
        ShowStatisticalCharacteristics("Max", propertyName, propertySelector, models, maxValue);

        var preMaxValue = models
            .Select(model => propertySelector(model))
            .Where(x => x is not null)
            .Distinct()
            .OrderByDescending(x => x)
            .Skip(1)
            .First();
        ShowStatisticalCharacteristics("pre-Min", propertyName, propertySelector, models, preMaxValue);

        var averageValue = models
            .Select(model => propertySelector(model))
            .Average();
        ShowStatisticalCharacteristicsFractional("Average", propertyName, propertySelector, models, averageValue);

        var medianValue = models
            .Select(model => propertySelector(model))
            .Where(x => x is not null)
            .Median();
        ShowStatisticalCharacteristicsFractional("Median", propertyName, propertySelector, models, medianValue);

        var preMinValue = models
            .Select(model => propertySelector(model))
            .Where(x => x is not null)
            .Distinct()
            .OrderBy(x => x)
            .Skip(1)
            .First();
        ShowStatisticalCharacteristics("pre-Min", propertyName, propertySelector, models, preMinValue);

        var minValue = models.Min(propertySelector);
        ShowStatisticalCharacteristics("Min", propertyName, propertySelector, models, minValue);
    }


    private void ShowStatisticalCharacteristics<T>(
        string descriptor,
        string propertyName,                   // is not used in the current format of output
        Func<T, long?> propertySelector,
        IEnumerable<T> objects,
        long? value) where T : class, IModel
    {
        var namesOfObjects = objects.Where(obj => propertySelector(obj) == value)
                                    .Select(obj => obj.GetType()
                                                      .GetProperty("Name")?
                                                      .GetValue(obj));
        _modelsStatsUserInteractor.ShowMessage(
            $"{descriptor,-8}: {value,17:N0}" +
            $"  (models: {string.Join(", ", namesOfObjects)})");
    }


    private void ShowStatisticalCharacteristicsFractional<T>(
        string descriptor,
        string propertyName,                   // is not used in the current format of output
        Func<T, long?> propertySelector,
        IEnumerable<T> objects,
        double? value) where T : class, IModel
    {
        var namesOfObjects = objects.Where(obj => propertySelector(obj) == value)
                                    .Select(obj => obj.GetType()
                                                      .GetProperty("Name")?
                                                      .GetValue(obj));
        _modelsStatsUserInteractor.ShowMessage(
            $"{descriptor,-8}: {value,19:N1}" +
            ((namesOfObjects.Any())
                ? $"  (models: {string.Join(", ", namesOfObjects)})"
                : ""));
    }
}
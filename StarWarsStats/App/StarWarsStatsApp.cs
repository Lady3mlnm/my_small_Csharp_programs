using StarWarsStats.DataAccess;
using StarWarsStats.ApiDataAccess;
using StarWarsStats.DTOs;
using StarWarsStats.Model;
using StarWarsStats.UserInteraction;

namespace StarWarsStats.App;
public class StarWarsStatsApp
{
    private readonly IModelsReader _modelsReader;
    private readonly IModelsStatsUserInteractor _modelsStatsUserInteractor;
    private readonly IStatisticsAnalyzer _modelsStatisticsAnalyzer;

    public StarWarsStatsApp(
        IModelsReader modelsReader,
        IModelsStatsUserInteractor modelsStatsUserInteractor,
        IStatisticsAnalyzer modelsStatisticsAnalyzer)
    {
        _modelsReader = modelsReader;
        _modelsStatsUserInteractor = modelsStatsUserInteractor;
        _modelsStatisticsAnalyzer = modelsStatisticsAnalyzer;
    }

    public async Task Run(string requestHost,
        string requestPathPeople,
        string requestPathSpecies,
        string requestPathPlanets,
        IApiDataReader reserveApiDataReaderPeople,
        IApiDataReader reserveApiDataReaderSpecies,
        IApiDataReader reserveApiDataReaderPlanets,
        bool imitateServerError = false)
    {
        ModelType selectedModel = _modelsStatsUserInteractor.GetColumnSorting();

        if(selectedModel == ModelType.Person) {
            var people  = await _modelsReader.Read<Person, ResultPerson>(requestHost, requestPathPeople, reserveApiDataReaderPeople, imitateServerError);
            var species = await _modelsReader.Read<Species, ResultSpecies>(requestHost, requestPathSpecies, reserveApiDataReaderSpecies, imitateServerError);

            _modelsStatisticsAnalyzer.DetermineSpeciesOfPeople(people, species);

            TableColumn[] columnsOfTable  = TableColumns.GetTableColumn("people");

            int yearForCalculatingPeopleAge = _modelsStatsUserInteractor.GetYearForCalculatingAge();

            _modelsStatisticsAnalyzer.DetermineAgeAtYearForPeople(people, yearForCalculatingPeopleAge);

            string? columnSorting = _modelsStatsUserInteractor.GetColumnSorting(columnsOfTable);

            _modelsStatsUserInteractor.ShowModels(people, columnsOfTable, columnSorting,
                                            additionalInfo: "Year for calculating ages of characters: " + yearForCalculatingPeopleAge);

            _modelsStatisticsAnalyzer.Analyze(people, columnsOfTable);
        }else if(selectedModel == ModelType.Species) {
            var species = await _modelsReader.Read<Species, ResultSpecies>(requestHost, requestPathSpecies, reserveApiDataReaderSpecies, imitateServerError);

            TableColumn[] columnsOfTable = TableColumns.GetTableColumn("species");

            string? columnSorting = _modelsStatsUserInteractor.GetColumnSorting(columnsOfTable);

            _modelsStatsUserInteractor.ShowModels(species, columnsOfTable, columnSorting);

            _modelsStatisticsAnalyzer.Analyze(species, columnsOfTable);
        } else if(selectedModel == ModelType.Planet) {
            var planets = await _modelsReader.Read<Planet, ResultPlanet>(requestHost, requestPathPlanets, reserveApiDataReaderPlanets, imitateServerError);

            TableColumn[] columnsOfTable = TableColumns.GetTableColumn("planets");

            string? columnSorting = _modelsStatsUserInteractor.GetColumnSorting(columnsOfTable);

            _modelsStatsUserInteractor.ShowModels(planets, columnsOfTable, columnSorting);

            _modelsStatisticsAnalyzer.Analyze(planets, columnsOfTable);
        }
    }
}
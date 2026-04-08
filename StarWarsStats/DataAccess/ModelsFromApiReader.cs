using StarWarsStats.ApiDataAccess;
using StarWarsStats.DTOs;
using StarWarsStats.Model;
using StarWarsStats.UserInteraction;
using System.Text.Json;

namespace StarWarsStats.DataAccess;

public class ModelsFromApiReader : IModelsReader
{
    private readonly IApiDataReader _apiDataReader;
    private readonly IUserInteractor _userInteractor;

    public ModelsFromApiReader(
        IApiDataReader apiDataReader,
        IUserInteractor userInteractor)
    {
        _apiDataReader = apiDataReader;
        _userInteractor = userInteractor;
    }

    public async Task<List<T>> Read<T, TRoot>(
        string requestHost,
        string requestPath,
        IApiDataReader reserveApiDataReader,
        bool imitateServerError = false) where T : class, IModel
                                         where TRoot : IResultModel
    {
        List<T> FullListOfModel = [];
        Root<TRoot>? root;
        string? json = null;
        bool isMockDataUsed = false;

        _userInteractor.ShowMessage();  // output empty string to visually separate logical blocks of the workflow

        int countPage = 0;
        do {
            countPage++;
            string requestPathWithQuery = (countPage == 1)
                ? requestPath
                : $"{requestPath}//?page={countPage}";
            try {
                _userInteractor.ShowMessage($"Sending a request to URI {requestHost}{requestPathWithQuery}.");

                if(imitateServerError)
                    throw new HttpRequestException("Simulated exception for demonstration purposes.");

                json = await _apiDataReader.Read(requestHost, requestPathWithQuery);
            } catch(HttpRequestException ex) {
                _userInteractor.ShowMessage("API request was unsuccessful. Exception message: " + ex.Message +
                                            "\nSwitching to mock data.");
                isMockDataUsed = true;
            }

            json ??= await reserveApiDataReader.Read(requestHost, requestPathWithQuery);  //??= - null-coalescing assignment operator
                                                                                          //      (it assigns the value only if json is null)

            root = JsonSerializer.Deserialize<Root<TRoot>>(json);

            FullListOfModel.AddRange(ToModel<T, TRoot>(root));
        } while(!isMockDataUsed && root != null && root.next != null);

        if(typeof(T) == typeof(Planet)) {
            Planet planetEarth = new Planet(
                "Earth", 24, 365, 12742, "various", "1 standard", "various", 71, 8_300_000_000);
            FullListOfModel.Add(planetEarth as T);
            _userInteractor.ShowMessage("To the output of the server added data for Earth", ConsoleColor.DarkYellow);
        }

        return FullListOfModel;
    }

    private static List<T> ToModel<T, TRoot>(Root<TRoot>? root) where T : class, IModel
                                                                where TRoot : IResultModel
    {
        if(root is null)
            throw new ArgumentNullException(nameof(root));

        if(typeof(T) == typeof(Person)) {
            return root.results
                   .Select(modelDto => (Person)(modelDto as ResultPerson)!)
                   .OfType<T>()
                   .ToList();
        } else if(typeof(T) == typeof(Species)) {
            return root.results
                   .Select(modelDto => (Species)(modelDto as ResultSpecies)!)
                   .OfType<T>()
                   .ToList();
        } else if(typeof(T) == typeof(Planet)) {
            return root.results
                   .Select(modelDto => (Planet)(modelDto as ResultPlanet)!)
                   .OfType<T>()
                   .ToList();
        } else
            throw new InvalidOperationException($"Unsupported type {typeof(T)}");
    }
}
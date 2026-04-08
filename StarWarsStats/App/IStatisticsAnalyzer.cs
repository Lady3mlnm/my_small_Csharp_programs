using StarWarsStats.Model;
using StarWarsStats.UserInteraction;

namespace StarWarsStats.App;

public interface IStatisticsAnalyzer
{
    public void DetermineAgeAtYearForPeople(IEnumerable<Person> people, string? yearForCalculatingAge);
    public void DetermineAgeAtYearForPeople(IEnumerable<Person> people, int yearForCalculatingAge);
    public void DetermineSpeciesOfPeople(IEnumerable<Person> people, IEnumerable<Species> species);
    void Analyze<T>(IEnumerable<T> models, TableColumn[] columnsOfTable) where T : class, IModel;
}
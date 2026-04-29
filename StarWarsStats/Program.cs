using StarWarsStats.ApiDataAccess;
using StarWarsStats.App;
using StarWarsStats.DataAccess;
using StarWarsStats.UserInteraction;

try {
    string requestHost = "https://swapi.dev/";
    string requestPathPeople  = "api/people";
    string requestPathSpecies = "api/species";
    string requestPathPlanets = "api/planets";
    var consoleUserInteractor = new ConsoleUserInteractor();
    var modelsStatsUserInteractor = new ModelsStatsUserInteractor(
                                        consoleUserInteractor);
    bool imitateServerError = true;    // true imitate API request failure, so mock data will be used

    await new StarWarsStatsApp(
        new ModelsFromApiReader(
            new ApiDataReader(),
            consoleUserInteractor),
        modelsStatsUserInteractor,
        new StatisticsAnalyzer(
            modelsStatsUserInteractor)).Run(requestHost,
                                            requestPathPeople, requestPathSpecies, requestPathPlanets,
                                            new MockApiDataReaderPeople(),
                                            new MockApiDataReaderSpecies(),
                                            new MockApiDataReaderPlanets(),
                                            imitateServerError);
} catch(Exception ex) {
    Console.WriteLine("An error occurred. " +
                      "Exception message: " + ex.Message);
    Console.ReadKey();
}


/* Chronology of the Star Wars fils:
Ep. I. Star Wars: The Phantom Menace: 32 BBY
Ep. II. Star Wars: Attack of the Clones: 22 BBY
Ep. III. Star Wars: Revenge of the Sith: 19 BBY

Ep. IV. Star Wars: A New Hope: 0 BBY/ABY
Ep. V. Star Wars: The Empire Strikes Back: 3 ABY
Ep. VI. Star Wars: Return of the Jedi: 4 ABY

Ep. VII. Star Wars: The Force Awakens: 34 ABY
Ep. VIII. Star Wars: The Last Jedi: 34 ABY
Ep. IX. Star Wars: The Rise of Skywalker: 35 ABY
Data source:
https://www.empireonline.com/movies/features/star-wars-timeline-chronological-order/
https://www.officetimeline.com/blog/star-wars-timeline-how-to-watch-star-wars-in-chronological-order
*/


//There's a discrepancy regarding the birth year of Qui-Gon Jinn:
//   - 92 BBY - Legends  (https://starwars.fandom.com/wiki/Qui-Gon_Jinn/Legends)
//   - 80 BBY - Canon    (https://starwars.fandom.com/wiki/Qui-Gon_Jinn)
//The same discrepancy regarding the droid C-3PO
//   - 112 BBY - Legends  (https://starwars.fandom.com/wiki/C-3PO/Legends)
//   -  32 BBY - Canon    (https://starwars.fandom.com/wiki/C-3PO)
//I suppose that similar discrepancies there're also about some other personages.


// Not: Sly Moore belongs to the species Umbaran.
// Her birth year is not officially documented in Star Wars Canon.
// According to a fan wiki, force-sensitives umbarans can live up to 200 years.


//——— WARNING: loss of precision in the table ———
//1. Database "people"
// The database says that "Anakin Skywalker" / "Darth Vader " was born 41.9BBY, Boba Fett was born 31.5BBY.
// Boba Fett has mass 78.2 kg, Luminara Unduli has mass 56.2 kg.
// But the program treates fields "BirthYear", "Age" and "Height" as integer, so I've implemented rounding.
//2. Database "planets"
// The database says that "Utapau" has surface water "0.9"
// But the program designed to take this field as integer, so I've implemented rounding.


/*
 Conversion of JSON to C# class:
- go to site https://json2csharp.com, insert JSON to the left field
- add Attributes/Decorators > "Use JsonPropertyName (.NET Core)" [for built-in System.Text.Json]
  (alternative: Attributes/Decorators > "Add JsonProperty Attributes" [for Json.NET by Newtownsoft]
- add "Class Settings" > "Generate Immutable Classes" and "Use Record Types"
*/


// DTOs - Data Transfer Objects

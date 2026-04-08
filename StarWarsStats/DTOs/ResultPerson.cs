using System.Text.Json.Serialization;

namespace StarWarsStats.DTOs;

public record ResultPerson(
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("height")] string height,
    [property: JsonPropertyName("mass")] string mass,
    [property: JsonPropertyName("hair_color")] string hair_color,
    [property: JsonPropertyName("skin_color")] string skin_color,
    [property: JsonPropertyName("eye_color")] string eye_color,
    [property: JsonPropertyName("birth_year")] string birth_year,
    [property: JsonPropertyName("gender")] string gender,
    [property: JsonPropertyName("homeworld")] string homeworld,
    [property: JsonPropertyName("films")] IReadOnlyList<string> films,
    [property: JsonPropertyName("species")] IReadOnlyList<string> species,
    [property: JsonPropertyName("vehicles")] IReadOnlyList<string> vehicles,
    [property: JsonPropertyName("starships")] IReadOnlyList<string> starships,
    [property: JsonPropertyName("created")] DateTime created,
    [property: JsonPropertyName("edited")] DateTime edited,
    [property: JsonPropertyName("url")] string url
) : IResultModel;
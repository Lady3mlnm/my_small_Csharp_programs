using System.Text.Json.Serialization;

namespace StarWarsStats.DTOs;

public record ResultSpecies(
    [property: JsonPropertyName("name")] string name,
    [property: JsonPropertyName("classification")] string classification,
    [property: JsonPropertyName("designation")] string designation,
    [property: JsonPropertyName("average_height")] string average_height,
    [property: JsonPropertyName("skin_colors")] string skin_colors,
    [property: JsonPropertyName("hair_colors")] string hair_colors,
    [property: JsonPropertyName("eye_colors")] string eye_colors,
    [property: JsonPropertyName("average_lifespan")] string average_lifespan,
    [property: JsonPropertyName("homeworld")] string homeworld,
    [property: JsonPropertyName("language")] string language,
    [property: JsonPropertyName("people")] IReadOnlyList<string> people,
    [property: JsonPropertyName("films")] IReadOnlyList<string> films,
    [property: JsonPropertyName("created")] DateTime created,
    [property: JsonPropertyName("edited")] DateTime edited,
    [property: JsonPropertyName("url")] string url
) : IResultModel;
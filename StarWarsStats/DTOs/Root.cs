using System.Text.Json.Serialization;

namespace StarWarsStats.DTOs;

public record Root<T>(
    [property: JsonPropertyName("count")] int count,
    [property: JsonPropertyName("next")] string next,
    [property: JsonPropertyName("previous")] object previous,
    [property: JsonPropertyName("results")] IReadOnlyList<T> results
) where T : IResultModel;
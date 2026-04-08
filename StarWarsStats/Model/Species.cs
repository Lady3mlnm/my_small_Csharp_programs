using StarWarsStats.DTOs;
using StarWarsStats.Utilities;

namespace StarWarsStats.Model;

public record Species : IModel
{
    private static int _counterId = 0;
    public int Id { get; }
    public string Name { get; }
    public string? Classification { get; }
    public string? Designation { get; }
    public int? AverageHeight { get; }
    public string? SkinColors { get; }
    public string? HairColors { get; }
    public string? EyeColors { get; }
    public int? Lifespan { get; }
    public string? Language { get; }
    public Species(
        string name,
        string? classification,
        string? designation,
        int? averageHeight,
        string? skinColors,
        string? hairColors,
        string? eyeColors,
        int? lifespan,
        string? language)
    {
        Id = ++_counterId;
        Name = name;
        Classification = classification;
        Designation = designation;
        AverageHeight = averageHeight;
        SkinColors = skinColors;
        HairColors = hairColors;
        EyeColors = eyeColors;
        Lifespan = lifespan;
        Language = language;
    }

    public static explicit operator Species(ResultSpecies speciesDto)
    {
        var name = speciesDto.name;
        var classification = speciesDto.classification;
        var designation = speciesDto.designation;
        var averageHeight = speciesDto.average_height.ToIntOrNull();
        var skinColors = speciesDto.skin_colors;
        var hairColors = speciesDto.hair_colors;
        var eyeColors = speciesDto.eye_colors;
        var lifespan = speciesDto.average_lifespan.ToIntOrNull();
        var language = speciesDto.language;

        return new Species(name, classification, designation, averageHeight,
                           skinColors, hairColors, eyeColors, lifespan, language);
    }
}
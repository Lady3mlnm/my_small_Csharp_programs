using StarWarsStats.DTOs;
using StarWarsStats.Utilities;

namespace StarWarsStats.Model;

public record Person : IModel
{
    public string Name { get; }
    public int? Height { get; }
    public int? Mass { get; }
    public string? HairColor { get; }
    public string? SkinColor { get; }
    public string? EyeColor { get; }
    public int? BirthYear { get; }
    public string? Gender { get; }
    public string? Species { get; private set; }  // as name of a species or, for intermediate purpose, as "id_" + id
    public int? Lifespan { get; private set; }
    public int? Age { get; private set; }

    public Person(
        string name,
        int? height,
        int? mass,
        string? hairColor,
        string? skinColor,
        string? eyeColor,
        int? birthYear,
        string? gender,
        string? species = null,
        int? lifespan = null,
        int? age = null)
    {
        if(name is null)
            throw new ArgumentNullException(nameof(name));

        Name = name;
        Height = height;
        Mass = mass;
        HairColor = hairColor;
        SkinColor = skinColor;
        EyeColor = eyeColor;
        BirthYear = birthYear;
        Gender = gender;
        Species = species;
        Lifespan = lifespan;
        Age = age;
    }


    public static explicit operator Person(ResultPerson personDto)
    {
        var name = personDto.name;
        int? height = personDto.height.ToIntOrNull();
        int? mass = personDto.mass.ToIntOrNull();
        string? hairColor = personDto.hair_color;
        string? skinColor = personDto.skin_color;
        string? eyeColor = personDto.eye_color;
        int? birthYear = personDto.birth_year.ToStarWarsYearIntOrNull();
        string? gender = personDto.gender;
        string? species = (personDto.species.Count) switch {
            0 => null,
            1 => "id_" + personDto.species[0].Split('/')[^2],
            2 => throw new ArgumentException($"{personDto.name} belongs to several species simultaneously")
        };

        return new Person(name, height, mass, hairColor, skinColor,
                                eyeColor, birthYear, gender, species);
    }


    public void DetermineAgeAtYear(int yearForCalculatingAge) {
        if(BirthYear is null)
            Age = null;
        else {
            int age = yearForCalculatingAge - BirthYear.Value;
            Age = (age < 0) ? null : age;
        }
    }


    public void SetSpecies(string species) {
        Species = species;
    }


    public void SetLifespan(int? lifespan) {
        Lifespan = lifespan;
    }
}
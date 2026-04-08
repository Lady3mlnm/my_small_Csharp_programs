using StarWarsStats.DTOs;
using StarWarsStats.Utilities;

namespace StarWarsStats.Model;

public record Planet : IModel
{
    public string Name { get; }
    public int? RotationPeriod { get; }
    public int? OrbitalPeriod { get; }
    public int? Diameter { get; }
    public string? Climate { get; }
    public string? Gravity { get; }
    public string? Terrain { get; }
    public int? SurfaceWater { get; }
    public long? Population { get; }

    public Planet(
        string name,
        int? rotationPeriod,
        int? orbitalPeriod,
        int? diameter,
        string? climate,
        string? gravity,
        string? terrain,
        int? surfaceWater,
        long? population)
    {
        if(name is null)
            throw new ArgumentNullException(nameof(name));

        Name = name;
        RotationPeriod = rotationPeriod;
        OrbitalPeriod = orbitalPeriod;
        Diameter = diameter;
        Climate = climate;
        Gravity = gravity;
        Terrain = terrain;
        SurfaceWater = surfaceWater;
        Population = population;
    }


    public static explicit operator Planet(ResultPlanet planetDto)
    {
        var name = planetDto.name;
        int? rotationPeriod = planetDto.rotation_period.ToIntOrNull();
        int? orbitalPeriod = planetDto.orbital_period.ToIntOrNull();
        int? diameter = planetDto.diameter.ToIntOrNull();
        string? climate = planetDto.climate;
        string? gravity = planetDto.gravity;
        string? terrain = planetDto.terrain;
        int? surfaceWater = planetDto.surface_water.ToIntOrNull();
        long? population = planetDto.population.ToLongOrNull();

        return new Planet(name, rotationPeriod, orbitalPeriod, diameter,
                          climate, gravity, terrain, surfaceWater, population);
    }
}
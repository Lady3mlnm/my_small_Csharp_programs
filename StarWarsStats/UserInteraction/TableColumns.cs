namespace StarWarsStats.UserInteraction;

public static class TableColumns
{
    public static readonly TableColumn[] columnsOfTablePeople = [
        new TableColumn("name",       "Name",      21, "Name"),
        new TableColumn("species",    "Species",   14, "Species"),
        new TableColumn("birth year", "BirthYear",  9, "BirthYear"),
        new TableColumn("age",        "Age",        4, "Age"),
        new TableColumn("lifespan",   "Lifespan",   8, "Lifespan"),
        new TableColumn("height",     "Height",     6, "Height"),
        new TableColumn("mass",       "Mass",       5, "Mass"),
        new TableColumn("gender",     "Gender",    12, "Gender"),
        new TableColumn("hair color", "HairColor",  8, "HairColor"),
        new TableColumn("skin color", "SkinColor", 16, "SkinColor"),
        new TableColumn("eye color",  "EyeColor",   7, "EyeColor")
    ];

    public static readonly TableColumn[] columnsOfTableSpecies = [
        new TableColumn("name",           "Name",       21, "Name"),
        new TableColumn("classification", "Classific.", 10, "Classification"),
	    //new TableColumn("designation", "Designation", 11, "Designation"),
	    new TableColumn("lifespan",       "Lifespan",    8, "Lifespan"),
        new TableColumn("average height", "AvHeight",    8, "AverageHeight"),
        new TableColumn("language",       "Language",   14, "Language"),
        new TableColumn("hair colors",    "HairColors", 18, "HairColors"),
        new TableColumn("skin colors",    "SkinColors", 18, "SkinColors"),
        new TableColumn("eye colors",     "EyeColors",  16, "EyeColors")
    ];

    public static readonly TableColumn[] columnsOfTablePlanets = [
        new TableColumn("name",           "Name",       14, "Name"),
        new TableColumn("diameter",       "Diameter",    8, "Diameter"),
        new TableColumn("orbital period", "OrbPrd",      6, "OrbitalPeriod"),
        new TableColumn("surface water",  "SWater",      4, "SurfaceWater"),
        new TableColumn("population",     "Population", 17, "Population"),
        new TableColumn("climate",        "Climate",    25, "Climate"),
        new TableColumn("terrain",        "Terrain",    40, "Terrain")
        //new TableColumn("rotation period", "RotPrd",   6, "RotationPeriod"),
        //new TableColumn("gravity",         "Gravity", 13, "Gravity")
    ];

    private static readonly Dictionary<string, TableColumn[]> _tables = new() {
        ["people"]   = columnsOfTablePeople,
        ["species"]  = columnsOfTableSpecies,
        ["planets"]  = columnsOfTablePlanets
    };

    public static TableColumn[] GetTableColumn(string key) =>
        _tables[key];
}
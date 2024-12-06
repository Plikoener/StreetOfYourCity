namespace StreetOfYourCity.Services.LocationDataServices.Dto;

public class StreetEntry
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public GeoPosition Center { get; } = new();

    public override string ToString()
    {
        return $"StreetEntry Id {Id} Name {Name} Center {Center}";
    }
}
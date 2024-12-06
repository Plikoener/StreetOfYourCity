namespace StreetOfYourCity.Services.LocationDataServices.Dto;

public class GeoPosition
{
    public double Latitude { get; set; } = 0;
    public double Longitude { get; set; } = 0;

    public override string ToString()
    {
        return $"GeoPosition Latitude {Latitude} Longitude {Longitude}";
    }
}
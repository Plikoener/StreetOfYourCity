namespace StreetOfYourCity.Services.LocationDataServices.Dto;

public class MapPoint
{
    public double Latitude { get; set; } = 0;
    public double Longitude { get; set; } = 0;
    
    public MapPoint(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }

    public override string ToString()
    {
        return $"GeoPosition Latitude {Latitude} Longitude {Longitude}";
    }
}
namespace StreetOfYourCity.Services.LocationDataServices.Dto;

public class MapPoint
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public MapPoint()
    {
    }

    public MapPoint(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }
}

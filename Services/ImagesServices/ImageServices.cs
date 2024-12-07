using StreetOfYourCity.Models;
using StreetOfYourCity.Services.LocationDataServices.Dto;
using System.Globalization;
using System.Reactive.Disposables;
using System.Text.Json;

namespace StreetOfYourCity.Services.ImagesServices;

public class ImageServices : IImageServices
{
    private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private const double Wgs84A = 6378137.0;
    private const double Wgs84B = 6356752.3;

    private const string ApiAccessUrl = "https://graph.mapillary.com/images";
    private readonly string _apiAccessToken;

    public ImageServices(string apiApiAccessToken)
    {
        _apiAccessToken = apiApiAccessToken;
    }

    public async Task<IList<ImageServiceModel>> GetMapillaryImagesUrl(MapPoint mapPoint, double halfSideInKm)
    {
        (MapPoint minPoint, MapPoint maxPoint) = GetImagePointRadii(mapPoint, halfSideInKm);

        if (minPoint == null || maxPoint == null) throw new InvalidOperationException("Bounding points cannot be null.");

        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("Authorization", $"OAuth {_apiAccessToken}");

        const string fieldName = "thumb_256_url";
        string url =
            $"{ApiAccessUrl}?fields={fieldName},creator,captured_at&bbox={minPoint.Longitude.ToString(CultureInfo.InvariantCulture)},{minPoint.Latitude.ToString(CultureInfo.InvariantCulture)}" +
            $",{maxPoint.Longitude.ToString(CultureInfo.InvariantCulture)},{maxPoint.Latitude.ToString(CultureInfo.InvariantCulture)}" +
            "&limit=3";

        HttpResponseMessage response = await client.GetAsync(url);

        //if (!response.DisposeWith()) throw new HttpRequestException($"Unexpected api status code. - {response.StatusCode}");

        string jsonData = await response.Content.ReadAsStringAsync();
        JsonElement jsonRootElement = JsonDocument.Parse(jsonData).RootElement;

        if (jsonRootElement.TryGetProperty("data", out JsonElement imageData) && imageData.GetArrayLength() > 0)
        {
            IList<ImageServiceModel> imageList = new List<ImageServiceModel>();

            for (int arrayPosition = 0; arrayPosition < imageData.GetArrayLength(); arrayPosition++)
            {
                string imageUrl = imageData[0].GetProperty(fieldName).GetString()!;
                DateTime imageRecordTime = DateTimeFromUnixTimestampMilliSeconds(imageData[0].GetProperty("captured_at").GetInt64());
                string? imageCreator = imageData[0].GetProperty("creator").GetProperty("username").GetString();

                imageList.Add(new ImageServiceModel(imageUrl, imageRecordTime, imageCreator));
            }

            return imageList;
        }

        return new List<ImageServiceModel>();
    }

    private static (MapPoint, MapPoint) GetImagePointRadii(MapPoint point, double halfSideInKm)
    {
        double lat = DegreesToRadians(point.Latitude);
        double lon = DegreesToRadians(point.Longitude);
        double halfSide = 1000 * halfSideInKm;

        double radius = Wgs84EarthRadius(lat);
        double pradius = radius * Math.Cos(lat);

        double latMin = lat - halfSide / radius;
        double latMax = lat + halfSide / radius;
        double lonMin = lon - halfSide / pradius;
        double lonMax = lon + halfSide / pradius;

        MapPoint minPoint = new()
        {
            Latitude = RadiansToDegrees(latMin),
            Longitude = RadiansToDegrees(lonMin)
        };

        MapPoint maxPoint = new()
        {
            Latitude = RadiansToDegrees(latMax),
            Longitude = RadiansToDegrees(lonMax)
        };

        return (minPoint, maxPoint);
    }

    private static double DegreesToRadians(double degrees)
    {
        return Math.PI * degrees / 180.0;
    }

    private static double RadiansToDegrees(double radians)
    {
        return 180.0 * radians / Math.PI;
    }

    private static double Wgs84EarthRadius(double latitude)
    {
        double scaledMajorAxisSquared = Wgs84A * Wgs84A * Math.Cos(latitude);
        double scaledMinorAxisSquared = Wgs84B * Wgs84B * Math.Sin(latitude);
        double projectedMajorAxis = Wgs84A * Math.Cos(latitude);
        double projectedMinorAxis = Wgs84B * Math.Sin(latitude);
        return Math.Sqrt((scaledMajorAxisSquared * scaledMajorAxisSquared + scaledMinorAxisSquared * scaledMinorAxisSquared) / (projectedMajorAxis * projectedMajorAxis + projectedMinorAxis * projectedMinorAxis));
    }

    private static DateTime DateTimeFromUnixTimestampMilliSeconds(long milliSeconds)
    {
        return UnixEpoch.AddMilliseconds(milliSeconds);
    }
}

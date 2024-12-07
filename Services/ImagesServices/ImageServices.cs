using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Internals;
using StreetOfYourCity.Services.ImagesServices.Dto;
using StreetOfYourCity.Services.LocationDataServices.Dto;

namespace StreetOfYourCity.Services.ImagesServices;

public class ImageServices : IImageServices
{
    private static readonly DateTime UnixEpoch = new( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );

    private readonly ILogger<ImageServices> _logger;
    private readonly string _accessToken;
    
    public ImageServices(ILogger<ImageServices> logger, string accessToken)
    {
        _logger = logger;
        _accessToken = accessToken;
        logger.LogDebug("ctor");
    }

    private class ApiSearchResult
    {
        public string? Url1 { get; set; }
        public DateTime Created1 { get; set; }
        public string? Creator1 { get; set; }

        public string? Url2 { get; set; }
        public DateTime Created2 { get; set; }
        public string? Creator2 { get; set; }

        public string? Url3 { get; set; }
        public DateTime Created3 { get; set; }
        public string? Creator3 { get; set; }
    }

    private  async Task<ApiSearchResult?> GetMapillaryImageUrlAsync(MapPoint mapPoint, double halfSideInKm)
    {
        var bbox = GeoHelper.GetBoundingBox(mapPoint, halfSideInKm);
        const string apiUrl = "https://graph.mapillary.com/images";

        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"OAuth {_accessToken}");


        // Anfrage-URL mit Koordinaten und den gewünschten Feldern  ( minLon, minLat, maxLon, maxLat).   thumb_2048
        var fieldName = "thumb_256_url"; //thumb_2048_url
        string url =
            $"{apiUrl}?fields={fieldName},creator,captured_at&bbox={bbox.MinPoint.Longitude.ToString(CultureInfo.InvariantCulture)},{bbox.MinPoint.Latitude.ToString(CultureInfo.InvariantCulture)}" +
            $",{bbox.MaxPoint.Longitude.ToString(CultureInfo.InvariantCulture)},{bbox.MaxPoint.Latitude.ToString(CultureInfo.InvariantCulture)}" +
            "&limit=3";
        _logger.LogDebug(url);
        
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                // Console.WriteLine(json);
                _logger.LogDebug(json);

                // JSON-Daten parsen
                JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("data", out JsonElement data) && data.GetArrayLength() > 0)
                {
                    _logger.LogDebug("Anzahl URLs : " + data.GetArrayLength());

                    // URL des ersten Bildes extrahieren
                    var result = new ApiSearchResult();

                    for (var i = 0; i < data.GetArrayLength(); i++)
                    {
                        if (i == 0)
                        {
                            result.Url1 = data[0].GetProperty(fieldName).GetString();
                            result.Creator1 = data[0].GetProperty("creator").GetProperty("username").GetString();
                            result.Created1 = DateTimeFromUnixTimestampMilliSeconds(data[0].GetProperty("captured_at").GetInt64());
                        }
                        else if (i == 1)
                        {
                            result.Url2 = data[1].GetProperty(fieldName).GetString();
                            result.Creator2 = data[1].GetProperty("creator").GetProperty("username").GetString();
                            result.Created2 = DateTimeFromUnixTimestampMilliSeconds(data[1].GetProperty("captured_at").GetInt64());
                        }
                        else if (i == 2)
                        {
                            result.Url3 = data[2].GetProperty(fieldName).GetString();
                            result.Creator3 = data[2].GetProperty("creator").GetProperty("username").GetString();
                            result.Created3 = DateTimeFromUnixTimestampMilliSeconds(data[2].GetProperty("captured_at").GetInt64());
                        }
                        else
                        {
                            break;
                        }
                    }
                    return result;
                }
            }
            else
            {
                var test = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error during Api Call: {StatusCode} {test}", response.StatusCode, test);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Exception : {message}", ex.Message);
        }

        return null;
    }

    public async Task<ImageResult?> GetImageForMapPoint(MapPoint mapPoint, double maxDistance = 0.3)
    {
        var urlResult = await GetMapillaryImageUrlAsync(mapPoint, maxDistance);

        if (urlResult == null)
        {
            _logger.LogDebug("nothing found");
            return null;
        }

        //Todo Download Image hier not on showing !

        var result = new ImageResult();
        result.Creator1 = urlResult.Creator1;
        result.Created1 = urlResult.Created1;
        if (!string.IsNullOrEmpty(urlResult.Url1))
        {
            result.Image1 = ImageSource.FromUri(new Uri(urlResult.Url1));
        }

        result.Creator2 = urlResult.Creator2;
        result.Created2 = urlResult.Created2;
        if (!string.IsNullOrEmpty(urlResult.Url2))
        {
            result.Image2 = ImageSource.FromUri(new Uri(urlResult.Url2));
        }
        
        result.Creator3 = urlResult.Creator3;
        result.Created3 = urlResult.Created3;
        if (!string.IsNullOrEmpty(urlResult.Url3))
        {
            result.Image3 = ImageSource.FromUri(new Uri(urlResult.Url3));
        }

        return result;
    }

    public static DateTime DateTimeFromUnixTimestampMilliSeconds( long milliSeconds )
    {
        return UnixEpoch.AddMilliseconds(milliSeconds);
    }

    public static async Task<ImageSource?> DownloadAsImageAsync(string imageUrl)
    {
        /*using var client = new HttpClient();
        using var response = await client.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead);
        using var stream = await response.Content.ReadAsStreamAsync();

        var imageSource = new BitmapImage();
        imageSource.BeginInit();
        imageSource.StreamSource = stream;
        imageSource.EndInit();
        return ImageSource.FromStream(p => stream);
        */

        return null;
    }

    public static async Task DownloadLargeImageAsync(string imageUrl, string fileName)
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead);
        await using var stream = await response.Content.ReadAsStreamAsync();
        await using var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
        await stream.CopyToAsync(fileStream);
    }
}

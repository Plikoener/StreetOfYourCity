using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using StreetOfYourCity.Services.LocationDataServices.Dto;

namespace StreetOfYourCity.Services.ImagesServices;

public class ImageServices : IImageServices
{

    private readonly ILogger<ImageServices> _logger;

    public ImageServices(ILogger<ImageServices> logger)
    {
        _logger = logger;
        logger.LogDebug("ctor");
    }
    private  async Task<string> GetMapillaryImageUrlAsync(MapPoint mapPoint, string accessToken)
    {
        var bbox = GeoHelper.GetBoundingBox(mapPoint, 0.3);
        const string apiUrl = "https://graph.mapillary.com/images";

        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"OAuth {accessToken}");


        // Anfrage-URL mit Koordinaten und den gewünschten Feldern  ( minLon, minLat, maxLon, maxLat).   thumb_2048
        var fieldName = "thumb_256_url"; //thumb_2048_url
        string url = $"{apiUrl}?fields={fieldName}&bbox={bbox.MinPoint.Longitude.ToString(CultureInfo.InvariantCulture)},{bbox.MinPoint.Latitude.ToString(CultureInfo.InvariantCulture)}" +
                                                        $",{bbox.MaxPoint.Longitude.ToString(CultureInfo.InvariantCulture)},{bbox.MaxPoint.Latitude.ToString(CultureInfo.InvariantCulture)}";
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
                    return data[0].GetProperty(fieldName).GetString();
                }
            }
            else
            {
                Console.WriteLine($"Fehler bei der API-Anfrage: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
        }

        return null;
    }
}

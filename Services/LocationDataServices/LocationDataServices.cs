using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Security.Policy;
using System.Text;

namespace StreetOfYourCity.Services.LocationDataServices;

public class LocationDataServices : ILocationDataServices
{
    private readonly ILogger<LocationDataServices> _logger;
    private const string OverpassApi = "http://overpass-api.de/api/interpreter";

    public LocationDataServices(ILogger<LocationDataServices> logger)
    {
        _logger = logger;
        logger.LogDebug("ctor");
    }

    public async Task<StreetSearchResult?> GetStreetsForCity(string city)
    {
        _logger.LogDebug("searching for streets of {city}", city);

        StreetSearchResult? result;
        try
        {
            result = await GetStreetNodes(city);

            if (result == null) return result;
        }
        catch (Exception exp)
        {
            _logger.LogError(exp, "GetStreetsForCity failed {message}", exp.Message);
            throw;
        }

        return result;
    }

    private async Task<StreetSearchResult?> GetStreetNodes(string city)
    {
        // Overpass-Abfrage
        var overpassQuery = $@"
        [out:json];
        area[name=""{city}""]->.searchArea;
        (
          way[""highway""](area.searchArea);
        );
        out body;";

        using HttpClient client = new HttpClient();

        StreetSearchResult? result = null;

        // Anfrage an die Overpass API senden
        var content = new StringContent(overpassQuery, Encoding.UTF8, "text/plain");
        var response = await client.PostAsync(OverpassApi, content);

        if (response.IsSuccessStatusCode)
        {
            // JSON-Daten verarbeiten
            var jsonResponse = await response.Content.ReadAsStringAsync();
            _logger.LogDebug("answer {jsonResponse}", jsonResponse);

            JObject data = JObject.Parse(jsonResponse);

            // Extrahieren der Straßeninformationen
            if (data["elements"] != null)
            {
                foreach (var element in data["elements"]!)
                {
                    var tags = element["tags"];
                    if(tags == null || tags["name"] == null)
                    {
                        continue;
                    }

                    if (element["type"]!.ToString() == "way" && !string.IsNullOrEmpty(tags["name"]!.ToString()))
                    {
                        Console.WriteLine($"Straße ID: {element["id"]}");

                        Console.WriteLine($"Straße Name: {tags["name"]}");
                        // Sie können auch andere Informationen wie `element["tags"]["name"]` abrufen

                        var nodes = element["nodes"];

                        var streetName = tags["name"]!.ToString();
                        /*if (_streetInfos.ContainsKey(streetName))
                        {
                            _streetInfos[streetName].Ids.Add(long.Parse(element["id"].ToString()));
                        }
                        else
                        {
                            var streetInfo = new StresstInfo();
                            streetInfo.Ids.Add(long.Parse(element["id"]!.ToString()));
                                
                            foreach (var node in nodes!)
                            {
                                streetInfo.Nodes.Add(long.Parse(node.ToString()));
                            }

                            _streetInfos.Add(streetName, streetInfo);
                        }*/
                    }
                }
                /*await GetPositions();

                var count = 0;
                foreach (var entry in _streetInfos)
                {
                    Console.WriteLine($"Straße: {entry}");
                    count++;
                    if (count > 11)
                    {
                        break;
                    }
                }*/
            }
        }
        else
        {
            _logger.LogError("Http Error {StatusCode}", response.StatusCode);
        }

        return result;
    }
}

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using StreetOfYourCity.Services.LocationDataServices.Dto;

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

        var content = new StringContent(overpassQuery, Encoding.UTF8, "text/plain");
        var response = await client.PostAsync(OverpassApi, content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            _logger.LogDebug("answer {jsonResponse}", jsonResponse);

            var overpassWayResult = JsonConvert.DeserializeObject<OverpassWayResult>(jsonResponse);

            if (overpassWayResult?.Elements == null) return result;

            foreach (var element in overpassWayResult.Elements)
            {
                var tags = element.Tags;
                if(tags == null || string.IsNullOrEmpty(tags.Name))
                {
                    continue;
                }

                result ??= new StreetSearchResult();

                //Todo Streets ca have more than one Id...
                result.Streets.Add(new StreetEntry
                {
                    Id = element.Id,
                    Name = tags.Name

                });
            }

            result = await GetPositions(result);

            if (result == null)
            {
                _logger.LogDebug("Nothing found return null");
                return result;
            }
#if DEBUG
            foreach (var entry in result.Streets)
            {
                _logger.LogTrace("Street: {entry}", entry);
            }
#endif
            _logger.LogDebug("{count} streets found", result.Streets.Count);
        }
        else
        {
            _logger.LogError("Http Error {StatusCode}", response.StatusCode);
        }

        return result;
    }

    private async Task<StreetSearchResult?> GetPositions(StreetSearchResult? result)
    {
        if(result?.Streets  == null) return null;

        var listIds = string.Empty;
        foreach (var entry in result.Streets)
        {
            if (!string.IsNullOrEmpty(listIds))
            {
                listIds += ",";
            }
            listIds += entry.Id;
        }

        var overpassQuery = $"""
                             [out:json];
                             way(id:{listIds});
                             out center;
                             """;

        using var client = new HttpClient();

        var content = new StringContent(overpassQuery, Encoding.UTF8, "text/plain");
        var response = await client.PostAsync(OverpassApi, content);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Http Error {StatusCode}", response.StatusCode);
            return null;
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var data = JObject.Parse(jsonResponse);

        if (data["elements"] == null) return result;

        foreach (var element in data["elements"]!)
        {
            var id = long.Parse(element["id"]!.ToString());
            var entry = result.Streets.FirstOrDefault(p => p.Id == id);

            if(entry == null) continue;

            entry.Center.Longitude = double.Parse(element["center"]!["lon"]!.ToString());
            entry.Center.Latitude = double.Parse(element["center"]!["lat"]!.ToString());
        }

        return result;
    }
}

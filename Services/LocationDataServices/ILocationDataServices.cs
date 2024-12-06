using StreetOfYourCity.Services.LocationDataServices.Dto;

namespace StreetOfYourCity.Services.LocationDataServices;

public interface ILocationDataServices
{
    Task<StreetSearchResult?> GetStreetsForCity(string city);
}

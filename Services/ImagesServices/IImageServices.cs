using StreetOfYourCity.Services.ImagesServices.Dto;
using StreetOfYourCity.Services.LocationDataServices.Dto;

namespace StreetOfYourCity.Services.ImagesServices;

public interface IImageServices
{
    Task<ImageResult?> GetImageForMapPoint(MapPoint mapPoint, double maxDistance = 0.3);
}

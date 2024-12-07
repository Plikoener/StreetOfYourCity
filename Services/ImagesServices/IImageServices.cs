using StreetOfYourCity.Services.LocationDataServices.Dto;

namespace StreetOfYourCity.Services.ImagesServices;

public interface IImageServices
{
    Task<ImageSource> GetImageForMapPoint(MapPoint mapPoint, double maxDistance = 0.3);
}

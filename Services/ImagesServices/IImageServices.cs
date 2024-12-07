using StreetOfYourCity.Models;
using StreetOfYourCity.Services.LocationDataServices.Dto;

namespace StreetOfYourCity.Services.ImagesServices;

interface IImageServices
{
    public Task<IList<ImageServiceModel>> GetMapillaryImagesUrl(MapPoint mapPoint, double halfSideInKm);
}

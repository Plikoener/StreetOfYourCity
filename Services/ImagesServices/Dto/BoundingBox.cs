using StreetOfYourCity.Services.LocationDataServices.Dto;

namespace StreetOfYourCity.Services.ImagesServices.Dto
{
    public class BoundingBox
    {
        public MapPoint? MinPoint { get; set; }
        public MapPoint? MaxPoint { get; set; }
    }
}

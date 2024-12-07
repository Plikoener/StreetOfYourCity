using StreetOfYourCity.Models;
using StreetOfYourCity.Services.ImagesServices;
using StreetOfYourCity.Services.LocationDataServices.Dto;

namespace StreetOfYourCity.ServiceTests;

class Program
{
    static async Task Main()
    {
        await TestImagesService();
    }

    private static async Task TestImagesService()
    {
        ImageServices service = new("MLY|9409597912402725|e0637e9824e9d881c6df22ea6ef05f09");
        MapPoint mapPoint = new(10.0253094, 54.5895856);
        IList<ImageServiceModel> result = await service.GetMapillaryImagesUrl(mapPoint, 5);

        foreach (ImageServiceModel image in result)
        {
            Console.WriteLine($"Image {image.Url} {image.RecordTime} {image.Creator}");
        }

        Console.ReadKey();
    }
}

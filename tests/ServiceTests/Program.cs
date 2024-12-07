using Microsoft.Extensions.Logging;
using StreetOfYourCity.Services.ImagesServices;
using StreetOfYourCity.Services.LocationDataServices;
using StreetOfYourCity.Services.LocationDataServices.Dto;

namespace StreetOfYourCity.ServiceTests
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Todo read from appsettings
            using ILoggerFactory factory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Trace);
            });
            
            ILogger logger = factory.CreateLogger("Program");

            logger.LogInformation("Hello World! Logging is {Description}.", "fun");
            
            //await Test1(factory, logger);

            await Test2(factory, logger);
        }

        private static async Task Test1(ILoggerFactory factory, ILogger logger)
        {
            var service = new LocationDataServices(factory.CreateLogger<LocationDataServices>());

            logger.LogDebug("Start search");

            await service.GetStreetsForCity("Damp");
        }

        private static async Task Test2(ILoggerFactory factory, ILogger logger)
        {
            var accessToken = Environment.GetEnvironmentVariable("MAPILLARY_ACCESS_TOKEN");

            var service = new ImageServices(factory.CreateLogger<ImageServices>(), accessToken!);

            var mapPoint = new MapPoint(10.0253094, 54.5895856);

            var result = await service.GetImageForMapPoint(mapPoint, 0.5);

            if (result == null)
            {
                Console.WriteLine("nothing found");
                return;
            }

            Console.WriteLine($"Created {result.Created1} from {result.Creator1}");

            if (!string.IsNullOrEmpty(result.Creator2))
            {
                Console.WriteLine($"Created {result.Created2} from {result.Creator2}");
            }

            if (!string.IsNullOrEmpty(result.Creator3))
            {
                Console.WriteLine($"Created {result.Created3} from {result.Creator3}");
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using StreetOfYourCity.Services.LocationDataServices;

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
        }
    }
}

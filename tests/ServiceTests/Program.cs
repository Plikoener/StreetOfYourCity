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

            var service = new LocationDataServices(factory.CreateLogger<LocationDataServices>());

            logger.LogDebug("Start search");

            await service.GetStreetsForCity("Damp");
        }
    }
}

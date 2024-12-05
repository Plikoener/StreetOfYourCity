using StreetOfYourCity.Pages.MainPage.Factories;
using StreetOfYourCity.Services.GameServices;
using StreetOfYourCity.Services.ImagesServices;
using StreetOfYourCity.Services.LocationDataServices;

namespace StreetOfYourCity.Configurations;

public static class ServiceConfigurations
{
    public static IServiceCollection ConfigureStreetOfYourCity(this IServiceCollection serviceCollection)
    {
        serviceCollection.ConfigureFactories();
        serviceCollection.ConfigureServices();

        return serviceCollection;
    }

    private static void ConfigureFactories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IMainPageFactory, MainPageFactory>();
    }

    private static void ConfigureServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILocationDataServices, LocationDataServices>();
        serviceCollection.AddSingleton<IGameServices, GameServices>();
        serviceCollection.AddSingleton<IImageServices, ImageServices>();
    }
}

using StreetOfYourCity.Configurations;
using StreetOfYourCity.Pages.MainPage.Factories;
using StreetOfYourCity.Pages.MainPage.Views;

namespace StreetOfYourCity;

public partial class App : Application
{
    private static ServiceProvider ServiceProvider { get; set; } = null!;

    public App()
    {
        InitializeComponent();

        ServiceProvider = new ServiceCollection()
            .ConfigureStreetOfYourCity()
            .BuildServiceProvider();

        IMainPageFactory mainPageFactory = ServiceProvider.GetRequiredService<IMainPageFactory>();
        MainPageView mainPageView = mainPageFactory.Create();
        Application.Current!.MainPage = mainPageView;
    }
}

using StreetOfYourCity.Pages.MainPage.Commands;
using StreetOfYourCity.Pages.MainPage.ViewModels;
using StreetOfYourCity.Pages.MainPage.Views;

namespace StreetOfYourCity.Pages.MainPage.Factories;

internal class MainPageFactory : IMainPageFactory
{

    public MainPageView Create()
    {
        MainPageView mainPageView = new();
        MainPageViewModel mainPageViewModel = new();

        mainPageViewModel.EndCommand = new EndCommand(mainPageViewModel);

        SetFields(mainPageViewModel);

        mainPageView.BindingContext = mainPageViewModel;
        return mainPageView;
    }

    //temporary method to set fields
    private static void SetFields(MainPageViewModel mainPageViewModel)
    {
        mainPageViewModel.Name = "Das Programm, StreetOfYourCity, hat das Licht der Welt erblickt.";
    }
}

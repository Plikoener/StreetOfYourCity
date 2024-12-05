using StreetOfYourCity.Core;
using StreetOfYourCity.Pages.MainPage.ViewModels;

namespace StreetOfYourCity.Pages.MainPage.Commands;

internal class EndCommand : ContextualCommand<MainPageViewModel>
{
    public EndCommand(MainPageViewModel mainPageViewModel) : base(mainPageViewModel)
    {
    }

    public override bool CanExecute(object? parameter)
    {
        return true;
    }

    public override void Execute(object? parameter)
    {
        Application.Current!.Quit();
    }
}

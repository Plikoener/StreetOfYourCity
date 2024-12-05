using ReactiveUI;
using System.Windows.Input;

namespace StreetOfYourCity.Pages.MainPage.ViewModels;

internal class MainPageViewModel : ReactiveObject
{
    public string? Name { get; set; }

    public ICommand? EndCommand { get; set; }
}

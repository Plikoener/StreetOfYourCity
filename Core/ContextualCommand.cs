using System.Windows.Input;
using ReactiveUI;

namespace StreetOfYourCity.Core;

internal abstract class ContextualCommand<TContext> : ICommand where TContext : ReactiveObject
{
    protected readonly TContext ViewModel;

    public ContextualCommand(TContext viewModel)
    {
        ViewModel = viewModel;
    }

    public abstract bool CanExecute(object? parameter);

    public abstract void Execute(object? parameter);

    public event EventHandler? CanExecuteChanged;
}

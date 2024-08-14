using System.Windows.Input;

namespace LiveSortingIssues.Components.Shared;

public sealed class DelegateCommand : ICommand
{
    private static readonly Func<object?, bool> CachedAlwaysCanExecute = _ => true;

    private readonly Func<object?, bool> _canExecute;
    private readonly Action<object?> _execute;

    public event EventHandler? CanExecuteChanged;


    public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute ?? CachedAlwaysCanExecute;
    }

    public bool CanExecute(object? parameter) => _canExecute(parameter);

    public void Execute(object? parameter) => _execute(parameter);

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

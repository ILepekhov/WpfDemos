using System.Windows;

namespace LiveSortingIssues;

public partial class App
{
    private readonly Bootstrapper _bootstrapper = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        ShowRootWindow();
        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Dispose();
        base.OnExit(e);
    }

    private void ShowRootWindow()
    {
        var window = _bootstrapper.Resolve<RootWindow>();

        Current.MainWindow = window;

        window.Show();
    }

    private void Dispose()
    {
        _bootstrapper.Dispose();
    }
}

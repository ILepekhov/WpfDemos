using LiveSortingIssues.Components.Root;
using LiveSortingIssues.DataModel;
using Microsoft.Extensions.DependencyInjection;

namespace LiveSortingIssues;

public sealed class Bootstrapper : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    private bool _disposed;

    public Bootstrapper()
    {
        ServiceCollection services = new();

        services
            .AddSingleton(new Drum(DrumParameters.ColumnCount, DrumParameters.RowCount))
            .AddRootComponent();

        _serviceProvider = services.BuildServiceProvider();
    }

    public TService Resolve<TService>() where TService : notnull
    {
        ObjectDisposedException.ThrowIf(_disposed, typeof(Bootstrapper));

        return _serviceProvider.GetRequiredService<TService>();
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _serviceProvider.Dispose();

        _disposed = true;
    }
}

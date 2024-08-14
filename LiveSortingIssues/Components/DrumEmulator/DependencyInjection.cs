using Microsoft.Extensions.DependencyInjection;

namespace LiveSortingIssues.Components.DrumEmulator;

public static class DependencyInjection
{
    public static IServiceCollection AddDrumEmulatorComponent(this IServiceCollection services)
    {
        services.AddSingleton<DrumEmulatorViewModel>();
        services.AddSingleton<DrumEmulatorView>(resolver => new DrumEmulatorView
        {
            DataContext = resolver.GetService<DrumEmulatorViewModel>()
        });

        return services;
    }
}

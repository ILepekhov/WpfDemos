using Microsoft.Extensions.DependencyInjection;

namespace LiveSortingIssues.Components.Root;

public static class DependencyInjection
{
    public static IServiceCollection AddRootComponent(this IServiceCollection services)
    {
        services.AddSingleton<RootViewModel>();
        services.AddSingleton<RootWindow>(resolver => new RootWindow
        {
            ViewModel = resolver.GetService<RootViewModel>()
        });

        return services;
    }
}

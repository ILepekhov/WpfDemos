using Microsoft.Extensions.DependencyInjection;

namespace LiveSortingIssues.Components.Slides;

public static class DependencyInjection
{
    public static IServiceCollection AddSlidesComponent(this IServiceCollection services)
    {
        services.AddSingleton<SlidesViewModel>();
        services.AddSingleton<SlidesView>(resolver => new SlidesView
        {
            DataContext = resolver.GetService<SlidesViewModel>()
        });

        return services;
    }
}

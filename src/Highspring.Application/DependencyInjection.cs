using Microsoft.Extensions.DependencyInjection;

namespace Highspring.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using DagoShopFlow.Application.Services;
using DagoShopFlow.Application.Abstractions.Services;

namespace DagoShopFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IStorefrontService, StorefrontService>();
        return services;
    }
}

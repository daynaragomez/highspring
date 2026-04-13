using Highspring.Application.Abstractions.Services;
using Highspring.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DagoShopFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<ITaxCalculator, TaxCalculator>();
        services.AddScoped<ICheckoutService, CheckoutService>();
        services.AddScoped<IStorefrontService, StorefrontService>();

        return services;
    }
}

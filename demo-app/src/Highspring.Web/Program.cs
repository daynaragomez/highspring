using DagoShopFlow.Application;
using DagoShopFlow.Application.Abstractions.Services;
using DagoShopFlow.Infrastructure;
using DagoShopFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

for (var attempt = 1; attempt <= 10; attempt++)
{
    try
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();

        if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
        {
            var hasProducts = await dbContext.Products.AnyAsync();
            if (!hasProducts)
            {
                var testControlService = scope.ServiceProvider.GetRequiredService<ITestControlService>();
                await testControlService.ResetToBaselineAsync(CancellationToken.None);
            }
        }

        break;
    }
    catch when (attempt < 10)
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

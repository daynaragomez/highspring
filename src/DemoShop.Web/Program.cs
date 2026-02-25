using DemoShop.Web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.WebHost.UseUrls("http://0.0.0.0:8080");
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapRazorPages();
app.MapGet("/health", () =>
{
    var configured = !string.IsNullOrWhiteSpace(app.Configuration.GetConnectionString("Default"));
    return Results.Ok(new { status = "Healthy", connectionStringConfigured = configured });
});

app.Run();

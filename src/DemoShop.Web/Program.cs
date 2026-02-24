var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.WebHost.UseUrls("http://0.0.0.0:8080");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapGet("/health", () =>
{
    var configured = !string.IsNullOrWhiteSpace(app.Configuration.GetConnectionString("Default"));
    return Results.Ok(new { status = "Healthy", connectionStringConfigured = configured });
});

app.Run();

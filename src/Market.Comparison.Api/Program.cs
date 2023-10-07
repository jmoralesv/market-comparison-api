using Market.Comparison.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

// Register the endpoints
app.MapWeatherForecastEndpoints();

await app.RunAsync();

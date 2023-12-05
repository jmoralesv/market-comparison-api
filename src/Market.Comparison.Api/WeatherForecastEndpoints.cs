namespace Market.Comparison.Api;

internal static class WeatherForecastEndpoints
{
    internal static WebApplication MapWeatherForecastEndpoints(this WebApplication app)
    {
        var groupBuilder = app.MapGroup("weather-forecast");
        groupBuilder.MapGetWeatherForecastEndpoint();
        return app;
    }

    private static RouteGroupBuilder MapGetWeatherForecastEndpoint(this RouteGroupBuilder groupBuilder)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        groupBuilder.MapGet("/", (HttpContext context) =>
        {
            var user = context.User;

            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)],
                    user.Claims.Select(c => new Claim(c.Type, c.Value)).ToList()
                ))
                .ToArray();
            return forecast;
        })
        .RequireAuthorization("ApiScope")
        .WithName("GetWeatherForecast")
        .WithOpenApi()
        .Produces<WeatherForecast[]>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized);

        return groupBuilder;
    }
}

internal sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary, List<Claim> claims)
{
    internal int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal sealed record Claim(string ClaimType, string ClaimValue);
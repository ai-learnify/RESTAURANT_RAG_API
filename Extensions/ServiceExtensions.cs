using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using RestaurantApi.Tools;
using RestaurantApi.Data;
using Google.GenAI;

namespace RestaurantApi.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddProblemDetails();
        services.AddControllers();
        services.AddOpenApi();

        services.AddScoped<MenuTools>();
        services.AddScoped<TableTools>();

        services.AddDbContext<DataContext>(options => options
            .UseNpgsql(config.GetConnectionString("Default"))
            .UseSeeding((context, _) => DataSeed.SeedBookings(context)));

        services.AddChatClient(new Client(apiKey: config["Gemini:ApiKey"]!).AsIChatClient(config["Gemini:LiteModel"]))
            .UseFunctionInvocation();

        return services;
    }
}
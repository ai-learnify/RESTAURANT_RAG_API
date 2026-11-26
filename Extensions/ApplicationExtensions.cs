namespace RestaurantApi.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication UseApplication(this WebApplication app)
    {
        app.UseExceptionHandler();

        app.MapOpenApi();
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));

        app.MapControllers();

        return app;
    }
}
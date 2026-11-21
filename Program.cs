using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<DataContext>(options => options
    .UseNpgsql(builder.Configuration.GetConnectionString("Default"))
    .UseSeeding((context, _) => DataSeed.SeedBookings(context)));

var app = builder.Build();

app.UseExceptionHandler();

app.MapOpenApi();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
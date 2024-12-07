using PerformanceTest.API;
using PerformanceTesting.Infrastructure;
using PerformanceTesting.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Test Performance API")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/");

using (var scope = app.Services.CreateScope())
{
    AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    ILoggerFactory loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
    AppDbContextInitializer contextInitializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();

    await contextInitializer.InitialiseAsync();
    await contextInitializer.SeedAsync();
}



app.Run();

using DataSourceExample;

var builder = WebApplication.CreateBuilder(args);

// Register a NpgsqlDataSourceBuilder singleton to provide NpgsqlDataSource instances.
builder.Services.AddSingleton(serviceProvider =>
    new NpgsqlDataSourceBuilder(
        serviceProvider
            .GetRequiredService<IConfiguration>()
            .GetConnectionString("Default")));

// Register transient NpgsqlDataSource instances from the NpgsqlDataSourceBuilder.
builder.Services.AddTransient(
    serviceProvider => serviceProvider.GetRequiredService<NpgsqlDataSourceBuilder>().Build());

// Use Npgsql provider for EF.
// The first ExampleContext will get a NpgsqlDataSource from DI but subsequent instances wont.
builder.Services.AddDbContext<ExampleContext>(builder =>
{
    builder.UseNpgsql();
});

var app = builder.Build();

app.MapGet("/test", async (ExampleContext context) =>
{
    try
    {
        await context.Database.ExecuteSqlRawAsync("SELECT 1");
        return "passed";
    }
    catch (Exception ex)
    {
        return $"failed: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}";
    }
});

app.Run();

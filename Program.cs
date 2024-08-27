using MySqlConnector;

public class CSPMiddleware
{
    private readonly string _cspPolicy;

    public CSPMiddleware(string cspPolicy)
    {
        _cspPolicy = cspPolicy;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Response.Headers.Append("Content-Security-Policy", _cspPolicy);
        await next(context);
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<CSPMiddleware>();
        services.AddSingleton<string>("default-src 'self';");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // ... other middleware configurations

        app.UseMiddleware<CSPMiddleware>();
        // Example middleware usage

        // ... other middleware configurations
    }
}

internal class Program
{

    public void ConfigureServices(IServiceCollection services)
    {
        string cspPolicy = "default-src 'self';";
        services.AddScoped<CSPMiddleware>(serviceProvider => new CSPMiddleware(cspPolicy));
    }

    private static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();

            }).Build();
        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Host created and logging enabled.");

        var builder = WebApplication.CreateBuilder(args);

        // Logging support
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();

        // CORS support
        var customOrigin = "mainOrigin";
        var localOrigin = "localOrigin";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: customOrigin,
                              policy =>
                              {
                                  policy.WithOrigins("https://www.nehsa.net").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                              });

            options.AddPolicy(name: localOrigin,
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                              });
        });

        // middleware services
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks();
        builder.Services.AddControllers();
        builder.Services.AddLogging();

        // MySQL support
        builder.Services.AddMySqlDataSource(builder.Configuration.GetConnectionString("Default")!);

        // Setup final app to run
        var app = builder.Build();

        // set to use CORS
        app.UseCors(customOrigin);
        app.UseCors(localOrigin);

        logger.LogInformation("app.Environment.IsDevelopment(): " + app.Environment.IsDevelopment());
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHttpsRedirection();
            app.UseExceptionHandler("/Error");
        }
        app.UseRouting();
        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHealthChecks("/health");
        app.UseStaticFiles(); // allow us to serve map images

        // start app
        logger.LogInformation("Starting Application!");
        app.Run();
    }
}


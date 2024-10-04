using MySqlConnector;
using Serilog;

namespace WebApp
{
    public class AngularMiddleware
    {
        private readonly RequestDelegate _next;

        public AngularMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine($"AngularMiddleware Invoke: {context.Request.Path}, IP: {context.Connection.RemoteIpAddress}");
            if (context.Request.Path.StartsWithSegments("/v1"))
            {
                // Forward API requests to Kestrel
                Console.WriteLine("Directing to Kestrel");
                await _next(context);
            }
            else
            {
                // Redirect all other requests to Angular's index.html
                Console.WriteLine("Directing to Angular SPA");
                context.Response.StatusCode = StatusCodes.Status302Found;
                context.Response.Headers.Location = "/";
            }
        }
    }

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

    internal class Program
    {
        private static void Main(string[] args)
        {
            WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();
            webApplicationBuilder.Services.AddHealthChecks();
            webApplicationBuilder.Services.AddControllers();
            webApplicationBuilder.Services.AddLogging();

            // CORS support
            webApplicationBuilder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(
                        policy =>
                        {
                            policy.WithOrigins("http://nehsa.net",
                                                "http://www.nehsa.net",
                                                "https://nehsa.net",
                                                "https://www.nehsa.net",
                                                "http://localhost:4200",
                                                "https://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                        });
                });

            // Logging support
            webApplicationBuilder.Logging.ClearProviders();
            webApplicationBuilder.Logging.AddConsole();
            webApplicationBuilder.Logging.AddDebug();

            // MySQL support
            webApplicationBuilder.Services.AddMySqlDataSource(webApplicationBuilder.Configuration.GetConnectionString("Default")!);
            var app = webApplicationBuilder.Build();

            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Logging setup");

            // app.UseHttpsRedirection(); // redirect to https
            app.UseExceptionHandler("/Error"); // handle exceptions
            app.UseSwagger(); // setup swagger, this is different than UseSWaggerUI in that it just sets up the middleware
            app.UseSwaggerUI(); // setup swagger UI, this is the UI that is used to view the API
            app.UseHealthChecks("/health"); // setup health checks using the default health check middleware
            app.UseRouting(); // This configues the routing middleware
            app.UseStaticFiles(); // allow us to serve map images
            app.MapControllers(); // This maps the controllers to the routing middleware. e.g. without this, the controllers will not be called

            // set to use CORS
            logger.LogInformation("Setting up CORS for API");
            app.UseCors();

            // start app
            logger.LogInformation("Starting Application!");
            app.Run();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using nehsanet_app.db;
using nehsanet_app.Services;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using static nehsanet_app.Services.IUserSession;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

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
                context.Response.StatusCode = StatusCodes.Status200OK;
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

            // Logging support 
            webApplicationBuilder.Logging.ClearProviders();
            webApplicationBuilder.Logging.AddConsole();
            webApplicationBuilder.Logging.AddDebug();

            // Add HTTP context accessor
            webApplicationBuilder.Services.AddHttpContextAccessor();

            // Add user session provider
            webApplicationBuilder.Services.AddScoped<IUserSessionProvider, UserSessionProvider>();

            // Add MySQL support
            webApplicationBuilder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseMySQL(webApplicationBuilder.Configuration.GetConnectionString("Default")!);
            });

            // Add authentication support
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "NehsaAPI",
                ValidAudience = "nehsa",
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            Environment.GetEnvironmentVariable("NEHSA_JWT_SECURITY_KEY") ?? ""))
            };

            webApplicationBuilder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async ctx =>
                        {
                            var userSession = ctx.HttpContext.RequestServices.GetRequiredService<IUserSessionProvider>();
                            await userSession.SetSession(ctx.Principal!);
                        }
                    };
                });

            // Add authorization support
            webApplicationBuilder.Services
                .AddAuthorizationBuilder()
                .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
                .AddPolicy("New", policy => policy.RequireRole("New"))
                .AddPolicy("User", policy => policy.RequireRole("User"));

            // CORS support
            webApplicationBuilder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.WithOrigins("http://nehsa.net",
                                            "http://www.nehsa.net",
                                            "https://mud.nehsa.net",
                                            "https://nehsa.net",
                                            "https://www.nehsa.net",
                                            "http://localhost:4200",
                                            "https://localhost:4200",
                                            "http://localhost:5173")
                                .AllowAnyMethod() // without this, only GET and POST are allowed.
                                .AllowAnyHeader() // without this, only the default headers are allowed.
                                .AllowCredentials();
                    });
            });

            // Add OpenTelemetry support
            const string serviceName = "nehsanet_app";
            webApplicationBuilder.Logging.AddOpenTelemetry(_ =>
            {
                _
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(serviceName))
                    .AddConsoleExporter();

                _.IncludeScopes = true; // include scope information
                _.IncludeFormattedMessage = true;
                _.AddOtlpExporter(exporter =>
                    {
                        exporter.Endpoint = new Uri("http://util.nehsa.net:5341/ingest/otlp/v1/logs");
                        exporter.Protocol = OtlpExportProtocol.HttpProtobuf;
                        exporter.Headers = $"X-Seq-ApiKey={nehsanet_app.Secrets.Seq.ApiKey}";
                    });
            });

            webApplicationBuilder.Services.AddOpenTelemetry()
                .ConfigureResource(
                    resource =>
                    {
                        resource.AddService(serviceName);
                        resource.AddAttributes(new Dictionary<string, object>()
                        {
                            ["deployment.environment"] = webApplicationBuilder.Environment.EnvironmentName,
                            ["version"] = DateTime.Now.DayOfYear.ToString(),
                            ["machine.hostname"] = Environment.MachineName
                        });
                    })
                  .WithTracing(tracing => tracing
                      .AddAspNetCoreInstrumentation()
                      .AddConsoleExporter())
                  .WithMetrics(metrics => metrics
                      .AddAspNetCoreInstrumentation()
                      .AddConsoleExporter());

            var app = webApplicationBuilder.Build();

            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Logging setup");

            // app.UseHttpsRedirection(); // redirect to https
            app.UseExceptionHandler("/Error"); // handle exceptions
            app.UseSwagger(); // setup swagger, this is different than UseSWaggerUI in that it just sets up the middleware
            app.UseSwaggerUI(); // setup swagger UI, this is the UI that is used to view the API
            app.UseRouting(); // This configures the routing middleware
            app.UseStaticFiles(); // allow us to serve map images
            app.MapControllers(); // This maps the controllers to the routing middleware. e.g. without this, the controllers will not be called
            app.MapHealthChecks("/v1/health"); // This maps the health checks to the routing middleware

            // set to use CORS
            logger.LogInformation("Setting up CORS for API");
            app.UseCors();

            // start app
            logger.LogInformation("Starting Application!");

            // launch with healthcheck page
            app.Run();
        }
    }
}

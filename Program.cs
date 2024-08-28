using System.Diagnostics;
using MySqlConnector;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace WebApp
{

    public class CSPMiddleware // Make the constructor public
    {
        private readonly string _cspPolicy;

        public CSPMiddleware(string cspPolicy) // public constructor
        {
            _cspPolicy = cspPolicy;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Access the cspPolicy parameter here
            context.Response.Headers.Append("Content-Security-Policy", _cspPolicy);
            await next(context);
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var customOrigin = "mainOrigin";
            var localOrigin = "localOrigin";
            WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();
            webApplicationBuilder.Services.AddHealthChecks();
            webApplicationBuilder.Services.AddControllers();
            webApplicationBuilder.Services.AddLogging();

            // CORS support
            webApplicationBuilder.Services.AddCors(options =>
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

            // Logging support
            webApplicationBuilder.Logging.ClearProviders();
            webApplicationBuilder.Logging.AddConsole();
            webApplicationBuilder.Logging.AddDebug();

            // MySQL support
            webApplicationBuilder.Services.AddMySqlDataSource(webApplicationBuilder.Configuration.GetConnectionString("Default")!);
            var app = webApplicationBuilder.Build();

            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Logging setup");

            // set to use CORS
            logger.LogInformation("Setting up CORS");
            app.UseCors(customOrigin);
            app.UseCors(localOrigin);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHealthChecks("/health");
            app.UseStaticFiles(); // allow us to serve map images

            logger.LogInformation("Host created and logging enabled.");
            app.MapControllers();

            // start app
            logger.LogInformation("Starting Application!");
            app.Run();
        }
    }
}


using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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
                                  policy.WithOrigins("http://localhost:4200");
                              });
        });

        // Add services to the container.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks();
        builder.Services.AddControllers();
        
        // Setup final app to run
        var app = builder.Build();
        app.UseCors(customOrigin);
        app.UseCors(localOrigin);

        Trace.WriteLine("app.Environment.IsDevelopment(): " + app.Environment.IsDevelopment());
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

        // start app
        app.Run();
    }
}


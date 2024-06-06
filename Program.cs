

using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;


var builder = WebApplication.CreateBuilder(args);
var logger = LoggerFactory
    .Create(loggingBuilder =>
    {
        // Copy all the current providers that was set within WebApplicationBuilder
        foreach (var serviceDescriptor in builder.Logging.Services)
            {
            loggingBuilder.Services
                .Add(serviceDescriptor);
        }
    })
    .CreateLogger<Program>();
;

// Add services to the container.
logger.LogInformation("Add services to the container...");
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
var  customOrigins = "_customOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: customOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("https://www.nehsa.net").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                      });

    // options.AddPolicy(name: customOrigins,
    //                   policy  =>
    //                   {
    //                       policy.WithOrigins("http://localhost:4200");
    //                   });     
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(customOrigins);
app.UseHttpsRedirection();
app.UseSwagger();
app.Services.SaveSwaggerJson();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

var quotes = new[]
{
    "“You live and learn. At any rate, you live.” - Douglas Adams", 
    "“A learning experience is one of those things that says, 'You know that thing you just did? Don't do that.’” - Douglas Adams", 
    "“I may not have gone where I intended to go, but I think I have ended up where I needed to be.” - Douglas Adams", 
    "“The quality of any advice anybody has to offer has to be judged against the quality of life they actually lead.” - Douglas Adams", 
    "“I refuse to answer that question on the grounds that I don't know the answer.” - Douglas Adams", 
    "“I love deadlines. I love the whooshing noise they make as they go by.” - Douglas Adams", 
    "“Anything that thinks logically can be fooled by something else that thinks at least as logically as it does.” - Douglas Adams", 
    "“Time is an illusion. Lunchtime doubly so” - Douglas Adams", 
    "“Life is wasted on the living.” - Douglas Adams", 
    "“Don't Panic.” - Douglas Adams", 
    "“Don't believe anything you read on the net. Except this. Well, including this, I suppose.” - Douglas Adams", 
    "“The impossible often has a kind of integrity to it which the merely improbable lacks.” - Douglas Adams", 
    "“When the elevator tries to bring you down, go crazy!” - Prince"
};

app.MapGet("/api/v1/da", [SwaggerOperation(
        Summary = "Returns a single quote",
        Description = "A random Douglas Adam quote is returned.  Because.")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(500, "An error occurred")] () =>
{

    return JsonSerializer.Serialize<string>(quotes[Random.Shared.Next(quotes.Length)]);
})
.WithName("DouglasAdamQuotes")
.WithOpenApi();

app.MapGet("/api/v1/swagger", [SwaggerOperation(
        Summary = "Returns documentation",
        Description = "Returns Swagger documentation is raw JSON form")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(500, "An error occurred")] () =>
{
    string filename = "swagger.json";
    string results = "No Swagger information found.";

    if (File.Exists(filename))
        results = JsonSerializer.Serialize<string>(File.ReadAllText(filename));

    return results;
})
.WithName("Swagger Documentation")
.WithOpenApi();

app.MapPost("/api/v1/adduser", ([FromBody] string jsonstring) =>
{
    return JsonSerializer.Serialize<string>("User added: " + jsonstring);
});

app.Run();

public static class SwaggerExtensions
{
    public static void SaveSwaggerJson(this IServiceProvider provider)
    {
        ISwaggerProvider sw = provider.GetRequiredService<ISwaggerProvider>();
        OpenApiDocument doc = sw.GetSwagger("v1", null, "/");
        string swaggerFile = doc.SerializeAsJson(Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0);
        File.WriteAllText("swagger.json", swaggerFile);
    }
}

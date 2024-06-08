

using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;


var builder = WebApplication.CreateBuilder(args);

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
app.UseSwaggerUI();

var names = new[]
{
    "Douglas",
    "Bob",
    "Kvothe",
    "Denna",
    "Bast",
    "Cinder",
    "Ambrose",
    "Simmon",
    "Wilem",
    "Manet",
    "Elodin",
    "Auri",
    "Devi",
    "Fela",
    "Mola",
    "Hemme",
    "Lorren",
    "Kilvin",
    "Herma",
    "Kilvin",
    "Erik",
    "Ulysses",
    "Dresden",
    "Harry",
    "Michael",
    "Thomas",
    "Murphy",
    "Butters",
    "Marcone",
    "Mab",
    "Lea",
    "Lily",
    "Sarissa",
    "Molly",
    "Sonya",
    "Walther Borden",
    "Marcone"
};

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

app.MapGet("/v1/name", [SwaggerOperation(
        Summary = "Returns a random name",
        Description = "A random name is returned.")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(500, "An error occurred")] () =>
{

    return JsonSerializer.Serialize<string>(names[Random.Shared.Next(names.Length)]);
})
.WithName("RandomNames")
.WithOpenApi();

app.MapGet("/v1/da", [SwaggerOperation(
        Summary = "Returns a single quote",
        Description = "A random quote is returned.  Because.")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(500, "An error occurred")] () =>
{

    return JsonSerializer.Serialize<string>(quotes[Random.Shared.Next(quotes.Length)]);
})
.WithName("Quotes")
.WithOpenApi();

app.MapGet("/v1/swagger", [SwaggerOperation(
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

// start app
app.Run();

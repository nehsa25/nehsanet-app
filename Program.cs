

using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var  customOrigins = "_customOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: customOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("https://www.nehsa.net",
                                              "http://localhost:4200");
                      });
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(customOrigins);

app.UseHttpsRedirection();

var douglasAdamQuotes = new[]
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
};

app.MapGet("/api/v1/da", () =>
{
    return JsonSerializer.Serialize<string>(douglasAdamQuotes[Random.Shared.Next(douglasAdamQuotes.Length)]);
})
.WithName("DouglasAdamQuotes")
.WithOpenApi();

app.Run();


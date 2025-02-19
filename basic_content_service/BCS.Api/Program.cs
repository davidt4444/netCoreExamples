using Microsoft.OpenApi.Models;
using MySql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.IO;
using BCS.Api.Data;
using BCS.Api.Services;
// using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                               .AllowCredentials()
                                .AllowAnyMethod();
                      });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add services to the container.
builder.Services.AddControllers();
// Add DbContext
StreamReader sr = new StreamReader("../../../aws-resources/localhost-mac-dotnet.txt");
String connect = sr.ReadToEnd();
builder.Services.AddDbContext<YourDbContext>(options =>
    // options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
//    options.UseMySQL("server=localhost;database=library;user=user;password=password")
   options.UseMySQL(connect)
    );
// Add PostService
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotNet Basic Content Service", Version = "v1" });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNet Basic Content Service v1"));
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.Run();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");

// app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }

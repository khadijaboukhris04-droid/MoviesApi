using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using MoviesApi.Services;
using System.Globalization;

// Culture invariante: bech el Rate yetقbel b point (8.8) badel virgule
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Services repository
builder.Services.AddTransient<IGenresService, GenresService>();
builder.Services.AddTransient<IMoviesService, MoviesService>();

// AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

// CORS
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "MoviesApi v1");
    });
}

app.UseHttpsRedirection();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
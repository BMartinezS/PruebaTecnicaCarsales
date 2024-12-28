using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PruebaTecnicaCarsales.Core.Interfaces;
using PruebaTecnicaCarsales.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(); // Añadimos soporte para controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS para permitir las peticiones desde Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // URL de tu app Angular
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Registrar HttpClient para consumir la API de Rick and Morty
builder.Services.AddHttpClient();

// Registrar nuestros servicios
builder.Services.AddScoped<IRickAndMortyService, RickAndMortyService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowAngular");

app.UseAuthorization();

// Mapear los controllers
app.MapControllers();

app.Run();
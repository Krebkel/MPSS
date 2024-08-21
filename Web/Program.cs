using System.Text.Json.Serialization;
using Data;
using Employees;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Products;
using Projects;

var builder = WebApplication.CreateBuilder(args);

// Добавление контроллеров
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });;

// Добавление Swagger для документации API
builder.Services.AddEndpointsApiExplorer()
                .AddSwaggerGen();

// Конфигурация параметров базы данных
builder.Services.Configure<DataOptions>(builder.Configuration.GetSection("Postgres"));

// Добавление сервисов для работы с базой данных
builder.Services.AddPostgresData()
    .AddPostgresProducts()
    .AddPostgresEmployees()
    .AddPostgresProjects();

// Добавление проверки здоровья приложения
builder.Services.AddHealthChecks();

var app = builder.Build();

// Настройка маршрутизации
app.UseRouting();

// Настройка конечных точек маршрутизации
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/health");
});

// Подключение Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = "swagger"; 
});

// Настройка статических файлов
app.UseDefaultFiles()
   .UseStaticFiles(new StaticFileOptions
   {
       OnPrepareResponse = context =>
       {
           var headers = context.Context.Response.GetTypedHeaders();
           headers.CacheControl = new CacheControlHeaderValue
           {
               Public = true,
               MaxAge = TimeSpan.FromMinutes(1)
           };
       }
   }); 

app.Run();
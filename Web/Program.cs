using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Users;
using Products;
using Projects;
using Data;
using Employees;
using Web.Extensions;
using Web.Options;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigings = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!;
builder.Services
    .AddCors(options => options
        .AddPolicy("CorsPolicy", policyBuilder => policyBuilder
            .WithOrigins(allowedOrigings)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()))
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(setup =>
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Вставь сюда JWT-токен",
        
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };
        
        setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
        
        setup.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { jwtSecurityScheme, Array.Empty<string>() }
        });
    });

builder.Services
    .Configure<DataOptions>(builder.Configuration.GetSection("Postgres"))
    .Configure<JwtTokenOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddPostgresData()
    .AddUsers()
    .AddPostgresProducts()
    .AddPostgresEmployees()
    .AddPostgresProjects()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/health");
});

// if (app.Environment.IsDevelopment()) 
app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = "swagger"; 
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/healthcheck");
app.MapControllers();
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

app.UseAuthorizationErrorHandling();

app.Run();
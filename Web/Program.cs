using System.Text;
using System.Text.Json.Serialization;
using Data;
using Employees;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Products;
using Projects;
using Users;
using Web.Options;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigings = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

IdentityModelEventSource.ShowPII = true;

builder.Services.AddAuthorization();
builder.Services.AddHealthChecks();

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
        setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        setup.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference 
                    { 
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer" 
                    }
                },
                new string[] {}
            }
        });
    });

builder.Services
    .Configure<DataOptions>(builder.Configuration.GetSection("Postgres"))
    .Configure<JwtTokenOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddPostgresData()
    .AddPostgresProducts()
    .AddPostgresEmployees()
    .AddPostgresProjects()
    .AddUsers();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/health");
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = "swagger"; 
});

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
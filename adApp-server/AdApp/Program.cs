using AdApp.Core.Handlers;
using AdApp.Core.Helpers;
using AdApp.Core.Handlers.JWT;
using AdApp.Core.Services.Ads;
using AdApp.Core.Services.Auth;

using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Runtime.Serialization.Json;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(static fv => fv.RegisterValidatorsFromAssemblyContaining<UserValidator>());
// Add FluentValidation services
// Register FluentValidation



builder.Services.AddValidatorsFromAssemblyContaining<Program>();  // This registers all validators in the same assembly

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.IsEssential = true;
});
// Add Swagger and configure JWT Bearer Authorization
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdApp API", Version = "v1" });

    // Custom schema ID resolver to avoid conflicts between entities with the same name
    c.CustomSchemaIds(type => type.FullName); // Use full name (namespace + class name) as schema ID

    // Add JWT Authentication to Swagger (your existing setup)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT token with Bearer prefix. Example: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// DI
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdService, AdService>();
builder.Services.AddScoped<IJWT_Handler, JWT_Handler>();


// Add JWT Bearer authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

     //JWT Bearer Allows secured authentication and authorization from client
     .AddJwtBearer(options =>
     {
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = builder.Configuration["Jwt:Issuer"],
             ValidAudience = builder.Configuration["Jwt:Audience"],
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))//using in code key for security
         };

         //options.Events = new JwtBearerEvents
         //{
         //    OnMessageReceived = context =>
         //    {
         //        // Read the access token from the query string when connecting to SignalR hub
         //        var accessToken = context.Request.Query["access_token"];

                

         //        return Task.CompletedTask;
         //    }
         //};
         options.Events = new JwtBearerEvents
         {
             OnMessageReceived = context =>
             {
                 var jwt = context.Request.Cookies["jwt"];
                 if (!string.IsNullOrEmpty(jwt))
                 {
                     context.Token = jwt;
                 }
                 return Task.CompletedTask;
             }
         };
     });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); 
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<AdApp.Core.Helpers.AuthorizationMiddleware>();
app.UseRouting();
app.UseCors("AllowAll");
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax, 
    Secure = CookieSecurePolicy.None 
});
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.Run();

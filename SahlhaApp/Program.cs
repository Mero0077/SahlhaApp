using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SahlhaApp.DataAccess.Data;
using SahlhaApp.DataAccess.Repositories;
using SahlhaApp.DataAccess.Repositories.IRepositories;
using SahlhaApp.Models.DTOs;
using SahlhaApp.Models.Models;
using SahlhaApp.Utility;
using SahlhaApp.Utility.NotifcationService;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT Configuration
var jwtoptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
builder.Services.AddSingleton(jwtoptions);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = jwtoptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtoptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoptions.SigninKey)),
        NameClaimType = "nameid"
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context => //This is triggered for every incoming HTTP request where JWT authentication is being used.
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/jobHub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});


//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//});

// Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();




// Register services for JobService and JobPostedNotificationHandler
builder.Services.AddScoped<JobService>();
builder.Services.AddScoped<JobPostedNotificationHandler>();

// Add HTTP Client for external services
builder.Services.AddHttpClient();

// CORS Configuration (for cross-origin requests)
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", builder =>
//        builder.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader()).AllowCredentials() 
//            .SetIsOriginAllowed(_ => true)); ;
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder
            .WithOrigins(
                "http://127.0.0.1:5500",
                "http://localhost:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Required for SignalR with JWT auth
    });
});



var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        try
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing OpenAPI: {ex.Message}");
        }
    }

    // Seed database
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            await DbInitializer.InitializeAsync(services);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding the database: {ex.Message}");
        }
    }


app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
    
    app.UseAuthentication();
    app.UseAuthorization();

app.MapControllers();
app.MapHub<JobHub>("/jobHub");
app.Run();

 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using User_API.APIKEYS;
using User_API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using User_API.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-Api-Key",
        Type = SecuritySchemeType.ApiKey,
        Description = "Gib deinen API-Key hier ein",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
});

// User secrets
builder.Configuration.AddUserSecrets<Program>();

// JWT Settings
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new ArgumentNullException("Jwt:Key is missing"));

// Database connection (PostgreSQL)
builder.Services.AddDbContext<ApplicatonDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AuthService & API-Key Service
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<ApiKey>();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Authorization
builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyWebsite", policy =>
    {
        policy.WithOrigins("https://hannibunn.github.io")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS & Routing
app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowMyWebsite");
// API-Key Middleware, außer Login/Register
app.UseWhen(context =>
    !context.Request.Path.StartsWithSegments("/api/Anmelden/login") &&
    !context.Request.Path.StartsWithSegments("/api/Anmelden/register"),
    appBuilder =>
    {
        appBuilder.UseMiddleware<ApiKey_Middelware>();
    });

// Auth & Authorization
app.UseAuthentication();
app.UseAuthorization();

// CORS
app.UseCors("AllowMyWebsite");

// Map Controllers
app.MapControllers();

app.Run();

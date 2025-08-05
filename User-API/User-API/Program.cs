using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using User_API.APIKEYS;
using User_API.Data;
using User_API;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// F�ge Benutzergeheimnisse hinzu
builder.Configuration.AddUserSecrets<Program>();

// Lese die JWT-Einstellungen aus der Konfiguration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new ArgumentNullException("Jwt:Key is missing"));

// Datenbank-Verbindungszeichenfolge
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// F�ge AuthService als Singleton hinzu
builder.Services.AddSingleton<AuthService>();

// connection for the database with posqre 
builder.Services.AddDbContext<ApplicatonDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Konfiguriere die JWT-Authentifizierung
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

// Registriere API-Key-Service als Singleton
builder.Services.AddSingleton<ApiKey>();

// F�ge Authorization und Controller hinzu
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// F�ge Swagger f�r die API-Dokumentation hinzu
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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Die API - Key Middleware kommt direkt nach UseRouting und vor Authentication und Authorization
app.UseMiddleware<ApiKey_Middelware>();   // API-Key Middleware zuerst


app.UseHttpsRedirection();

app.UseAuthorization();
// Weitere Middleware
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

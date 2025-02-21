using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Warehouse_API_Test.APIKEYS;
using Warehouse_API_Test.Data;
using Warehouse_API_Test;

var builder = WebApplication.CreateBuilder(args);

// Füge Benutzergeheimnisse hinzu
builder.Configuration.AddUserSecrets<Program>();

// Lese die JWT-Einstellungen aus der Konfiguration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new ArgumentNullException("Jwt:Key is missing"));

// Datenbank-Verbindungszeichenfolge
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Füge AuthService als Singleton hinzu
builder.Services.AddSingleton<AuthService>();

// Füge DbContext hinzu
builder.Services.AddDbContext<ApplicatonDbContext>(options =>
    options.UseSqlServer(connectionString)
);

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

// Füge Authorization und Controller hinzu
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Füge Swagger für die API-Dokumentation hinzu
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

// Benutze Swagger in der Entwicklungsumgebung
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Die API-Key Middleware kommt direkt nach UseRouting und vor Authentication und Authorization
app.UseMiddleware<ApiKey_Middelware>();   // API-Key Middleware zuerst

// Authentifizierung und Autorisierung aktivieren
app.UseAuthentication(); // Dann Authentication
app.UseAuthorization();  // Dann Authorization

// Weitere Middleware
app.UseHttpsRedirection();

// Konfiguriere Controller
app.MapControllers();

app.Run();

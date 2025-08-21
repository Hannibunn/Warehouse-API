using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sets_API;
using Sets_API.APIKEYS;
using Sets_API.Data;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


// Füge Benutzergeheimnisse hinzu
builder.Configuration.AddUserSecrets<Program>();
// Lese die JWT-Einstellungen aus der Konfiguration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new ArgumentNullException("Jwt:Key is missing"));


// Füge AuthService als Singleton hinzu
builder.Services.AddSingleton<AuthService>();


// connection for the database with posqre 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
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


// User secrets
builder.Configuration.AddUserSecrets<Program>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyWebsite", policy =>
    {
        policy.WithOrigins("https://hannibunn.github.io/Warehouse-API/")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Verwende die API-Key-Middleware
app.UseMiddleware<ApiKey_Middelware>();
app.UseHttpsRedirection();
app.UseRouting();
//// Verwende CORS
//app.UseCors("AllowMyWebsite");
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sets API V1");
//    c.RoutePrefix = string.Empty; // Swagger UI unter der Root-URL verfügbar machen
//});
//app.UseRouting();


// Verwende die Authentifizierungapp.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();


app.MapControllers();

app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Warehouse_API_Test.Data;

var builder = WebApplication.CreateBuilder(args);


// Token einstellungen 
var jwtSettings = builder.Configuration.GetSection("Jwt"); // lies was in Jwt ist 
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]); // hol was in jwt key und tuh es verschluesseln damit keiner den kez sehen kann

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

builder.Services.AddAuthorization();



// Add services to the container.
builder.Services.AddDbContext<ApplicatonDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

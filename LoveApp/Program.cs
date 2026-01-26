using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using LoveApp.Data; // your DbContext namespace

var builder = WebApplication.CreateBuilder(args);

// ------------------------
// Add services
// ------------------------

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Controllers
builder.Services.AddControllers();

// ------------------------
// Swagger (Dev Only)
// ------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ------------------------
// Database (Supabase Postgres)
// ------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ------------------------
// JWT Authentication
// ------------------------
var key = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    throw new Exception("JWT Key, Issuer, or Audience is missing in appsettings.json!");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

// ------------------------
// CORS for Angular frontend
// ------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularFrontend", policy =>
    {
        policy.WithOrigins(
            "https://lapp-5vko.onrender.com" // <-- your deployed Angular URL
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// ------------------------
// Cloudinary Settings
// ------------------------
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary")
);

var app = builder.Build();

// ------------------------
// Middleware pipeline
// ------------------------

// Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Always enable CORS
app.UseCors("AllowAngularFrontend");

// Use HTTPS redirection in production (optional)
app.UseHttpsRedirection();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run on PORT environment variable (Render sets this automatically)
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://*:{port}");

app.Run();

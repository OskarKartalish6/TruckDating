using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TruckDating.Backend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TruckDating.Backend.Services;
using TruckDating.Backend.Models.Entities;
using Npgsql;
using TruckDating.Backend.Models.Enums;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<
    IPasswordHasher<User>,
    PasswordHasher<User>
>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Database connection string is missing."
    );

// Configure PostgreSQL native ENUM types.
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

dataSourceBuilder.MapEnum<AccountStatus>("account_status");
dataSourceBuilder.MapEnum<ModerationActionType>("moderation_action_type");
dataSourceBuilder.MapEnum<PhotoStatus>("photo_status");
dataSourceBuilder.MapEnum<ReportReason>("report_reason");
dataSourceBuilder.MapEnum<ReportStatus>("report_status");

var dataSource = dataSourceBuilder.Build();

builder.Services.AddSingleton(dataSource);

// Configure Entity Framework Core.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        dataSource,
        npgsqlOptions =>
        {
            npgsqlOptions.UseNetTopologySuite();
        }
    )
);
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT key is missing.");

var signingKey = new SymmetricSecurityKey(
    Convert.FromBase64String(jwtKey)
);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine(
                    $"JWT ERROR: {context.Exception.Message}"
                );

                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtTokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

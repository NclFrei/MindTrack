using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MindTrack.Application.Mapper;
using MindTrack.Application.Service;
using MindTrack.Domain.DTOs.Request;
using MindTrack.Domain.Interfaces;
using MindTrack.Domain.Validator;
using MindTrack.Infrastructure.Configuration;
using MindTrack.Infrastructure.Data;
using MindTrack.Infrastructure.Repository;
using MindTrack.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// DATABASE
builder.Services.AddDbContext<MindTrackContext>(options =>
 options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// JWT SETTINGS
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWTSettings"));
var jwtSettings = builder.Configuration.GetSection("JWTSettings").Get<JwtSettings>();

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(MetaProfile).Assembly);
    cfg.AddMaps(typeof(UserProfile).Assembly);
    cfg.AddMaps(typeof(TarefaProfile).Assembly);
    cfg.AddMaps(typeof(HealthProfile).Assembly);
});

// Services + Repositories
builder.Services.AddScoped<MetaService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TarefaService>();

builder.Services.AddScoped<IJwtSettingsProvider, JwtSettingsProvider>();
builder.Services.AddScoped<IMetaRepository, MetaRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();

// Health (MVP)
builder.Services.AddScoped<StressService>();
builder.Services.AddScoped<HeartMetricService>();
builder.Services.AddScoped<IHeartMetricRepository, HeartMetricRepository>();
builder.Services.AddScoped<IStressRepository, StressRepository>();

// Validators
builder.Services.AddScoped<IValidator<MetaCreateRequest>, MetaCreateRequestValidator>();
builder.Services.AddScoped<IValidator<UserCreateRequest>, UserCreateRequestValidator>();
builder.Services.AddScoped<IValidator<TarefaCreateRequest>, TarefaCreateRequestValidator>();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
     .AllowAnyHeader()
     .AllowAnyMethod();
    });
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
    new UrlSegmentApiVersionReader(),
    new HeaderApiVersionReader("X-Api-Version")
    );
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Swagger com Versionamento
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MottuChallenge API", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "MottuChallenge API", Version = "v2" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Insira o token JWT válido abaixo",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
 {
 { jwtSecurityScheme, Array.Empty<string>() }
 });
});

builder.Services.AddControllers();

// Health Check
builder.Services.AddHealthChecks()
 .AddCheck("self", () => HealthCheckResult.Healthy())
 .AddDbContextCheck<MindTrackContext>("database");

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MottuChallenge API V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "MottuChallenge API V2");
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health").WithTags("Health");

app.MapHealthChecks("/health/details", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            results = report.Entries.Select(e => new
            {
                component = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
}).WithTags("Health");

app.MapControllers();

app.Run();

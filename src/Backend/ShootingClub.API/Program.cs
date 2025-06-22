using Microsoft.OpenApi.Models;
using ShootingClub.API.Converters;
using ShootingClub.API.Filters;
using ShootingClub.API.Middleware;
using ShootingClub.API.Token;
using ShootingClub.Application;
using ShootingClub.Domain.Security.Tokens;
using ShootingClub.Infrastructure;
using ShootingClub.Infrastructure.Extensions;
using ShootingClub.Infrastructure.Migrations;

const string CORSSpecifcOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
    { 
        Description = @"JWT Autorization header using the Bearer scheme.
        Enter 'Bearer' [space] and then your token in the text input below.
        Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"

    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>() 
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORSSpecifcOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173") // URL do seu front-end React
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(CORSSpecifcOrigins); // Habilita a pol�tica de CORS
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

app.Run();

void MigrateDatabase()
{
    var connectionString = builder.Configuration.ConnectionString();
    var serviceProvider = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    DatabaseMigration.Migrate(connectionString, serviceProvider.ServiceProvider);
}

﻿using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Security.Tokens;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Infrastructure.DataAccess;
using ShootingClub.Infrastructure.DataAccess.Repositories;
using ShootingClub.Infrastructure.Extensions;
using ShootingClub.Infrastructure.Security.Tokens.Access.Generator;
using ShootingClub.Infrastructure.Security.Tokens.Access.Validator;
using ShootingClub.Infrastructure.Services.LoggedUsuario;
using System.Reflection;

namespace ShootingClub.Infrastructure
{
    public static class DependecyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
            AddLoggedUsuario(services);
            AddTokens(services, configuration);
            AddDbContext(services,configuration);
            AddFluentMigrator(services, configuration);
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();
            services.AddDbContext<ShootingClubDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUsuarioWriteOnlyRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioReadOnlyRepository, UsuarioRepository>();
        }

        private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();
            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("ShootingClub.Infrastructure")).For.All();
            });
        }

        private static void AddTokens(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
            services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));
        }

        private static void AddLoggedUsuario(IServiceCollection services) => services.AddScoped<ILoggedUsuario, LoggedUsuario>();
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShootingClub.Application.Services.AutoMapper;
using ShootingClub.Application.UseCases.Arma.Filter;
using ShootingClub.Application.UseCases.Arma.Register;
using ShootingClub.Application.UseCases.Clube.Register;
using ShootingClub.Application.UseCases.Login.DoLogin;
using ShootingClub.Application.UseCases.Usuario.ChangeSenha;
using ShootingClub.Application.UseCases.Usuario.Profile;
using ShootingClub.Application.UseCases.Usuario.Register;
using ShootingClub.Application.UseCases.Usuario.Update;

namespace ShootingClub.Application
{
    public static class DependecyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddAutoMapper(services);
            AddUseCases(services);
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddScoped(option => new AutoMapper.MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping());
            }).CreateMapper());
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUsuarioUseCase, RegisterUsuarioUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IGetUsuarioProfileUseCase, GetUsuarioProfileUseCase>();
            services.AddScoped<IUpdateUsuarioUseCase, UpdateUsuarioUseCase>();
            services.AddScoped<IChangeSenhaUseCase, ChangeSenhaUseCase>();
            services.AddScoped<IRegisterClubeUseCase, RegisterClubeUseCase>();
            services.AddScoped<IRegisterArmaUseCase, RegisterArmaUseCase>();
            services.AddScoped<IFilterArmaUseCase, FilterArmaUseCase>();
        }

    }
}

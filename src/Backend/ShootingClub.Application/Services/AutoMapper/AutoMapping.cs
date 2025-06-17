using AutoMapper;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;

namespace ShootingClub.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            CreateMap<RequestRegisterUsuarioJson, Domain.Entities.Usuario>()
                .ForMember(dest => dest.Senha, opt => opt.Ignore())
                .ForMember(dest => dest.CPF, opt => opt.Ignore());

            CreateMap<RequestRegisterClubeJson, Domain.Entities.Clube>()
                .ForMember(dest => dest.CNPJ, opt => opt.Ignore());

            CreateMap<RequestArmaJson, Domain.Entities.Arma>();

        }

        private void DomainToResponse()
        {
            CreateMap<Domain.Entities.Usuario, ResponseUsuarioProfileJson>();
        }
    }
}

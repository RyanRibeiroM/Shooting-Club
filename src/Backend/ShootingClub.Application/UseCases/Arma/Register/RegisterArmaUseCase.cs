using AutoMapper;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Arma.Register
{
    public class RegisterArmaUseCase : IRegisterArmaUseCase
    {
        private readonly IArmaWriteOnlyRepository _repository;
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterArmaUseCase(IArmaWriteOnlyRepository repository, ILoggedUsuario loggedUsuario, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _loggedUsuario = loggedUsuario;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseRegisteredArmaJson> Execute(RequestArmaJson request)
        {
            Validate(request);

            var loggedUsuario = _loggedUsuario.Usuario();

            var Arma = _mapper.Map<Domain.Entities.Arma>(request);

            return new ResponseRegisteredArmaJson
            {
                Tipo = string.Empty,
                Marca = string.Empty,
                NumeroSerie = string.Empty
            };

        }

        private static void Validate(RequestArmaJson request)
        {
            var result = new ArmaValidator().Validate(request);
            if (!result.IsValid)
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}

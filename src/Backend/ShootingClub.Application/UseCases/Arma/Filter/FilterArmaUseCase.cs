using AutoMapper;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingClub.Application.UseCases.Arma.Filter
{
    public class FilterArmaUseCase : IFilterArmaUseCase
    {
        private readonly IMapper _mapper;
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IArmaReadOnlyRepository _armaReadOnlyRepository;
        public FilterArmaUseCase(IMapper mapper, ILoggedUsuario loggedUsuario, IArmaReadOnlyRepository armaReadOnlyRepository)
        {
            _mapper = mapper;
            _loggedUsuario = loggedUsuario;
            _armaReadOnlyRepository = armaReadOnlyRepository;
        }
        public async Task<ResponseArmasJson> Execute(RequestFilterArmaJson request)
        {
            Validate(request);
            var loggedUser = await _loggedUsuario.Usuario();

            var filters = new Domain.Dtos.FilterArmasDto 
            {
                TipoPosse = (Domain.Enums.TipoPosseArma?)request.TipoPosse,
                Tipo = request.Tipo,
                Marca = request.Marca,
                NumeroSerie = request.NumeroSerie,
                ProximoExpiracao = request.ProximoExpiracao,
                SoArmasDoClube = request.SoArmasDoClube,
                IdUsuario = request.IdUsuario
            };
            var armas = await _armaReadOnlyRepository.Filter(loggedUser, filters);

            return new ResponseArmasJson { Armas = _mapper.Map<List<ResponseArmaShortJson>>(armas) };
           
        }

        private static void Validate(RequestFilterArmaJson request)
        {
            var validator = new FilterArmasValidator();

            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}

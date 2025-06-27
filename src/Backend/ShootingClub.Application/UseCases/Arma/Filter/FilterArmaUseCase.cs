using AutoMapper;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
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
        public FilterArmaUseCase(IMapper mapper, ILoggedUsuario loggedUsuario)
        {
            _mapper = mapper;
            _loggedUsuario = loggedUsuario;
        }
        public async Task<ResponseArmasJson> Execute(RequestFilterArmaJson request)
        {
            Validate(request);
            var loggedUser = await _loggedUsuario.Usuario();
            return new ResponseArmasJson { Armas = [] };
           
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

﻿using AutoMapper;
using ShootingClub.Application.UseCases.Usuario.Register;
using ShootingClub.Application.Utils;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Responses;
using ShootingClub.Domain.Repositories;
using ShootingClub.Domain.Repositories.Clube;
using ShootingClub.Domain.Repositories.Usuario;
using ShootingClub.Domain.Services.LoggedUsuario;
using ShootingClub.Exceptions;
using ShootingClub.Exceptions.ExceptionsBase;

namespace ShootingClub.Application.UseCases.Clube.Register
{
    public class RegisterClubeUseCase : IRegisterClubeUseCase
    {
        private readonly ILoggedUsuario _loggedUsuario;
        private readonly IClubeReadOnlyRepository _clubeReadOnlyRepository;
        private readonly IClubeWriteOnlyRepository _clubeWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterClubeUseCase(ILoggedUsuario loggedUsuario, IClubeReadOnlyRepository clubeReadOnlyRepository, IClubeWriteOnlyRepository clubeWriteOnlyRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _loggedUsuario = loggedUsuario;
            _clubeReadOnlyRepository = clubeReadOnlyRepository;
            _clubeWriteOnlyRepository = clubeWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseRegisteredClubeJson> Execute(RequestRegisterClubeJson request)
        {
            var loggedUsuario = await _loggedUsuario.Usuario();

            await Validate(request, loggedUsuario.Id);

            var clube = _mapper.Map<Domain.Entities.Clube>(request);

            clube.ResponsavelId = loggedUsuario.Id;

            clube.CNPJ = CnpjUtils.Format(request.CNPJ);
            clube.AtualizadoEm = DateTime.UtcNow;

            await _clubeWriteOnlyRepository.Add(clube);
            await _unitOfWork.Commit();

            return new ResponseRegisteredClubeJson { Nome = request.Nome };
        }

        private async Task Validate(RequestRegisterClubeJson request, int idUsuario)
        {
            var validator = new RegisterClubeValidator();

            var result = validator.Validate(request);
            var cnpj = CnpjUtils.Format(request.CNPJ);
            var AdminExist = await _clubeReadOnlyRepository.ExistActiveClubeWithAdmin(idUsuario);
            var cnpjExist = await _clubeReadOnlyRepository.ExistActiveClubeWithCNPJ(cnpj);

            if (AdminExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.ADMIN_JA_POSSUI_CLUBE));

            if (cnpjExist)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.CNPJ_JA_CADASTRADO));

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}

using FluentValidation;
using ShootingClub.Application.Utils;
using ShootingClub.Communication.Requests;
using ShootingClub.Exceptions;

namespace ShootingClub.Application.UseCases.Clube.Register
{
    public class RegisterClubeValidator : AbstractValidator<RequestRegisterClubeJson>
    {
        public RegisterClubeValidator()
        {
            RuleFor(clube => clube.Nome)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.NOME_INVALIDO);

            RuleFor(clube => clube.CNPJ)
                .NotEmpty()
                .Must(CnpjUtils.ValidCNPJ)
                .WithMessage(ResourceMessagesException.CNPJ_INVALIDO);

            RuleFor(clube => clube.CertificadoRegistro)
                .NotEmpty()
                .Matches("^[0-9]+$").WithMessage(ResourceMessagesException.CLUBE_CR_INVALIDO);

            RuleFor(clube => clube.EnderecoPais)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_PAIS_INVALIDO);

            RuleFor(clube => clube.EnderecoEstado)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_ESTADO_INVALIDO);

            RuleFor(clube => clube.EnderecoCidade)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_CIDADE_INVALIDO);

            RuleFor(clube => clube.EnderecoBairro)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_BAIRRO_INVALIDO);

            RuleFor(clube => clube.EnderecoRua)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_RUA_INVALIDA);

            RuleFor(clube => clube.EnderecoNumero)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_NUMERO_INVALIDO);

        }
    }
}

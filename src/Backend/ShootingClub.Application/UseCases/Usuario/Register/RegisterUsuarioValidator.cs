using FluentValidation;
using ShootingClub.Application.SharedValidators;
using ShootingClub.Application.Utils;
using ShootingClub.Communication.Requests;
using ShootingClub.Exceptions;
using System.Data;

namespace ShootingClub.Application.UseCases.Usuario.Register
{
    public class RegisterUsuarioValidator : AbstractValidator<RequestRegisterUsuarioJson>
    {
        public RegisterUsuarioValidator()
        {
            RuleFor(usuario => usuario.Nome)
                .NotEmpty()
                .Must(nome => nome.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length >= 2)
                .WithMessage(ResourceMessagesException.NOME_INVALIDO);

            RuleFor(usuario => usuario.Email).NotEmpty()
                .EmailAddress()
                .WithMessage(ResourceMessagesException.EMAIL_INVALIDO);

            RuleFor(usuario => usuario.Senha).SetValidator(new SenhaValidator<RequestRegisterUsuarioJson>());

            RuleFor(Usuario => Usuario.CPF)
                .NotEmpty()
                .Must(CpfUtils.ValidCPF)
                .WithMessage(ResourceMessagesException.CPF_INVALIDO);

            RuleFor(usuario => usuario.DataNascimento)
                .NotEmpty()
                .LessThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.Today))
                .WithMessage(ResourceMessagesException.DATA_NASCIMENTO_INVALIDA);

            RuleFor(usuario => usuario.EnderecoPais)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_PAIS_INVALIDO);

            RuleFor(usuario => usuario.EnderecoEstado)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_ESTADO_INVALIDO);

            RuleFor(usuario => usuario.EnderecoCidade)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_CIDADE_INVALIDO);

            RuleFor(usuario => usuario.EnderecoBairro)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_BAIRRO_INVALIDO);

            RuleFor(usuario => usuario.EnderecoRua)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_RUA_INVALIDA);

            RuleFor(usuario => usuario.EnderecoNumero)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.ENDERECO_NUMERO_INVALIDO);

            RuleFor(usuario => usuario.NumeroFiliacao)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.NUMERO_FILIACAO_INVALIDO);

            RuleFor(usuario => usuario.DataFiliacao)
                .NotEmpty()
                .LessThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.Today))
                .WithMessage(ResourceMessagesException.DATA_FILIACAO_INVALIDA);

            RuleFor(usuario => usuario.DataRenovacaoFiliacao)
                .NotEmpty()
                .LessThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.Today))
                .Must((usuario, dataRenovacao) => dataRenovacao > usuario.DataFiliacao)
                .WithMessage(ResourceMessagesException.DATA_RENOVACAO_FILIACAO_INVALIDA);

            When(usuario => !string.IsNullOrWhiteSpace(usuario.CR), () =>
            {
                RuleFor(usuario => usuario.CR)
                    .Matches("^[0-9]+$")
                    .WithMessage(ResourceMessagesException.CR_INVALIDO);

                RuleFor(usuario => usuario.DataVencimentoCR)
                    .NotEmpty().WithMessage(ResourceMessagesException.DATA_VENCIMENTO_CR_REQUERIDA)
                    .GreaterThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ResourceMessagesException.DATA_VENCIMENTO_CR_INVALIDA);

                RuleFor(usuario => usuario.SFPCVinculacao)
                    .NotEmpty().WithMessage(ResourceMessagesException.SFPC_VINCULACAO_REQUERIDO);
            })
            .Otherwise(() =>
            {
                RuleFor(usuario => usuario.DataVencimentoCR)
                    .Empty()
                    .WithMessage(ResourceMessagesException.DATA_VENCIMENTO_CR_SEM_CR);

                RuleFor(usuario => usuario.SFPCVinculacao)
                    .Empty().WithMessage(ResourceMessagesException.SFPC_VINCULACAO_SEM_CR);
            });
        }
    }
}
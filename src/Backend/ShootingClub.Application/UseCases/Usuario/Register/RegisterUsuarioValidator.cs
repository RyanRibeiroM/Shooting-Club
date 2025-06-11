using FluentValidation;
using ShootingClub.Communication.Requests;
using ShootingClub.Exceptions;
using System.Data;

namespace ShootingClub.Application.UseCases.Usuario.Register
{
    internal class RegisterUsuarioValidator : AbstractValidator<RequestRegisterUsuarioJson>
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

            RuleFor(usuario => usuario.Senha)
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage(ResourceMessagesException.SENHA_INVALIDA);
                

            When(usuario => usuario.Senha.Length >= 8, () =>
            {
                RuleFor(usuario => usuario.Senha)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
                .WithMessage(ResourceMessagesException.SENHA_INVALIDA);
            });

            RuleFor(Usuario => Usuario.CPF)
                .NotEmpty()
                .Must(ValidCPF)
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
                .WithMessage(ResourceMessagesException.DATA_RENOVACAO_FILIACAO_INVALIDA);

            RuleFor(usuario => usuario.DataRenovacaoFiliacao)
                .NotEmpty()
                .LessThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.Today))
                .Must((usuario, dataRenovacao) => dataRenovacao > usuario.DataFiliacao)
                .WithMessage(ResourceMessagesException.DATA_RENOVACAO_FILIACAO_INVALIDA);
        }

        private bool ValidCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            var digits = new string(cpf.Where(char.IsDigit).ToArray());

            if (digits.Length != 11)
                return false;

            if (digits.Distinct().Count() == 1)
                return false;

            var nums = digits.Select(c => c - '0').ToArray();

            int soma1 = 0;
            for (int i = 0; i < 9; i++)
                soma1 += nums[i] * (10 - i);

            int resto1 = (soma1 * 10) % 11;
            if (resto1 == 10) resto1 = 0;
            if (nums[9] != resto1)
                return false;

            int soma2 = 0;
            for (int i = 0; i < 10; i++)
                soma2 += nums[i] * (11 - i);

            int resto2 = (soma2 * 10) % 11;
            if (resto2 == 10) resto2 = 0;
            if (nums[10] != resto2)
                return false;

            return true;
        }

    }
}

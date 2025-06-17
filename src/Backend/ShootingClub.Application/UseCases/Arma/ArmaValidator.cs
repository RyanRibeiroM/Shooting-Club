using FluentValidation;
using ShootingClub.Communication.Requests;
using ShootingClub.Communication.Enums;
using ShootingClub.Exceptions;

namespace ShootingClub.Application.UseCases.Arma
{
    public class ArmaValidator : AbstractValidator<RequestArmaJson>
    {
        public ArmaValidator()
        {
            RuleFor(arma => arma.Tipo).NotEmpty().WithMessage(ResourceMessagesException.TIPO_ARMA_VAZIO);
            RuleFor(arma => arma.Marca).NotEmpty().WithMessage(ResourceMessagesException.MARCA_ARMA_VAZIO);
            RuleFor(arma => arma.NumeroSerie).NotEmpty().WithMessage(ResourceMessagesException.NUMERO_SERIE_ARMA_VAZIO);
            RuleFor(arma => arma.TipoPosse).IsInEnum().WithMessage(ResourceMessagesException.TIPO_POSSE_ARMA_INVALIDO);

            RuleFor(arma => arma.Calibre)
                .GreaterThan(0)
                .When(arma => arma.Calibre.HasValue && arma.Calibre > 0)
                .WithMessage(ResourceMessagesException.CALIBRE_ARMA_INVALIDO);

            When(arma => arma.TipoPosse == TipoPosseArma.Exercito, () =>
            {
                RuleFor(arma => arma.NumeroSigma).NotEmpty().Matches("^[0-9]+$").WithMessage(ResourceMessagesException.NUMERO_SIGMA_INVALIDO);
                RuleFor(arma => arma.ValidadeCRAF).NotEmpty().GreaterThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ResourceMessagesException.VALIDADE_CRAF_INVALIDA);
                RuleFor(arma => arma.DataExpedicaoCRAF).NotEmpty().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ResourceMessagesException.DATA_EXPEDICAO_CRAF_INVALIDA);
            });

            When(arma => arma.TipoPosse == TipoPosseArma.PoliciaFederal, () =>
            {
                RuleFor(arma => arma.NumeroSinarm).NotEmpty().WithMessage(ResourceMessagesException.NUMERO_SINARM_VAZIO);
                RuleFor(arma => arma.NumeroRegistroPF).NotEmpty().WithMessage(ResourceMessagesException.NUMERO_REGISTRO_PF_VAZIO);
                RuleFor(arma => arma.NumeroNotaFiscal).NotEmpty().WithMessage(ResourceMessagesException.NUMERO_NF_VAZIO);
                RuleFor(arma => arma.DataValidadePF).NotEmpty().GreaterThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ResourceMessagesException.VALIDADE_PF_INVALIDA);
            });

            When(arma => arma.TipoPosse == TipoPosseArma.PortePessoal, () =>
            {
                RuleFor(arma => arma.NumeroCertificado).NotEmpty().WithMessage(ResourceMessagesException.NUMERO_CERTIFICADO_VAZIO);
                RuleFor(arma => arma.ValidadeCertificacao).NotEmpty().GreaterThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ResourceMessagesException.VALIDADE_CERTIFICADO_INVALIDA);
            });

            When(arma => !string.IsNullOrWhiteSpace(arma.NumeroGTE), () =>
            {
                RuleFor(arma => arma.ValidadeGTE).NotEmpty().GreaterThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage(ResourceMessagesException.VALIDADE_GTE_INVALIDA);
            });

            RuleFor(arma => arma)
                .Must(HaveConsistentData)
                .WithMessage(ResourceMessagesException.DADOS_INCONSISTENTES_TIPO_POSSE);
        }

        private bool HaveConsistentData(RequestArmaJson arma)
        {
            switch (arma.TipoPosse)
            {
                case TipoPosseArma.Exercito:
                    return string.IsNullOrWhiteSpace(arma.NumeroSinarm) &&
                           string.IsNullOrWhiteSpace(arma.NumeroRegistroPF) &&
                           string.IsNullOrWhiteSpace(arma.NumeroNotaFiscal) &&
                           !arma.DataValidadePF.HasValue &&
                           string.IsNullOrWhiteSpace(arma.NumeroCertificado) &&
                           !arma.ValidadeCertificacao.HasValue;

                case TipoPosseArma.PoliciaFederal:
                    return string.IsNullOrWhiteSpace(arma.NumeroSigma) &&
                           !arma.ValidadeCRAF.HasValue &&
                           !arma.DataExpedicaoCRAF.HasValue &&
                           string.IsNullOrWhiteSpace(arma.NumeroCertificado) &&
                           !arma.ValidadeCertificacao.HasValue;

                case TipoPosseArma.PortePessoal:
                    return string.IsNullOrWhiteSpace(arma.NumeroSigma) &&
                           !arma.ValidadeCRAF.HasValue &&
                           !arma.DataExpedicaoCRAF.HasValue &&
                           string.IsNullOrWhiteSpace(arma.NumeroSinarm) &&
                           string.IsNullOrWhiteSpace(arma.NumeroRegistroPF) &&
                           string.IsNullOrWhiteSpace(arma.NumeroNotaFiscal) &&
                           !arma.DataValidadePF.HasValue;
                default:
                    break;
            }
            return true;
        }
    }
}
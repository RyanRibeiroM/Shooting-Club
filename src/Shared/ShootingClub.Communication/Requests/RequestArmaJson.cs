using ShootingClub.Communication.Enums;

namespace ShootingClub.Communication.Requests
{
    public class RequestArmaJson
    {
        public string Tipo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public decimal? Calibre { get; set; }
        public string NumeroSerie { get; set; } = string.Empty;
        public TipoPosseArma TipoPosse { get; set; }
        public string? NumeroSigma { get; set; }
        public string? LocalRegistro { get; set; }
        public DateOnly? DataExpedicaoCRAF { get; set; }
        public DateOnly? ValidadeCRAF { get; set; }
        public string? NumeroGTE { get; set; }
        public DateOnly? ValidadeGTE { get; set; }
        public string? NumeroSinarm { get; set; }
        public string? NumeroRegistroPF { get; set; }
        public string? NumeroNotaFiscal { get; set; }
        public DateOnly? DataValidadePF { get; set; }
        public string? NumeroCertificado { get; set; }
        public DateOnly? DataCertificacao { get; set; }
        public DateOnly? ValidadeCertificacao { get; set; }
    }
}

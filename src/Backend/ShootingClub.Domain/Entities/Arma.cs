using ShootingClub.Domain.Enums;

namespace ShootingClub.Domain.Entities
{
    public class Arma : EntityBase
    {
        public string Tipo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public float Calibre { get; set; }
        public string NumeroSerie { get; set; } = string.Empty;
        public TipoPosseArma TipoPosse { get; set; }
        public string NumeroSigma { get; set; } = string.Empty;
        public string NumeroSinarm { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public int ClubeId { get; set; }
        public string NumeroGTE { get; set; } = string.Empty;
        public DateOnly ValidadeGTE { get; set; }
        public DateOnly DataCertificacao { get; set; }
        public DateOnly ValidadeCertificacao { get; set; }

    }
}

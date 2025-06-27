using ShootingClub.Domain.Enums;

namespace ShootingClub.Domain.Entities
{
    public abstract class ArmaBase : EntityBase
    {
        public string Tipo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string? Calibre { get; set; }
        public string NumeroSerie { get; set; } = string.Empty;
        public TipoPosseArma TipoPosse { get; set; }
        public int UsuarioId { get; set; }
        public int ClubeId { get; set; }
    }
}

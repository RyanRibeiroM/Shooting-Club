namespace ShootingClub.Domain.Entities
{
    public class ArmaPortePessoal : ArmaBase
    {
        public string? NumeroCertificado { get; set; }
        public DateOnly? DataCertificacao { get; set; }
        public DateOnly? ValidadeCertificacao { get; set; }
    }
}

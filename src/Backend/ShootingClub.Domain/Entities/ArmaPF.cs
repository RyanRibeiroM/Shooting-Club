namespace ShootingClub.Domain.Entities
{
    public class ArmaPF : ArmaBase
    {
        public string? NumeroSinarm { get; set; }
        public string? NumeroRegistroPF { get; set; }
        public string? NumeroNotaFiscal { get; set; }
        public DateOnly? DataValidadePF { get; set; }
    }
}

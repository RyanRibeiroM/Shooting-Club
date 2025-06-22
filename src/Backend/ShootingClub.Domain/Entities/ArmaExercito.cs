namespace ShootingClub.Domain.Entities
{
    public class ArmaExercito : ArmaBase
    {
        public string? NumeroSigma { get; set; }
        public string? LocalRegistro { get; set; }
        public DateOnly? DataExpedicaoCRAF { get; set; }
        public DateOnly? ValidadeCRAF { get; set; }
        public string? NumeroGTE { get; set; }
        public DateOnly? ValidadeGTE { get; set; }
    }
}

namespace ShootingClub.Communication.Responses
{
    public class ResponseArmaExercitoShortJson : ResponseShortArmaJson
    {
        public DateOnly? ValidadeCRAF { get; set; }
        public DateOnly? ValidadeGTE { get; set; }
    }
}

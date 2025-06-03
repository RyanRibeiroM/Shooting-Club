namespace ShootingClub.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : ShootingClubException
    {
        public IList<string> ErrorMessages { get; set; }

        public ErrorOnValidationException(IList<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }
    }
}

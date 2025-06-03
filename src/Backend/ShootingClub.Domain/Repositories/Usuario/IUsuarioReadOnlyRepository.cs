namespace ShootingClub.Domain.Repositories.Usuario
{
    public interface IUsuarioReadOnlyRepository
    {
        Task<bool> ExistActiveUsuarioWithEmail(string email);
        Task<bool> ExistActiveUsuarioWithCPF(string cpf);
        Task<bool> ExistActiveUsuarioWithNumeroFiliacao(string numeroFiliacao);
    }
}

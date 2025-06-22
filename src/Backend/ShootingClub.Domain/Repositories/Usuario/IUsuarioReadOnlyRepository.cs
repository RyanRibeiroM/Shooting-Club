namespace ShootingClub.Domain.Repositories.Usuario
{
    public interface IUsuarioReadOnlyRepository
    {
        Task<bool> ExistActiveUsuarioWithEmail(string email);
        Task<bool> ExistActiveUsuarioWithCPF(string cpf);
        Task<bool> ExistActiveUsuarioWithCR(string cr);
        Task<bool> ExistActiveUsuarioWithNumeroFiliacao(string numeroFiliacao);
        public Task<Entities.Usuario?> GetByEmailAndSenha(string email, string senha);
        public Task<bool> ExistActiveUserWithIdentificador(Guid IdentificadorUsuario);
        public Task<bool> ExistActiveAdminWithIdentificador(Guid IdentificadorUsuario);
    }
}

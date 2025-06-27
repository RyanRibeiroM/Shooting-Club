namespace ShootingClub.Domain.Repositories.Usuario
{
    public interface IUsuarioReadOnlyRepository
    {
        Task<bool> ExistActiveUsuarioWithEmail(string email);
        Task<bool> ExistActiveUsuarioWithCPF(string cpf);
        Task<bool> ExistActiveUsuarioWithCR(string cr);
        Task<bool> ExistActiveUsuarioWithClubeAndCPF(int clubeId, string cpf);
        Task<bool> ExistActiveUsuarioWithNumeroFiliacao(string numeroFiliacao);
        Task<bool> ActiveUsuarioHasClube(Guid IdentificadorUsuario);
        public Task<Entities.Usuario?> GetByEmailAndSenha(string email, string senha);
        public Task<int> GetIdUsuarioByCPF(string cpf);
        public Task<bool> ExistActiveUserWithIdentificador(Guid IdentificadorUsuario);
    }
}

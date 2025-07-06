namespace ShootingClub.Domain.Repositories.Clube
{
    public interface IClubeReadOnlyRepository
    {
        Task<Entities.Clube> GetById(int id);
        Task<bool> ExistActiveClubeWithAdmin(int responsavelId);
        Task<bool> ExistActiveClubeWithCNPJ(string cnpj);
        Task<bool> ExistActiveClubeWithCertificadoRegistro(string certificadoRegistro);
    }
}

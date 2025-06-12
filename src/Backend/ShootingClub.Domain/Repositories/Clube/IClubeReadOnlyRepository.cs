namespace ShootingClub.Domain.Repositories.Clube
{
    public interface IClubeReadOnlyRepository
    {
        Task<bool> ExistActiveClubeWithAdmin(int responsavelId);
        Task<bool> ExistActiveClubeWithCNPJ(string cnpj);
    }
}

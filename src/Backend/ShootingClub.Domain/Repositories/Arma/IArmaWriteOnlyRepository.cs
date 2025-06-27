namespace ShootingClub.Domain.Repositories.Arma
{
    public interface IArmaWriteOnlyRepository
    {
        public Task Add(Entities.ArmaBase arma);
    }
}

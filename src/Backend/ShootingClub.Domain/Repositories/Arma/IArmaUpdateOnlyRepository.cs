using ShootingClub.Domain.Entities;

namespace ShootingClub.Domain.Repositories.Arma
{
    public interface IArmaUpdateOnlyRepository
    {
        Task<Entities.ArmaBase?> GetById(Entities.Usuario usuario, int armaId);
        Task<bool> ExistActiveArmaWithNumeroSerieAndIgnoreId(string NumeroSerie, int Armaid);
        void Update(Entities.ArmaBase arma);
    }
}

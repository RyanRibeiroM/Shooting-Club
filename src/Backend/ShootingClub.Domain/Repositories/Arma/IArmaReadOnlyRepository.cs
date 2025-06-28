using ShootingClub.Domain.Dtos;

namespace ShootingClub.Domain.Repositories.Arma
{
    public interface IArmaReadOnlyRepository
    {
        Task<bool> ExistActiveArmaWithNumeroSerie(string numeroSerie);
        Task<IList<Entities.ArmaBase>> Filter(Entities.Usuario usuario, FilterArmasDto filters);
        
    }
}

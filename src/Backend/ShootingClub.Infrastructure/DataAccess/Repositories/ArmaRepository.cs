using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Repositories.Arma;

namespace ShootingClub.Infrastructure.DataAccess.Repositories
{
    public class ArmaRepository : IArmaWriteOnlyRepository
    {
        private readonly ShootingClubDbContext _dbContext;

        public ArmaRepository(ShootingClubDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(ArmaBase arma) => await _dbContext.Armas.AddAsync(arma);

    }
}

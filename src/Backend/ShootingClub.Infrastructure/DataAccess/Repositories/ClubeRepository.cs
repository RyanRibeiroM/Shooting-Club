using Microsoft.EntityFrameworkCore;
using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Repositories.Clube;

namespace ShootingClub.Infrastructure.DataAccess.Repositories
{
    public class ClubeRepository : IClubeReadOnlyRepository, IClubeUpdateOnlyRepository, IClubeWriteOnlyRepository
    {
        private readonly ShootingClubDbContext _dbContext;

        public ClubeRepository(ShootingClubDbContext dbContext) => _dbContext = dbContext;
        public async Task Add(Clube clube) => await _dbContext.Clubes.AddAsync(clube);
        public async Task<bool> ExistActiveClubeWithAdmin(int responsavelId) => await _dbContext.Clubes.AnyAsync(clube => clube.ResponsavelId == responsavelId && clube.Ativo);

        public async Task<bool> ExistActiveClubeWithCNPJ(string cnpj) => await _dbContext.Clubes.AnyAsync(clube => clube.CNPJ.Equals(cnpj) && clube.Ativo);

        public async Task<Clube> GetById(int id) => await _dbContext.Clubes.FirstAsync(usuario => usuario.Id == id);
        
        public void Update(Clube clube) => _dbContext.Clubes.Update(clube);
        
    }
}

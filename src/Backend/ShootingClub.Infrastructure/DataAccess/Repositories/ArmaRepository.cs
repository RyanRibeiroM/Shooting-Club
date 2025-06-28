using Microsoft.EntityFrameworkCore;
using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Repositories.Arma;
using ShootingClub.Domain.Enums;
using ShootingClub.Domain.Dtos;

namespace ShootingClub.Infrastructure.DataAccess.Repositories
{
    public class ArmaRepository : IArmaWriteOnlyRepository, IArmaReadOnlyRepository
    {
        private readonly ShootingClubDbContext _dbContext;

        public ArmaRepository(ShootingClubDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(ArmaBase arma) => await _dbContext.Armas.AddAsync(arma);

        public async Task<bool> ExistActiveArmaWithNumeroSerie(string numeroSerie)
        {
            return await _dbContext.Armas.AnyAsync(arma => arma.NumeroSerie.Equals(numeroSerie) && arma.Ativo);
        }

        public async Task<IList<ArmaBase>> Filter(Usuario usuario, FilterArmasDto filters)
        {
            var query = _dbContext.Armas.AsNoTracking().Where(arma => arma.Ativo);


            if (usuario.Nivel == NivelUsuario.AdminUsuario)
            {
                var armasDeMembrosDoClube =
                    from arma in query
                    join user in _dbContext.Usuarios on arma.UsuarioId equals user.Id
                    where user.ClubeId == usuario.ClubeId
                    select arma;

                var armasDoClube = query.Where(arma => arma.ClubeId == usuario.ClubeId && arma.UsuarioId == 0);

                query = armasDeMembrosDoClube.Union(armasDoClube);
            }
            else if (usuario.Nivel == NivelUsuario.CommonUsuario)
            {
                query = query.Where(a => a.UsuarioId == usuario.Id);
            }

            if (filters.IdUsuario.HasValue)
            {
                query = query.Where(a => a.UsuarioId == filters.IdUsuario.Value);
            }

            if (filters.SoArmasDoClube == true)
            {
                query = query.Where(a => a.ClubeId == usuario.ClubeId && a.UsuarioId == 0);
            }

            if (filters.TipoPosse.HasValue)
            {
                query = query.Where(arma => arma.TipoPosse == filters.TipoPosse.Value);
            }

            if (!string.IsNullOrWhiteSpace(filters.Tipo))
            {
                query = query.Where(arma => arma.Tipo.Contains(filters.Tipo));
            }

            if (!string.IsNullOrWhiteSpace(filters.Marca))
            {
                query = query.Where(arma => arma.Marca.Contains(filters.Marca));
            }

            if (!string.IsNullOrWhiteSpace(filters.NumeroSerie))
            {
                query = query.Where(arma => arma.NumeroSerie.Contains(filters.NumeroSerie));
            }

            if (filters.ProximoExpiracao == true)
            {
                var dataLimite = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));
                var hoje = DateOnly.FromDateTime(DateTime.UtcNow);

                query = query.Where(arma =>
                    (arma is ArmaExercito &&
                        (((ArmaExercito)arma).ValidadeCRAF.HasValue && ((ArmaExercito)arma).ValidadeCRAF <= dataLimite) ||
                        (((ArmaExercito)arma).ValidadeGTE.HasValue && ((ArmaExercito)arma).ValidadeGTE <= dataLimite)
                    ) ||
                    (arma is ArmaPF &&
                        ((ArmaPF)arma).DataValidadePF.HasValue && ((ArmaPF)arma).DataValidadePF <= dataLimite
                    ) ||
                    (arma is ArmaPortePessoal &&
                        ((ArmaPortePessoal)arma).ValidadeCertificacao.HasValue && ((ArmaPortePessoal)arma).ValidadeCertificacao <= dataLimite)
                );
            }

            return await query.ToListAsync();
        }
    }
}

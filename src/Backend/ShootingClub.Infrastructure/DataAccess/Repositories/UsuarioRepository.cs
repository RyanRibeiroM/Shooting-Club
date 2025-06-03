using Microsoft.EntityFrameworkCore;
using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Repositories.Usuario;

namespace ShootingClub.Infrastructure.DataAccess.Repositories
{
    public class UsuarioRepository : IUsuarioWriteOnlyRepository, IUsuarioReadOnlyRepository
    {
        private readonly ShootingClubDbContext _dbContext;

        public UsuarioRepository(ShootingClubDbContext dbContext) => _dbContext = dbContext;


        public async Task Add(Usuario usuario) => await _dbContext.Usuarios.AddAsync(usuario);

        public async Task<bool> ExistActiveUsuarioWithEmail(string email)
        {
           return await _dbContext.Usuarios.AnyAsync(usuario => usuario.Email.Equals(email) && usuario.Ativo);
        }

        public async Task<bool> ExistActiveUsuarioWithCPF(string cpf)
        {
            return await _dbContext.Usuarios.AnyAsync(usuario => usuario.CPF.Equals(cpf) && usuario.Ativo);
        }

        public async Task<bool> ExistActiveUsuarioWithNumeroFiliacao(string numeroFiliacao)
        {
            return await _dbContext.Usuarios.AnyAsync(usuario => usuario.NumeroFiliacao.Equals(numeroFiliacao) && usuario.Ativo);
        }



    }
}

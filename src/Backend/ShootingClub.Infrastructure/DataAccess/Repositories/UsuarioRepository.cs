using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Enums;
using ShootingClub.Domain.Repositories.Usuario;

namespace ShootingClub.Infrastructure.DataAccess.Repositories
{
    public class UsuarioRepository : IUsuarioWriteOnlyRepository, IUsuarioReadOnlyRepository, IUsuarioUpdateOnlyRepository
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

        public async Task<bool> ExistActiveUsuarioWithCR(string cr)
        {
            return await _dbContext.Usuarios.AnyAsync(usuario => usuario.CR.Equals(cr) && usuario.Ativo);
        }

        public async Task<bool> ExistActiveUsuarioWithNumeroFiliacao(string numeroFiliacao)
        {
            return await _dbContext.Usuarios.AnyAsync(usuario => usuario.NumeroFiliacao.Equals(numeroFiliacao) && usuario.Ativo);
        }


        public async Task<Usuario?> GetByEmailAndSenha(string email, string senha)
        {
            return await _dbContext
                .Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(usuario =>usuario.Ativo && usuario.Email.Equals(email) && usuario.Senha.Equals(senha));
        }

        public async Task<bool> ExistActiveUserWithIdentificador(Guid IdentificadorUsuario) => await _dbContext.Usuarios.AnyAsync(usuario => usuario.IdentificadorUsuario.Equals(IdentificadorUsuario) && usuario.Ativo);

        public async Task<bool> ExistActiveAdminWithIdentificador(Guid IdentificadorUsuario) => await _dbContext.Usuarios.AnyAsync(usuario => usuario.IdentificadorUsuario.Equals(IdentificadorUsuario) && usuario.Ativo && usuario.Nivel == NivelUsuario.AdminUsuario);

        public async Task<Usuario> GetById(int id)
        {
            return await _dbContext
                .Usuarios
                .FirstAsync(usuario => usuario.Id == id);
        }

        public void Update(Usuario usuario) => _dbContext.Usuarios.Update(usuario);
    }
}

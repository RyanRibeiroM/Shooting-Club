using Microsoft.EntityFrameworkCore;
using ShootingClub.Domain.Entities;

namespace ShootingClub.Infrastructure.DataAccess
{
    public class ShootingClubDbContext: DbContext
    {
        public ShootingClubDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShootingClubDbContext).Assembly);
        }
    }
}

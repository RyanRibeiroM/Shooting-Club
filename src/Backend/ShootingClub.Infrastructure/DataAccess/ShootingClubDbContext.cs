using Microsoft.EntityFrameworkCore;
using ShootingClub.Domain.Entities;
using ShootingClub.Domain.Enums;

namespace ShootingClub.Infrastructure.DataAccess
{
    public class ShootingClubDbContext: DbContext
    {
        public ShootingClubDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Clube> Clubes { get; set; }
        public DbSet<ArmaBase> Armas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShootingClubDbContext).Assembly);
            modelBuilder.Entity<ArmaBase>()
            .HasDiscriminator(x => x.TipoPosse)
            .HasValue<ArmaExercito>(TipoPosseArma.Exercito)
            .HasValue<ArmaPF>(TipoPosseArma.PoliciaFederal)
            .HasValue<ArmaPortePessoal>(TipoPosseArma.PortePessoal);
        }
    }
}

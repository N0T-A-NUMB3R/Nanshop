using GestUser.Models;
using Microsoft.EntityFrameworkCore;


namespace Service
{
    public class NanshopDbContext : DbContext
    {
        public NanshopDbContext(DbContextOptions<NanshopDbContext> options)
            : base(options)
        {
            //Database.Migrate();
        }

        public virtual DbSet<Utenti> Utenti { get; set; }
        public virtual DbSet<Profili> Profili { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Utenti>()
                .HasKey(a => new { a.CodFidelity });

            modelBuilder.Entity<Profili>()
               .Property(a => a.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<Profili>()
                .HasOne<Utenti>(s => s.Utente)
                .WithMany(g => g.Profili)
                .HasForeignKey(g => g.CodFidelity);



        }


    }
}
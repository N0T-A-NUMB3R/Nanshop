using ArticoliWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticoliWebService.Services
{
    public class NanshopDbContext : DbContext
    {
        public NanshopDbContext (DbContextOptions<NanshopDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Articoli> Articoli { get; set; }
        public virtual DbSet<Ean> Barcode { get; set; }
        public virtual DbSet<FamAssort> Famassort { get; set; }
        public virtual DbSet<Ingredienti> Ingredienti { get; set; }
        public virtual DbSet<Iva> Iva { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            //Relazione one to many (uno a molti) fra articoli e barcode
            modelBuilder.Entity<Ean>()
                .HasOne<Articoli>(s => s.Articolo) //ad un articolo...
                .WithMany(g => g.Barcode) //corrispondono molti barcode
                .HasForeignKey(s => s.CodArt); //la chiave esterna dell'entity barcode

            //Relazione one to one (uno a uno) fra articoli e ingredienti
            modelBuilder.Entity<Articoli>()
                .HasOne<Ingredienti>(s => s.Ingredienti) //ad un ingrediente...
                .WithOne(g => g.Articolo)  //corrisponde un articolo
                .HasForeignKey<Ingredienti>(s => s.CodArt);

            //Relazione one to many fra iva e articoli 
            modelBuilder.Entity<Articoli>()
                .HasOne<Iva>(s => s.Iva) //ad una aliquota iva
                .WithMany(g => g.Articoli) // corrispondono molti articoli
                .HasForeignKey(s => s.IdIva);

            //Rela<ione one to many fra FamAssort e Articoli
            modelBuilder.Entity<Articoli>()
                .HasOne<FamAssort>(s => s.FamAssort)
                .WithMany(g => g.Articoli)
                .HasForeignKey(s => s.IdFamAss);

        }

    }
}
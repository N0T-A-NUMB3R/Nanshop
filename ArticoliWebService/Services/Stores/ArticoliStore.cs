using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticoliWebService.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ArticoliWebService.Services.Stores
{
    public class ArticoliStore : IArticoliStore
    {
        NanshopDbContext nanshopDbContext{get; set;}
        public ArticoliStore(NanshopDbContext nanshopDbContext)
        {
            this.nanshopDbContext = nanshopDbContext;
        }
        public async Task<bool> ArticoloExists(string code)
        {
            return await this.nanshopDbContext.Articoli
            .AnyAsync(ar => ar.CodArt == code);
        }

        public bool DeleteArticolo(Articoli articolo)
        {
            this.nanshopDbContext.Articoli.Remove(articolo);;
            return Salva().Item1;
        }

        public async Task<ICollection<Articoli>> GetArticoliByDescr(string descrizione)
        {
            return await this.nanshopDbContext.Articoli
            .Where(art => art.Descrizione.Contains(descrizione))
            .OrderBy(art => art.Descrizione)
            .ToListAsync();
        }

        public async Task<Articoli> GetArticoloByCodice(string codice)
        {
            return await this.nanshopDbContext.Articoli
            .Where (art => art.CodArt.Equals(codice))
            .Include(a => a.Barcode)
            .Include(f => f.FamAssort)
            .FirstOrDefaultAsync();
        }

        public Articoli GetArticoloByCodice2(string codice)
        {
            return this.nanshopDbContext.Articoli
           .Where(art => art.CodArt.Equals(codice))
           .Include(a => a.Barcode)
           .Include(f => f.FamAssort)
           .FirstOrDefault();
        }
        public async Task<Articoli> GetArticoloByEan(string ean)
        {
            var param = new SqlParameter("@Barcode", ean);

            string Sql = "SELECT A.* FROM [dbo].[ARTICOLI] A JOIN [dbo].[BARCODE] B ";
            Sql += "ON A.CODART = B.CODART WHERE B.BARCODE = @Barcode";

            return await this.nanshopDbContext.Articoli
                .FromSqlRaw(Sql, param)
                .Include(a => a.FamAssort)
                .Include(a => a.Barcode)
                .Include(a => a.Iva)
                .FirstOrDefaultAsync();
        }
        public bool InsertArticolo(Articoli articolo)
        {
            this.nanshopDbContext.Add(articolo);
            return Salva().Item1;
        }

        public Tuple<bool, int> Salva()
        {
            var numberOfEntities = this.nanshopDbContext.SaveChanges();
            return new Tuple<bool,int>(numberOfEntities >= 0 ? true : false, numberOfEntities);
        }

        public bool UpdateArticolo(Articoli articolo)
        {
            this.nanshopDbContext.Articoli.Update(articolo);
            return Salva().Item1;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticoliWebService.Models;
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
            throw new System.NotImplementedException();
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

        public async Task<Articoli> GetArticoloByEan(string ean)
        {
            return await this.nanshopDbContext.Barcode
            .Where(bc => bc.Barcode.Equals(ean))
            .Select(ar => ar.Articolo)
            .FirstOrDefaultAsync();
        }

        public bool InsertArticolo(Articoli articolo)
        {
            throw new System.NotImplementedException();
        }

        public bool Salva()
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateArticolo(Articoli articolo)
        {
            throw new System.NotImplementedException();
        }
    }
}
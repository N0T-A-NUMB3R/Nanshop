using System.Collections.Generic;
using System.Linq;
using ArticoliWebService.Models;

namespace ArticoliWebService.Services.Stores
{
    public class ArticoliStore : IArticoliStore
    {
        NanshopDbContext nanshopDbContext{get; set;}
        public ArticoliStore(NanshopDbContext nanshopDbContext)
        {
            this.nanshopDbContext = nanshopDbContext;
        }
        public bool ArticoloExists(string code)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteArticolo(Articoli articolo)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Articoli> GetArticoliByDescr(string descrizione)
        {
            return this.nanshopDbContext.Articoli
            .Where(art => art.Descrizione.Contains(descrizione))
            .OrderBy(art => art.Descrizione)
            .ToList();
        }

        public Articoli GetArticoloByCodice(string codice)
        {
            return this.nanshopDbContext.Articoli
            .FirstOrDefault(art => art.CodArt.Equals(codice));
        }

        public Articoli GetArticoloByEan(string ean)
        {
            return this.nanshopDbContext.Barcode
            .Where(bc => bc.Barcode.Equals(ean))
            .Select(ar => ar.Articolo)
            .FirstOrDefault();
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
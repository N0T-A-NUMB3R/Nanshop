using System.Collections.Generic;
using ArticoliWebService.Models;

namespace ArticoliWebService.Services.Stores
{
    public interface IArticoliStore
    {
        ICollection<Articoli> GetArticoliByDescr(string descrizione);

        Articoli GetArticoloByCodice(string codice);

        Articoli GetArticoloByEan(string ean);

        bool InsertArticolo(Articoli articolo);

        bool UpdateArticolo(Articoli articolo);

        bool DeleteArticolo (Articoli articolo);

        bool Salva();

        bool ArticoloExists(string code);
        
    } 
}
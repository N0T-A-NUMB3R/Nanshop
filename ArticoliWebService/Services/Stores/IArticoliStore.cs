using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArticoliWebService.Models;

namespace ArticoliWebService.Services.Stores
{
    public interface IArticoliStore
    {
        Task<ICollection<Articoli>> GetArticoliByDescr(string descrizione);

        Task<Articoli> GetArticoloByCodice(string codice);

        Articoli GetArticoloByCodice2(string codice);

        Task<Articoli> GetArticoloByEan(string ean);

        bool InsertArticolo(Articoli articolo);

        bool UpdateArticolo(Articoli articolo);

        bool DeleteArticolo (Articoli articolo);

        Tuple<bool, int> Salva();

        Task<bool> ArticoloExists(string code);
        
    } 
}
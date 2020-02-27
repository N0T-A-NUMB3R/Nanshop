using System;
using System.Collections.Generic;

namespace ArticoliWebService.Dtos
{
    public class ArticoliDTO
    {
        public string CodArt { get; set; }
        public string Descrizione { get; set; }
        public string Um { get; set; }
        public string CodStat { get; set; }
        public Int16? PzCart { get; set; }
        public double? PesoNetto { get; set; }
        public DateTime? DataCreazione { get; set; }

        public ICollection<BarcodeDTO> Ean {get; set;}

        public FamAssortDTO FamAssort{get; set;}

        public string Categoria {get;set;}
    }
}
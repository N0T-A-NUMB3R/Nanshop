using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ArticoliWebService.Models
{
    public class FamAssort
    {
        [Key]
        public int id { get; set; }
        [MinLength(5, ErrorMessage = " Il numero minimo di caratteri è 5")]
        [MaxLength(80, ErrorMessage = "Il numero massimo di caratteri è 80")]
        public string Descrizione { get; set; }
        public virtual ICollection<Articoli> Articoli { get; set; }
    }
}
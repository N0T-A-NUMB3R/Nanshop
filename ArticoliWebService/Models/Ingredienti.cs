using System.ComponentModel.DataAnnotations;

namespace ArticoliWebService.Models
{
    public class Ingredienti
    {
        [Key]
        public string CodArt { get; set; }
        [MinLength(5, ErrorMessage = " Il numero minimo di caratteri è 5")]
        [MaxLength(80, ErrorMessage = "Il numero massimo di caratteri è 80")]
        public string Info{ get; set; }
        public virtual Articoli Articolo { get; set; }
    }
}
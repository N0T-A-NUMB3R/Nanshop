using System.ComponentModel.DataAnnotations;

namespace ArticoliWebService.Models
{
    public class Ean
    {
        [MinLength(5, ErrorMessage = " Il numero minimo di caratteri è 5")]
        [MaxLength(30, ErrorMessage = "Il numero massimo di caratteri è 30")]
        public string CodArt { get; set; }
        [MinLength(8, ErrorMessage = " Il numero minimo di caratteri è 8")]
        [MaxLength(13, ErrorMessage = "Il numero massimo di caratteri è 13")]
        [Key]
        public string Barcode { get; set; }
        [MinLength(5, ErrorMessage = " Il numero minimo di caratteri è 5")]
        [MaxLength(30, ErrorMessage = "Il numero massimo di caratteri è 30")]
        [Required]
        public string IdTipoArt { get; set; }
        public virtual Articoli articolo {get;set;}
    }
}
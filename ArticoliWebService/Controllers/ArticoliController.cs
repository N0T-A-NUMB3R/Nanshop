using System.Collections.Generic;
using System.Threading.Tasks;
using ArticoliWebService.Dtos;
using ArticoliWebService.Models;
using ArticoliWebService.Services.Stores;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ArticoliWebService.Controllers
{
    [ApiController]
    [DisableCors]
    [Produces("application/json")]
    [Route("api/articoli")]
    public class ArticoliController : Controller
    {
        private readonly IArticoliStore articoliStore;

        public ArticoliController(IArticoliStore articoliStore)
        {
            this.articoliStore = articoliStore;
        }

        [HttpGet("cerca/descrizione/{filter}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ArticoliDTO>))]
        public async Task<IActionResult> GetArticoliByDesc (string filter)
        {
            var articoliDTO = new List<ArticoliDTO>();
            var articoli = await articoliStore.GetArticoliByDescr(filter);
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (articoli.Count == 0)
            {
                return NotFound(string.Format("Non è stato trovato alcun articolo con il filtro '{0}'", filter));
            }
            foreach (var articolo in articoli)
            {
                articoliDTO.Add(new ArticoliDTO
                {
                    CodArt = articolo.CodArt,
                    Descrizione = articolo.Descrizione,
                    Um = articolo.Um,
                    CodStat = articolo.CodStat,
                    PzCart = articolo.PzCart,
                    PesoNetto = articolo.PesoNetto,
                    DataCreazione = articolo.DataCreazione,
                    IdStatoArt = articolo.IdStatoArt,
                });
            }

            return Ok(articoliDTO);
        }
        [HttpGet("cerca/codice/{codice}", Name = "GetArticolo")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ArticoliDTO))]
        public async Task<IActionResult> GetArticoloByCodice(string codice)
        {
            if (!(await this.articoliStore.ArticoloExists(codice)))
            {
                return NotFound(string.Format("Non è stato trovato l'articolo con codice '{0}'", codice));
            }

            var articolo = await articoliStore.GetArticoloByCodice(codice);
            var barcodeDTO = new List<BarcodeDTO>();
            
            var famAssortDTO = new FamAssortDTO{
                id = articolo.FamAssort.id,
                Descrizione = articolo.FamAssort.Descrizione
            };

            foreach (var ean in articolo.Barcode)
            {
                barcodeDTO.Add(new BarcodeDTO
                {
                    Barcode = ean.Barcode,
                    Tipo = ean.IdTipoArt
                });
            }

            var articoloDTO = new ArticoliDTO 
            {
                CodArt = articolo.CodArt,
                Descrizione = articolo.Descrizione,
                Um = articolo.Um,
                CodStat = articolo.CodStat,
                PzCart = articolo.PzCart,
                PesoNetto = articolo.PesoNetto,
                DataCreazione = articolo.DataCreazione,
                IdStatoArt = articolo.IdStatoArt,
                Ean = barcodeDTO,
                FamAssort = famAssortDTO
            };

            return Ok(articoloDTO);
        }
        [HttpGet("cerca/ean/{ean}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ArticoliDTO))]
        public async Task<IActionResult> GetArticoloByEan(string ean)
        {
            var articoliDTO = new List<ArticoliDTO>();
            var articolo = await articoliStore.GetArticoloByEan(ean);

            if (articolo == null)
            {
                return NotFound(string.Format("Non è stato trovato l'articolo con ean '{0}'", ean));
            }   
            var articoloDTO = new ArticoliDTO
            {
                CodArt = articolo.CodArt,
                Descrizione = articolo.Descrizione,
                Um = articolo.Um,
                CodStat = articolo.CodStat,
                PzCart = articolo.PzCart,
                PesoNetto = articolo.PesoNetto,
                DataCreazione = articolo.DataCreazione,
                IdStatoArt = articolo.IdStatoArt
                
            };

            return Ok(articoloDTO);
        }

        [HttpPost("inserisci")]
        [ProducesResponseType(201, Type = typeof(Articoli))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostArticoli ([FromBody] Articoli articolo)
        {
            if (articolo == null)
            {
                return BadRequest(ModelState);
            }
            var isPresent = await articoliStore.GetArticoloByCodice(articolo.CodArt);
            if (isPresent != null)
            {
               ModelState.AddModelError("", $"Articolo {articolo.CodArt} già presente in anagrafica.");
               return StatusCode(422,ModelState); 
            }
            if (!ModelState.IsValid)
            {
                var errVal = "";
                foreach(var ms in ModelState.Values)
                {
                    foreach (var error in ms.Errors)
                    {
                        errVal += error.ErrorMessage + "|";
                    }
                }
                return BadRequest(errVal);
            }
            
            if(!(articoliStore.InsertArticolo(articolo)))
            {
                ModelState.AddModelError("", $"Ci sono stati problemi nell'inserimento dell'articolio:{articolo.CodArt}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute ("GetArticolo", new {codice = articolo.CodArt}, articolo);
        }
        
    }
}
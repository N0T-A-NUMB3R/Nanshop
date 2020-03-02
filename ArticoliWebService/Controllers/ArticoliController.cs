using System;
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

        private ArticoliDTO CreateArticoloDTO(Articoli articolo)
        {
            var barcodeDTO = new List<BarcodeDTO>();
            if (articolo.Barcode != null)
            {
                foreach (var ean in articolo.Barcode)
                {
                    barcodeDTO.Add(new BarcodeDTO
                    {
                        Barcode = ean.Barcode,
                        Tipo = ean.IdTipoArt
                    });
                }
            }
            
            return new ArticoliDTO
            {
                CodArt = articolo.CodArt,
                Descrizione = articolo.Descrizione,
                Um = (articolo.Um != null) ? articolo.Um.Trim() : "",
                CodStat = (articolo.CodStat != null) ? articolo.CodStat.Trim() : "",
                PzCart = articolo.PzCart,
                PesoNetto = articolo.PesoNetto,
                DataCreazione = articolo.DataCreazione,
                Ean = barcodeDTO,
                IdFamAss = articolo.IdFamAss,
                IdStatoArt = (articolo.IdStatoArt != null) ? articolo.IdStatoArt.Trim() : "",
                IdIva = articolo.IdIva,
                Categoria = (articolo.FamAssort != null) ? articolo.FamAssort.Descrizione : "Non Definito"
            };
        }

        [HttpGet("cerca/descrizione/{filter}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ArticoliDTO>))]
        public async Task<IActionResult> GetArticoliByDesc(string filter)
        {
            var articoliDTO = new List<ArticoliDTO>();
            var articoli = await articoliStore.GetArticoliByDescr(filter);

            if (!ModelState.IsValid)
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
                    Categoria = (articolo.FamAssort != null) ? articolo.FamAssort.Descrizione : null
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

            return Ok(CreateArticoloDTO(articolo));
        }

        [HttpGet("cerca/ean/{ean}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ArticoliDTO))]
        public async Task<IActionResult> GetArticoloByEan(string ean)
        {
            var articolo = await articoliStore.GetArticoloByEan(ean);

            if (articolo == null)
            {
                return NotFound(string.Format("Non è stato trovato l'articolo con ean '{0}'", ean));
            }   
           
            return Ok(CreateArticoloDTO(articolo));
        }

        [HttpPost("inserisci")]
        [ProducesResponseType(201, Type = typeof(Articoli))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult PostArticolo ([FromBody] Articoli articolo)
        {
            if (articolo == null)
            {
                return BadRequest(ModelState);
            }
            var isPresent = articoliStore.GetArticoloByCodice2(articolo.CodArt);
            
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
            //return CreatedAtRoute ("GetArticolo", new {codice = articolo.CodArt}, articolo);
            return Ok(new InfoMsg(DateTime.Today, $"Inserimento articolo {articolo.CodArt} eseguita con successo!"));
        }

        [HttpPost("modifica")]
        [ProducesResponseType(201, Type = typeof(InfoMsg))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutArticolo([FromBody] Articoli articolo)
        {
            if (articolo == null)
            {
                return BadRequest(ModelState);
            }
            var isPresent = await articoliStore.GetArticoloByCodice(articolo.CodArt);
            if (isPresent == null)
            {
                ModelState.AddModelError("", $"Articolo {articolo.CodArt} NON è presente in anagrafica.");
                return StatusCode(422, ModelState);
            }
            if (!(articoliStore.UpdateArticolo(articolo)))
            {
                ModelState.AddModelError("", $"Ci sono stati problemi nella modifica dell'articolio:{articolo.CodArt}");
                return StatusCode(500, ModelState);
            }
            var returnMessage = new InfoMsg(DateTime.Now, $"Modifica articolo {articolo.CodArt} eseguita con successo.");
            return Ok(returnMessage);

        }

        [HttpDelete("elimina/{codice}")]
        [ProducesResponseType(201, Type = typeof(InfoMsg))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        [ProducesResponseType(422, Type = typeof(InfoMsg))]
        [ProducesResponseType(500)]
        public IActionResult DeleteArticolo(string codice)
        {
            if (codice == "")
            {
                return BadRequest(new InfoMsg(DateTime.Today, $"E' necessario inserire il codice dell'articolo da eliminare!"));
            }

            //Contolliamo se l'articolo è presente (Usare il metodo senza Traking)
            var articolo = articoliStore.GetArticoloByCodice2(codice);

            if (articolo == null)
            {
                return StatusCode(422, new InfoMsg(DateTime.Today, $"Articolo {codice} NON presente in anagrafica! Impossibile Eliminare!"));
            }

            //verifichiamo che i dati siano stati regolarmente eliminati dal database
            if (!articoliStore.DeleteArticolo(articolo))
            {
                return StatusCode(500, new InfoMsg(DateTime.Today, $"Ci sono stati problemi nella eliminazione dell'Articolo {articolo.CodArt}.  "));
            }

            return Ok(new InfoMsg(DateTime.Today, $"Eliminazione articolo {codice} eseguita con successo!"));

        }
    }
}
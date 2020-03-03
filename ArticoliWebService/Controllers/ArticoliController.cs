using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArticoliWebService.Dtos;
using ArticoliWebService.Models;
using ArticoliWebService.Services.Stores;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArticoliWebService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/articoli")]
    public class ArticoliController : Controller
    {
        private readonly IArticoliStore articoliStore;
        private readonly IMapper mapper;

        public ArticoliController(IArticoliStore articoliStore, IMapper mapper)
        {
            this.articoliStore = articoliStore;
            this.mapper = mapper;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ArticoliDTO>))]
        public async Task<ActionResult<IEnumerable<ArticoliDTO>>> GetArticoliByDesc(string filter,
        [FromQuery] string idCat)
        {
            //utilizzamdo ActionResult<T> anziche IasctionRes per ottimizzare i tempi
            var articoliDTO = new List<ArticoliDTO>();
            var articoli = await articoliStore.GetArticoliByDescr(filter, idCat);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (articoli.Count == 0)
            {
                return NotFound(string.Format("Non è stato trovato alcun articolo con il filtro '{0}'", filter));
            }
           
            return Ok(mapper.Map<IEnumerable<ArticoliDTO>>(articoli));
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

            if (!ModelState.IsValid)
            {
                var errVal = "";
                foreach (var ms in ModelState.Values)
                {
                    foreach (var error in ms.Errors)
                    {
                        errVal += error.ErrorMessage + "|";
                    }
                }
                return BadRequest(errVal);
            }

            var isPresent = articoliStore.GetArticoloByCodice2(articolo.CodArt);
            
            if (isPresent != null)
            {
               return StatusCode(422, new InfoMsg(DateTime.Today, $"Articolo {articolo.CodArt} presente in anagrafica! Impossibile utilizzare il metodo POST!")); 
            }

            articolo.DataCreazione = DateTime.Today;
            
            if(!(articoliStore.InsertArticolo(articolo)))
            {
                return StatusCode(500, new InfoMsg(DateTime.Today, $"Ci sono stati problemi nell'inserimento dell'Articolo {articolo.CodArt}."));
            }

            return Ok(new InfoMsg(DateTime.Today, $"Inserimento articolo {articolo.CodArt} eseguita con successo!"));
        }

        [HttpPost("modifica")]
        [ProducesResponseType(201, Type = typeof(InfoMsg))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult PutArticolo([FromBody] Articoli articolo)
        {
            if (articolo == null)
            {
                return BadRequest(ModelState);
            }
            
            if (!ModelState.IsValid)
            {
                string ErrVal = "";

                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        ErrVal += modelError.ErrorMessage + " - ";
                    }
                }

                return BadRequest(new InfoMsg(DateTime.Today, ErrVal));
            }
            var isPresent = articoliStore.GetArticoloByCodice(articolo.CodArt);
            
            if (isPresent == null)
            {
                return StatusCode(422, new InfoMsg(DateTime.Today, $"Articolo {articolo.CodArt} NON presente in anagrafica! Impossibile utilizzare il metodo PUT!"));
            }

            if (!(articoliStore.UpdateArticolo(articolo)))
            {
                return StatusCode(500, new InfoMsg(DateTime.Today, $"Ci sono stati problemi nella modifica dell'Articolo {articolo.CodArt}.  "));
            }

            return Ok(new InfoMsg(DateTime.Today, $"Modifica articolo {articolo.CodArt} eseguita con successo!"));

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

            //Contolliamo se l'articolo è presente (Usare il metodo senza Tracking)
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
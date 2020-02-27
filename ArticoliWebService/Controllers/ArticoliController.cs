using System.Collections.Generic;
using ArticoliWebService.Models;
using ArticoliWebService.Services.Stores;
using Microsoft.AspNetCore.Mvc;

namespace ArticoliWebService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/articoli")]
    public class ArticoliController : Controller
    {
        private IArticoliStore articoliStore {get; set;}

        public ArticoliController(IArticoliStore articoliStore)
        {
            this.articoliStore = articoliStore;
        }

        [HttpGet("cerca/descrizione/{filter}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Articoli>))]
        public IActionResult GetArticoliByDesc (string filter)
        {
            var articoli = articoliStore.GetArticoliByDescr(filter);
            return Ok(articoli);
        }
    }
}
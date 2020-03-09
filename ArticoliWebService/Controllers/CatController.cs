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
    [Route("api/cat")]
    public class CatController : Controller
    {
        private readonly IArticoliStore articoliStore;
        private readonly IMapper mapper;

        public CatController(IArticoliStore articoliStore, IMapper mapper)
        {
            this.articoliStore = articoliStore;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FamAssortDTO>))]
        public async Task<IActionResult> GetCat()
        {
            var listToReturn = new List<FamAssortDTO>();
            var iva = await this.articoliStore.SelCat();
            foreach (var elem in iva)
            {
                listToReturn.Add(new FamAssortDTO
                {
                    id = elem.id,
                    Descrizione = elem.Descrizione
                });
            }
            return Ok(listToReturn);

        }
    }
}
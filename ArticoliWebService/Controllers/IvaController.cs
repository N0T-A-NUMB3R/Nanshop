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
    [Route("api/iva")]
    public class IvaController : Controller
    {
        private readonly IArticoliStore articoliStore;


        public IvaController(IArticoliStore articoliStore, IMapper mapper)
        {
            this.articoliStore = articoliStore;    
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<IvaDTO>))]
        public async Task<IActionResult> GetIva()
        {
            var listToReturn = new List<IvaDTO>();
            var iva = await this.articoliStore.SelIva();
            foreach (var elem in iva)
            {
                listToReturn.Add(new IvaDTO
                {
                    IdIva = elem.IdIva,
                    Descrizione = elem.Descrizione
                });
            }
            return Ok(listToReturn);

        }
    }
}
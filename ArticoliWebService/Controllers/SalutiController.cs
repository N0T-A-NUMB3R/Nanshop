using Microsoft.AspNetCore.Mvc;

namespace ArticoliWebService.Controllers
{
    [ApiController]
    [Route("api/saluti")]
    public class SalutiController
    {
        [HttpGet]
        public string getSaluti()
        {
            return "Saluti";
        }

        [HttpGet("{nome}")]
        public string getSaluti2(string nome) =>  $"Saluti, {nome} sono il primo ws";
    }

}
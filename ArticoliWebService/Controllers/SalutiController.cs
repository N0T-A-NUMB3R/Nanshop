using System;
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
        public string getSaluti2(string nome)
        {
            try
            {
                if(nome == "Marco")
                {
                    throw new Exception("\"Errore: l'utente Marco è disabilitato\"");
                }
                else
                {
                    return string.Format("\"Saluti, {0} sono il tuo web service\"", nome);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
           
      
    }

}
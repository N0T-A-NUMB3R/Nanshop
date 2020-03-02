using System.Threading.Tasks;
using Xunit;
using ArticoliWebService.Controllers;
using ArticoliWebService.Services.Stores;
using ArticoliWebService.Dtos;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ArticoliWebService.Models;
using System;

namespace ArticoliWebService.test
{
    public class ArticoliControllerTestCrud
    {
        private Articoli CreateArtTest()
        {
            // Creazione Articolo
            var Articolo = new Articoli() {CodArt = "NANANAN", Descrizione = "Articolo Test Inserimento", 
                Um = "PZ", CodStat = "TESTART", PzCart = 6, PesoNetto = 1.750, IdIva = 10, IdFamAss = 1, 
                IdStatoArt = "1", DataCreazione = DateTime.Today};

            //Creazione Barcode 
            List<Ean> Barcodes = new List<Ean>();
            var Barcode = new Ean {CodArt="NANANAN", Barcode = "00000000", IdTipoArt = "CP"};
            Barcodes.Add(Barcode);

            //Passiamo il Barcode all'Articolo
            Articolo.Barcode = Barcodes;

            return Articolo;
        }


        [Fact]
        public void TestPostArticolo()
        {
            // Arrange
            var dbContext = DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext));

            // Act
            var response = controller.PostArticolo(this.CreateArtTest()) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Inserimento articolo NANANAN eseguita con successo!", value.Message);
        }

        [Fact]
        public void TestPostArticoloKO()
        {
            // Arrange
            var dbContext = DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext));

            // Act
            var response = controller.PostArticolo(this.CreateArtTest()) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(422, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Articolo NANANAN presente in anagrafica! Impossibile utilizzare il metodo POST!", value.Message);
        }

        [Fact]
        public async Task TestPutArticoloAsync()
        {
            // Arrange
            var dbContext = DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext));

            Articoli articolo = this.CreateArtTest();
            articolo.Descrizione = "Articolo Test Inserimento Modificato";

            // Act
            var response = await controller.PutArticolo(articolo) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

             // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Modifica articolo NANANAN eseguita con successo!", value.Message);
        }

        [Fact]
        public void TestDeleteArticolo()
        {
            // Arrange
            var dbContext = DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext));

            // Act
            var response = controller.DeleteArticolo("321ProvaIns") as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

             // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Eliminazione articolo NANANAN eseguita con successo!", value.Message);

        }
    }
}
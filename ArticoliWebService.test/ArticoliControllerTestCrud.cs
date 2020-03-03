using Xunit;
using ArticoliWebService.Controllers;
using ArticoliWebService.Services.Stores;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ArticoliWebService.Models;
using System;
using ArticoliWebService.Test;

namespace ArticoliWebService.test
{
    [TestCaseOrderer("ArticoliWebService.test.AlphabeticalOrderer", "ArticoliWebService.test")]
    public class ArticoliControllerTestCrud
    {
        private Articoli CreateArtTest()
        {
            var Articolo = new Articoli()
            {
                CodArt = "124ProvaIns",
                Descrizione = "Articolo Test Inserimento",
                Um = "PZ",
                CodStat = "TESTART",
                PzCart = 6,
                PesoNetto = 1.750,
                IdIva = 10,
                IdFamAss = 1,
                IdStatoArt = "1",
                DataCreazione = DateTime.Today
            };

            //Creazione Barcode 
            List<Ean> Barcodes = new List<Ean>();
            var Barcode = new Ean { CodArt = "124ProvaIns", Barcode = "85698742", IdTipoArt = "CP" };
            Barcodes.Add(Barcode);

            //Passiamo il Barcode all'Articolo
            Articolo.Barcode = Barcodes;

            return Articolo;
        
        }


        [Fact]
        public void ATestPostArticolo()
        {
            // Arrange
            var dbContext = DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext), MapperMocker.GetMapper());

            // Act
            var response = controller.PostArticolo(this.CreateArtTest()) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Inserimento articolo 124ProvaIns eseguita con successo!", value.Message);
        }

        [Fact]
        public void BTestPostArticoloKO()
        {
            // Arrange
            var dbContext = DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext), MapperMocker.GetMapper());

            // Act
            var response = controller.PostArticolo(this.CreateArtTest()) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

            // Assert
            Assert.Equal(422, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Articolo 124ProvaIns presente in anagrafica! Impossibile utilizzare il metodo POST!", value.Message);
        }

        [Fact]
        public void CTestPutArticolo()
        {
            // Arrange
            var dbContext = DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext),MapperMocker.GetMapper());

            Articoli articolo = this.CreateArtTest();
            articolo.Descrizione = "Articolo Test Inserimento Modificato";

            // Act
            var response = controller.PutArticolo(articolo) as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

             // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Modifica articolo 124ProvaIns eseguita con successo!", value.Message);
        }

        [Fact]
        public void DTestDeleteArticolo()
        {
            // Arrange
            var dbContext = DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext), MapperMocker.GetMapper());

            // Act
            var response = controller.DeleteArticolo("124ProvaIns") as ObjectResult;
            var value = response.Value as InfoMsg;

            dbContext.Dispose();

             // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("Eliminazione articolo 124ProvaIns eseguita con successo!", value.Message);

        }
    }
}
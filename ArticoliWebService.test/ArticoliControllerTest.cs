using System.Threading.Tasks;
using Xunit;
using ArticoliWebService.Controllers;
using ArticoliWebService.Services.Stores;
using ArticoliWebService.Dtos;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ArticoliWebService.Test;

namespace ArticoliWebService.test
{
    public class ArticoliControllerTest
    {
        [Fact]
        public async Task TestGetArticoloByCodice()
        {
            string codArt = "000992601";

            //arrange
            var dbContext = DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext), MapperMocker.GetMapper());

            // Act
            var response = await controller.GetArticoloByCodice(codArt) as ObjectResult;
            var value = response.Value as ArticoliDTO;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal(codArt, value.CodArt);
        }

        [Fact]
        public async Task TestErrGetArticoloByCode()
        {
            string CodArt = "0009926010";

            // Arrange
            var dbContext = DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext), MapperMocker.GetMapper());

            // Act
            var response = await controller.GetArticoloByCodice(CodArt) as ObjectResult;
            var value = response.Value as ArticoliDTO;

            dbContext.Dispose();

            // Assert
            Assert.Equal(404, response.StatusCode);
            Assert.Null(value);
            Assert.Equal("Non è stato trovato l'articolo con codice '0009926010'", response.Value);
        }

        [Fact]
        public async Task TestSelArticoliByDescrizione()
        { // System.NullReferenceException : Object reference not set to an instance of an object.
            string Descrizione = "ACQUA ROCCHETTA";

            // Arrange
            var dbContext = DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext), MapperMocker.GetMapper());

            // Act
            var actionResult = await controller.GetArticoliByDesc(Descrizione,"");
            dbContext.Dispose();
            
            var result = actionResult.Result as ObjectResult;
            var value = result.Value as ICollection<ArticoliDTO>;

           

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(value);
            Assert.Equal(3, value.Count);
            Assert.Equal("002001201", value.FirstOrDefault().CodArt);
        }

        [Fact]
        public async Task TestErrSelArticoliByDescrizione()
        {
            string Descrizione = "Pippo";

            // Arrange
            var dbContext = DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext), MapperMocker.GetMapper());

            // Act
            var actionResult = await controller.GetArticoliByDesc(Descrizione, "");
            dbContext.Dispose();

            var result = actionResult.Result as ObjectResult;
            var value = result.Value as ICollection<ArticoliDTO>;

            Assert.Equal(404, result.StatusCode);
            Assert.Null(value);
            Assert.Equal("Non è stato trovato alcun articolo con il filtro 'Pippo'", result.Value);

        }

        [Fact]
        public async Task TestSelArticoloByEan()
        { // System.InvalidCastException : Unable to cast object of type 'System.Int16' to type 'System.Int32'.
            string Ean = "80582533";

            // Arrange
            var dbContext = DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext), MapperMocker.GetMapper());

            // Act
            var response = await controller.GetArticoloByEan(Ean) as ObjectResult;
            var value = response.Value as ArticoliDTO;

            dbContext.Dispose();

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal("000974302", value.CodArt);

        }

        [Fact]
        public async Task TestErrSelArticoloByEan()
        {
            string Ean = "80582533A";

            // Arrange
            var dbContext =DbContextMocker.nanshopDbContext();
            var controller = new ArticoliController(new ArticoliStore(dbContext), MapperMocker.GetMapper());

            // Act
            var response = await controller.GetArticoloByEan(Ean) as ObjectResult;
            var value = response.Value as ArticoliDTO;

            dbContext.Dispose();

            // Assert
            Assert.Equal(404, response.StatusCode);
            Assert.Null(value);
            Assert.Equal("Non è stato trovato l'articolo con ean '80582533A'", response.Value);
        }

    }
}
using ArticoliWebService.Controllers;
using Xunit;

namespace ArticoliWebService.test
{
    public class SalutiControllerTest
    {
        SalutiController salutiController;

        public SalutiControllerTest()
        {
            salutiController = new SalutiController();
        }

        [Fact]
        public void TestGetSaluti()
        {
            var retVal = salutiController.getSaluti();
            var expected = "Saluti";
            Assert.Equal(retVal, expected);
        }

        [Fact]
        public void TestGetSaluti2()
        {
            var retVal = salutiController.getSaluti2("Fabio");
            var expected = "\"Saluti, Fabio sono il tuo web service\"";
            Assert.Equal(retVal, expected);
        }

        [Fact]
        public void TestGetSaluti2Ko()
        {
            var retVal = salutiController.getSaluti2("Marco");
            var expected = "\"Errore: l'utente Marco è disabilitato\"";
            Assert.Equal(retVal, expected);
        }
    }
}
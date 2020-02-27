using ArticoliWebService;
using ArticoliWebService.Services;
using Microsoft.EntityFrameworkCore;

namespace ArticoliWebService.test
{
    public class DbContextMocker
    {
        public static NanshopDbContext nanshopDbContext()
        {
            var connectionString = "Data Source=localhost; Initial Catalog=AlphaShop; Integrated Security=False; User ID=sa; Password=123Stella";

            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<NanshopDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            // Create instance of DbContext
            var dbContext = new NanshopDbContext(options);

            return dbContext;
        }
    }
}
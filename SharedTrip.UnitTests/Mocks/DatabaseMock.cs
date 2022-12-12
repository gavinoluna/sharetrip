using Microsoft.EntityFrameworkCore;
using SharedTrip.Infrastructure.Data;

namespace SharedTrip.UnitTests.Mocks
{
    public static class DatabaseMock
    {
        public static ApplicationDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("SharedTripInMemoryDb")
                    .Options;

                return new ApplicationDbContext(dbContextOptions, false);
            }
        }
    }
}
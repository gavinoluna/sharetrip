using Microsoft.EntityFrameworkCore;
using SharedTrip.Core.Contracts;
using SharedTrip.Infrastructure.Data;

namespace SharedTrip.Core.Services
{
    public class TripService : ITripService
    {
        private readonly ApplicationDbContext context;

        public TripService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> GetCountOfTripsAsync()
            => await this.context
                         .Trips
                         .CountAsync();
    }
}
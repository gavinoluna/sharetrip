using Microsoft.EntityFrameworkCore;
using Quartz;
using SharedTrip.Core.Models.User;
using SharedTrip.Infrastructure.Data;
using SharedTrip.Infrastructure.Data.Entities;
using static SharedTrip.Infrastructure.Data.Constants.DataConstants;

namespace SharedTrip.Core.Quartz.Jobs
{
    public class TripJob : IJob
    {
        private readonly ApplicationDbContext context;

        public TripJob(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var outDatedTrips = await this.context
                .Trips
                .Where(t => t.IsActive == true && DateTime.Compare(t.Date, DateTime.Now) <= 0)
                .ToListAsync();

            try
            {
                foreach (var trip in outDatedTrips)
                {
                    trip.IsActive = false;
                }

                await this.context.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
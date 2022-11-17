using Microsoft.EntityFrameworkCore;
using Quartz;
using SharedTrip.Core.Models.User;
using SharedTrip.Infrastructure.Data;
using SharedTrip.Infrastructure.Data.Entities;

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
            var trips = await this.context
                .Trips
                .Where(t => t.IsActive == true)
                .ToListAsync();

            try
            {
                foreach (var trip in trips)
                {
                    if (DateTime.Compare(trip.Date, DateTime.Now) <= 0)
                    {
                        trip.IsActive = false;
                    }
                }

                await this.context.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
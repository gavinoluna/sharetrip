using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedTrip.Infrastructure.Data.Entities;

namespace SharedTrip.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; } = null!;

        public DbSet<Brand> Brands { get; set; } = null!;

        public DbSet<Colour> Colours { get; set; } = null!;

        public DbSet<Trip> Trips { get; set; } = null!;

        public DbSet<Message> Messages { get; set; } = null!;

        public DbSet<Comment> Comments { get; set; } = null!;

        public DbSet<PopulatedPlace> Cities { get; set; } = null!;

        public DbSet<PassengerTrip> PassengersTrips { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<PassengerTrip>()
                .HasKey(x => new { x.TripId, x.PassengerId });

            builder
                .Entity<Trip>()
                .HasOne(x => x.Driver)
                .WithMany(x => x.DriverTrips)
                .HasForeignKey(x => x.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Message>()
                .HasOne(x => x.Sender)
                .WithMany(s => s.SentMessages)
                .HasForeignKey(s => s.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<Message>()
                .HasOne(x => x.Receiver)
                .WithMany(r => r.ReceivedMessages)
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<Car>()
                .HasOne(x => x.Driver)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.DriverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Trip>()
                .HasOne(x => x.StartDestination)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Trip>()
                .HasOne(x => x.EndDestination)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }
    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedTrip.Infrastructure.Data.Entities;
using SharedTrip.Infrastructure.Data.Seeding.Configuration;

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

        public DbSet<PopulatedPlace> PopulatedPlaces { get; set; } = null!;

        public DbSet<PassengerTrip> PassengersTrips { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //ApplicationUser
            builder
                .Entity<ApplicationUser>()
                .Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            //Comments
            builder
                .Entity<Comment>()
                .HasOne(x => x.Receiver)
                .WithMany(x => x.ReceivedComments)
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Comment>()
                .HasOne(x => x.Creator)
                .WithMany()
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            //Trips
            builder
                .Entity<PassengerTrip>()
                .HasKey(x => new { x.TripId, x.PassengerId });

            builder
                .Entity<Trip>()
                .HasOne(x => x.Driver)
                .WithMany(x => x.DriverTrips)
                .HasForeignKey(x => x.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Trip>()
                .HasOne(x => x.StartDestination)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Trip>()
                .HasOne(x => x.EndDestination)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Trip>()
                .Property(t => t.IsActive)
                .HasDefaultValue(true);

            builder.Entity<Trip>()
                .Property(t => t.IsDeleted)
                .HasDefaultValue(false);

            //Messages
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

            //Cars
            builder
                .Entity<Car>()
                .HasOne(x => x.Driver)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.DriverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<Car>()
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            builder.ApplyConfiguration(new PopulatedPlaceConfiguration());
            builder.ApplyConfiguration(new CarBrandsConfiguration());
            builder.ApplyConfiguration(new ColourConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
namespace SharedTrip.Core.Models.Trip
{
    public class MyTripViewModel
    {
        public int Id { get; set; }

        public string DriverId { get; set; } = null!;

        public string DriverImageUrl { get; set; } = null!;

        public string DriverName { get; set; } = null!;

        public string StartDestination { get; set; } = null!;

        public string EndDestination { get; set; } = null!;

        public string Car { get; set; } = null!;

        public decimal Price { get; set; }

        public string Date { get; set; } = null!;

        public int FreeSeats { get; set; }

        public int AllSeats { get; set; }

        public bool IsActive { get; set; }
    }
}
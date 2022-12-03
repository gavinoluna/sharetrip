namespace SharedTrip.Core.Models.Car
{
    public class AllCarsViewModel
    {
        public int Id { get; set; }

        public string Brand { get; set; } = null!;

        public string Model { get; set; } = null!;

        public string Colour { get; set; } = null!;

        public int Year { get; set; }

        public string DriverName { get; set; } = null!;
    }
}
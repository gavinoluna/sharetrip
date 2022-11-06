namespace SharedTrip.Core.Models.Car
{
    public class ProfileCarViewModel
    {
        public int Id { get; set; }

        public string Brand { get; set; } = null!;

        public string Model { get; set; } = null!;

        public string Colour { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public int Year { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Core.Models.Car
{
    public class CreateTripCarViewModel
    {
        public int Id { get; set; }

        public string Model { get; set; } = null!;
    }
}
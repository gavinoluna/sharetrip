using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SharedTrip.Infrastructure.Data.Constants.DataConstants.Car;

namespace SharedTrip.Infrastructure.Data.Entities
{
    public class Car
    {
        public int Id { get; set; }

        public int BrandId { get; set; }

        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; } = null!;

        [Required]
        [StringLength(ModelMaxLength)]
        public string Model { get; set; } = null!;

        public int ColourId { get; set; }

        [ForeignKey(nameof(ColourId))]
        public Colour Colour { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        public int Year { get; set; }

        public bool Climatronic { get; set; }

        public int CountOfSeats { get; set; }

        [Required]
        public string DriverId { get; set; } = null!;

        [ForeignKey(nameof(DriverId))]
        public ApplicationUser Driver { get; set; } = null!;
    }
}
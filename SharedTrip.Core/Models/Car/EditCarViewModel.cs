using Microsoft.AspNetCore.Http;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.CustomAttributes;
using System.ComponentModel.DataAnnotations;

using static SharedTrip.Infrastructure.Data.Constants.DataConstants.Car;

namespace SharedTrip.Core.Models.Car
{
    public class EditCarViewModel : ICarModel
    {
        public int Id { get; set; }

        public int BrandId { get; set; }

        public IEnumerable<BrandViewModel> Brands { get; set; } = new List<BrandViewModel>();

        [Required]
        [StringLength(ModelMaxLength, MinimumLength = ModelMinLength, ErrorMessage = "{0} must be between {2} and {1} characters")]
        public string Model { get; set; } = null!;

        public int ColourId { get; set; }

        public IEnumerable<ColourViewModel> Colours { get; set; } = new List<ColourViewModel>();

        public IFormFile Image { get; set; } = null!;

        [CarYear]
        public int Year { get; set; }

        public bool Climatronic { get; set; }

        [Range(2, 10)]
        [Display(Name = "Count of Seats")]
        public int CountOfSeats { get; set; }
    }
}
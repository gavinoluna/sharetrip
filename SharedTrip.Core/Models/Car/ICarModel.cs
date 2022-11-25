using Microsoft.AspNetCore.Http;

namespace SharedTrip.Core.Models.Car
{
    public interface ICarModel
    {
        public int BrandId { get; set; }

        public IEnumerable<BrandViewModel> Brands { get; set; }

        public string Model { get; set; }

        public int ColourId { get; set; }

        public IEnumerable<ColourViewModel> Colours { get; set; }

        public IFormFile Image { get; set; }

        public int Year { get; set; }

        public bool Climatronic { get; set; }

        public int CountOfSeats { get; set; }
    }
}
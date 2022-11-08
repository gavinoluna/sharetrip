using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Core.Models.Car
{
    public class BrandViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
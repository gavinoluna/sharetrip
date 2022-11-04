using System.ComponentModel.DataAnnotations;

using static SharedTrip.Infrastructure.Data.Constants.DataConstants.Car;

namespace SharedTrip.Infrastructure.Data.Entities
{
    public class Colour
    {
        public int Id { get; set; }

        [Required]
        [StringLength(ColourMaxLength)]
        public string Name { get; set; } = null!;
    }
}
using System.ComponentModel.DataAnnotations;

using static SharedTrip.Infrastructure.Data.Constants.DataConstants.Car;

namespace SharedTrip.Infrastructure.Data.Entities
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        [StringLength(BrandNameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
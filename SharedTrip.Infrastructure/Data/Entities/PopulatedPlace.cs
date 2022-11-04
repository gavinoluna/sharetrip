using System.ComponentModel.DataAnnotations;

using static SharedTrip.Infrastructure.Data.Constants.DataConstants.PopulatedPlace;

namespace SharedTrip.Infrastructure.Data.Entities
{
    public class PopulatedPlace
    {
        public int Id { get; set; }

        [Required]
        [StringLength(PopulatedPlaceNameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
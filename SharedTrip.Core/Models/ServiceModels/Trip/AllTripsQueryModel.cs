using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Core.Models.Trip.ServiceModels
{
    public class AllTripsQueryModel
    {
        public const int TripsPerPage = 5;

        [Display(Name = "From")]
        public int StartDestinationId { get; set; }

        public IEnumerable<PopulatedPlaceViewModel> StartDestinations { get; set; } = new List<PopulatedPlaceViewModel>();

        [Display(Name = "To")]
        public int EndDestinationId { get; set; }

        public IEnumerable<PopulatedPlaceViewModel> EndDestinations { get; set; } = new List<PopulatedPlaceViewModel>();

        public DateTime Date { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalTripsCount { get; set; }

        public IEnumerable<AllTripsViewModel> Trips { get; set; } = Enumerable.Empty<AllTripsViewModel>();
    }
}
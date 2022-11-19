using SharedTrip.Core.Models.Trip;

namespace SharedTrip.Core.Contracts
{
    public interface ITripService
    {
        Task<TripQueryServiceModel> AllAsync(int startDestinationId, int endDestinationId, DateTime date, int currentPage = 1, int tripsPerPage = 5);

        Task<int> GetCountOfTripsAsync();

        Task<int> CreateTripAsync(CreateTripViewModel model, string driverId);

        Task<IEnumerable<PopulatedPlaceViewModel>> GetPopulatedPlacesAsync();

        Task<bool> CheckWhetherUserIsFree(string userId, DateTime date, int? tripId = null);

        Task<IEnumerable<AllTripsViewModel>> GetMyTripsAsync(string userId);

        Task<TripViewModel> GetTripDetailsAsync(int tripId);

        Task<EditTripViewModel> GetTripForEditAsync(int tripId);

        Task<bool> EditTripAsync(EditTripViewModel model);

        Task<bool> JoinTripAsync(string userId, int tripId);

        Task<bool> DeleteTripAsync(int tripId);
    }
}
using SharedTrip.Core.Models.Trip;
using SharedTrip.Core.Models.Trip.ServiceModels;

namespace SharedTrip.Core.Contracts
{
    public interface ITripService
    {
        Task<TripQueryServiceModel> AllAsync(int startDestinationId, int endDestinationId, DateTime date, int currentPage = 1, int tripsPerPage = 5);

        Task<int> GetCountOfTripsAsync();

        Task<int> CreateTripAsync(CreateTripViewModel model, string driverId);

        Task<IEnumerable<PopulatedPlaceViewModel>> GetPopulatedPlacesAsync();

        Task<bool> CheckWhetherUserIsFree(string userId, DateTime date, int? tripId = null);

        Task<TripQueryServiceModel> GetMyTripsAsync(string userId, int currentPage = 1, int tripsPerPage = 5);

        Task<TripViewModel> GetTripDetailsAsync(int tripId);

        Task<EditTripViewModel> GetTripForEditAsync(int tripId);

        Task EditTripAsync(EditTripViewModel model);

        Task JoinTripAsync(string userId, int tripId);

        Task<bool> CheckIfUserIsInTripAsync(string userId, int tripId);

        Task<bool> IsUserDriverOfTrip(string userId, int tripId);

        Task<bool> TripExists(int tripId);

        Task LeaveTripAsync(string userId, int tripId);

        Task<int> GetCountOfFreeSeatsAsync(int tripId);

        Task DeleteTripAsync(int tripId);
    }
}
using SharedTrip.Core.Models.Trip;

namespace SharedTrip.Core.Contracts
{
    public interface ITripService
    {
        Task<TripQueryServiceModel> AllAsync(int startDestinationId, int endDestinationId, DateTime date, int currentPage = 1, int tripsPerPage = 5);

        Task<int> GetCountOfTripsAsync();

        Task<int> CreateTripAsync(CreateTripViewModel model, string driverId);

        Task<IEnumerable<PopulatedPlaceViewModel>> GetPopulatedPlacesAsync();

        Task<bool> CheckWhetherUserIsFree(string userId, ITrip tripModel, int? tripId = null);

        Task<IEnumerable<MyTripViewModel>> GetMyTripsAsync(string userId);

        Task<TripViewModel> GetTripDetailsAsync(int tripId);

        Task<EditTripViewModel> GetTripForEditAsync(int tripId);

        Task<bool> EditTripAsync(EditTripViewModel model);

        Task<bool> DeleteTripAsync(int tripId);
    }
}
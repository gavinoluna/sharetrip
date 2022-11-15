using SharedTrip.Core.Models.Trip;

namespace SharedTrip.Core.Contracts
{
    public interface ITripService
    {
        Task<int> GetCountOfTripsAsync();

        Task<int> CreateTripAsync(CreateTripViewModel model, string driverId);

        Task<IEnumerable<PopulatedPlaceViewModel>> GetPopulatedPlacesAsync();

        Task<bool> CheckWhetherUserIsFree(string userId, CreateTripViewModel tripModel);

        Task<IEnumerable<MyTripViewModel>> GetMyTripsAsync(string userId);

        Task<TripViewModel> GetTripDetailsAsync(int tripId);
    }
}
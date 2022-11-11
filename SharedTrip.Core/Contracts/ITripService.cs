using SharedTrip.Core.Models.Trip;

namespace SharedTrip.Core.Contracts
{
    public interface ITripService
    {
        Task<int> GetCountOfTripsAsync();

        Task<int> CreateTripAsync(CreateTripViewModel model);

        Task<IEnumerable<PopulatedPlaceViewModel>> GetPopulatedPlacesAsync();
    }
}
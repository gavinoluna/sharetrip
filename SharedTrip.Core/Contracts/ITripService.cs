namespace SharedTrip.Core.Contracts
{
    public interface ITripService
    {
        Task<int> GetCountOfTripsAsync();
    }
}
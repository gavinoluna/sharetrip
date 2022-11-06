using SharedTrip.Core.Models.Car;

namespace SharedTrip.Core.Contracts
{
    public interface ICarService
    {
        Task<IEnumerable<ProfileCarViewModel>> GetMyCarsAsync(string userId);
    }
}
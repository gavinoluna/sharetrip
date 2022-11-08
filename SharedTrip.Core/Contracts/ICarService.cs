using SharedTrip.Core.Models.Car;

namespace SharedTrip.Core.Contracts
{
    public interface ICarService
    {
        Task<IEnumerable<ProfileCarViewModel>> GetMyCarsAsync(string userId);

        Task<bool> AddCarAsync(AddCarViewModel model, string userId);

        Task<IEnumerable<BrandViewModel>> GetBrandsAsync();

        Task<IEnumerable<ColourViewModel>> GetColoursAsync();
    }
}
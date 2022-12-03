using SharedTrip.Core.Models.Car;
using SharedTrip.Core.Models.ServiceModels.Car;

namespace SharedTrip.Core.Contracts
{
    public interface ICarService
    {
        Task<int> CreateCarAsync(CreateCarViewModel model, string userId);

        Task<IEnumerable<BrandViewModel>> GetBrandsAsync();

        Task<IEnumerable<ColourViewModel>> GetColoursAsync();

        Task<CarDetailsViewModel> GetCarAsync(int carId);

        Task<IEnumerable<CreateTripCarViewModel>> GetCarsForTripAsync(string userId);

        Task<CarQueryServiceModel> GetMyCarsAsync(string userId, int currentPage = 1, int carsPerPage = 3);

        Task<EditCarViewModel> GetCarForEditAsync(int carId);

        Task<AllCarsQueryServiceModel> GetAllCarsAsync(int currentPage = 1, int carsPerPage = 6);

        Task EditCarAsync(EditCarViewModel model);

        Task<bool> IsUserOwnerOfACar(string userId, int carId);

        Task<int> GetCountOfCarsAsync();

        Task DeleteAsync(int carId);
    }
}
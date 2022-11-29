using SharedTrip.Core.Models.ServiceModels.User;
using SharedTrip.Core.Models.User;

namespace SharedTrip.Core.Contracts
{
    public interface IUserService
    {
        Task<ProfileViewModel> GetProfileInfoAsync(string userId);

        Task<int> GetCountOfUsersAsync();

        Task<bool> HasCar(string userId);

        Task<TripDriverViewModel> GetTripDriverAsync(string driverId);

        Task<EditUserViewModel> GetUserForEditAsync(string userId);

        Task EditUserAsync(EditUserViewModel model);

        Task<UserQueryServiceModel> GetUsers(int currentPage = 1, int usersPerPage = 6);

        Task<string> GetUserFullNameAsync(string userId);
    }
}
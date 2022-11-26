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

        Task<string> GetUserFullNameAsync(string userId);
    }
}
using SharedTrip.Core.Models.User;

namespace SharedTrip.Core.Contracts
{
    public interface IUserService
    {
        Task<ProfileViewModel> GetProfileInfoAsync(string userId, bool showMoreInfo);

        Task<int> GetCountOfUsersAsync();
    }
}
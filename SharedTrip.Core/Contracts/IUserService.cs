using SharedTrip.Core.Models.User;

namespace SharedTrip.Core.Contracts
{
    public interface IUserService
    {
        Task<MyProfileViewModel> GetMyProfileDataAsync(string userId);
    }
}
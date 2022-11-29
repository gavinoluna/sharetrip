using SharedTrip.Core.Models.User;

namespace SharedTrip.Core.Models.ServiceModels.User
{
    public class UserQueryServiceModel
    {
        public int TotalUsersCount { get; set; }

        public IEnumerable<UserListViewModel> Users { get; set; } = new List<UserListViewModel>();
    }
}
using SharedTrip.Core.Models.User;

namespace SharedTrip.Core.Models.ServiceModels.User
{
    public class AllUsersQueryModel
    {
        public const int UsersPerPage = 6;

        public int CurrentPage { get; set; } = 1;

        public int TotalUsersCount { get; set; }

        public IEnumerable<UserListViewModel> Users { get; set; } = Enumerable.Empty<UserListViewModel>();
    }
}
namespace SharedTrip.Areas.Admin.Models.User
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string[] RoleNames { get; set; }
    }
}
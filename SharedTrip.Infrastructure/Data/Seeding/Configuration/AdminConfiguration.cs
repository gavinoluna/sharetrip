using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedTrip.Infrastructure.Data.Entities;

using static SharedTrip.Infrastructure.Data.Constants.AdminConstants;

namespace SharedTrip.Infrastructure.Data.Seeding.Configuration
{
    public class AdminConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasData(CreateAdmin());
        }

        private ApplicationUser CreateAdmin()
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var admin = new ApplicationUser()
            {
                Id = "bcb4f072-ecca-43c9-ab26-c060c6f364e4",
                Email = AdminEmail,
                NormalizedEmail = AdminEmail,
                UserName = AdminEmail,
                NormalizedUserName = AdminEmail,
                FirstName = "Great",
                LastName = "Admin",
                PhoneNumber = AdminPhoneNumber,
                ProfilePictureUrl = AdminProfilePictureUrl
            };

            admin.PasswordHash =
                 hasher.HashPassword(admin, "!Password123");

            return admin;
        }
    }
}
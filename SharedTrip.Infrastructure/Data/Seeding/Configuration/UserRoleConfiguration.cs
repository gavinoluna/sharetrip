using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SharedTrip.Infrastructure.Data.Seeding.Configuration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        private const string roleId = "0da87dbc-5868-4b99-927d-1366f8c0fdbf";
        private const string concurrencyStamp = "77db1f29-2c45-4a2c-a102-571ceac0a0ab";

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(CreateUserRole());
        }

        private IdentityRole CreateUserRole()
        {
            var role = new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = concurrencyStamp,
                Id = roleId
            };

            return role;
        }
    }
}
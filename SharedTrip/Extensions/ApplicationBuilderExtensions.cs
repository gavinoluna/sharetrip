using Microsoft.AspNetCore.Identity;
using SharedTrip.Infrastructure.Data.Entities;

using static SharedTrip.Infrastructure.Data.Constants.AdminConstants;

namespace SharedTrip.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var services = scopedServices.ServiceProvider;

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRole))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRole };
                    await roleManager.CreateAsync(role);

                    var admin = await userManager.FindByNameAsync(AdminEmail);

                    await userManager.AddToRoleAsync(admin, role.Name);
                })
                .GetAwaiter()
                .GetResult();

            return app;
        }
    }
}
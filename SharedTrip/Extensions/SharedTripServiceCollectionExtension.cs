using CloudinaryDotNet;
using SharedTrip.Core.Contracts;
using SharedTrip.Core.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SharedTripServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton(new Cloudinary(config["CloudinaryString"]));
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ITripService, TripService>();

            return services;
        }
    }
}
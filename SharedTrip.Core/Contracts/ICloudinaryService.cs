using Microsoft.AspNetCore.Http;

namespace SharedTrip.Core.Contracts
{
    public interface ICloudinaryService
    {
        Task<string> UploadPicture(IFormFile picture);
    }
}
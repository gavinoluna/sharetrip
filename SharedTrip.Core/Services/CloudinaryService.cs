using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using SharedTrip.Core.Contracts;

namespace SharedTrip.Core.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploadPicture(IFormFile picture)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(picture.FileName, picture.OpenReadStream()),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            string publicId = uploadResult.JsonObj["public_id"]!.ToString();
            var uploadedImageUrl = await cloudinary.GetResourceAsync(publicId);

            return uploadedImageUrl.SecureUrl;
        }
    }
}
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Rest_ApiVapes.Models;

namespace Rest_ApiVapes.utils
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<CloudinaryUploadResult> UploadImageAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return new CloudinaryUploadResult
            {
                SecureUrl = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId
            };
        }
    }
}

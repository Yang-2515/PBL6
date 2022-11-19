using Booking.Domain.Interfaces;
using Booking.Domain.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace Booking.API.Services
{
    public class PhotoService : ServiceBase
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            var acc = new Account
            (
                CloudinarySettings.CloudName,
                CloudinarySettings.ApiKey,
                CloudinarySettings.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<string> AddItemPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    //Transformation = new Transformation().Crop("fit").Height(200).Width(200) //.Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult.Url.ToString();
        }


    }
}

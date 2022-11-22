using Booking.API.Services;
using Booking.API.ViewModel.Photos.Requests;
using Booking.API.ViewModel.Photos.Response;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [ApiController]
    [Route("api/booking/photos")]
    [Authorize]
    public class PhotoController : ControllerBase
    {
        private readonly PhotoService _photoService;
        public PhotoController(PhotoService photoService)
        {
            _photoService = photoService;
        }
        [HttpPost("upload")]
        public async Task<PhotoResponse> Add([FromForm] UploadPhotoRequest request)
        {
            var uploadFile = new ImageUploadResult();
            if (request.Img != null)
                uploadFile = await _photoService.AddItemPhotoAsync(request.Img);
            return new PhotoResponse
            {
                ImgId = uploadFile.PublicId,
                ImgUrl = uploadFile.Url.ToString(),
            };
        }

        [HttpDelete]
        public async Task Delete([FromBody] DeletePhotoRequest request)
        {
            await _photoService.DeleteImage(request.ImgId);
        }
    }
}

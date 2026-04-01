using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace Infrastructure.Services
{
    public class MediaUploadService
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IWebHostEnvironment _env;
        public MediaUploadService(IHttpContextAccessor httpContext, IWebHostEnvironment env)
        {
            contextAccessor = httpContext;
            _env = env;
        }
        public record MediaUploadResult(string Url, string PhysicalPath);

        public async Task<MediaUploadResult> UploadImageWithPath(IFormFile image, string projectTitle)
        {
            //var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var sanitizedProjectName = string.Join("_", projectTitle.Split(Path.GetInvalidFileNameChars()));
            var fileNameWithoutExt = $"{sanitizedProjectName}_Abdo_Fathy";
            var webpFileName = fileNameWithoutExt + ".webp";
            var webpFilePath = Path.Combine(uploadsFolder, webpFileName);

            using var webPImage = await Image.LoadAsync(image.OpenReadStream());
            await webPImage.SaveAsync(webpFilePath, new WebpEncoder { Quality = 75 });

            var request = contextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var url = $"{baseUrl}/uploads/{webpFileName}";
            return new MediaUploadResult(url, webpFilePath);
        }

        public async Task<MediaUploadResult> UploadVideoWithPath(IFormFile video, string projectTitle)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var sanitizedProjectTitle = string.Join("_", projectTitle.Split(Path.GetInvalidFileNameChars()));
            var fileExtention = Path.GetExtension(video.FileName).ToLower();
            var videoName = $"{sanitizedProjectTitle}_Abdo_Fathy{fileExtention}";
            var filePath = Path.Combine(uploadsFolder, videoName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await video.CopyToAsync(stream);

            var request = contextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var url = $"{baseUrl}/uploads/{videoName}";
            return new MediaUploadResult(url, filePath);
        }

        public Task DeleteIfExistsAsync(string physicalPath)
        {
            if (!string.IsNullOrWhiteSpace(physicalPath) && File.Exists(physicalPath))
                File.Delete(physicalPath);
            return Task.CompletedTask;
        }
    }
}

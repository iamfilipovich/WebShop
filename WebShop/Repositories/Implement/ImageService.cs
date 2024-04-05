using WebShop.Repositories.Implement;
using Stripe;

namespace WebShop.Repositories.Implementation
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        public ImageService(IWebHostEnvironment env)
        {
            _environment = env;
        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var wwwPath = _environment.WebRootPath;
                var uploadsPath = Path.Combine(wwwPath, "Uploads");

                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };

                if (!allowedExtensions.Contains(ext))
                {
                    throw new NotSupportedException("Only JPG, PNG, and JPEG extensions are allowed.");
                }

                var uniqueFileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                return new Tuple<int, string>(1, uniqueFileName);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Consider throwing the exception for better error handling
                return new Tuple<int, string>(0, "An error occurred while saving the image.");
            }
        }


        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = _environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
namespace WebShop.Repositories.Abstract
{
    public interface IImageService
    {
        public Tuple<int, string> SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
    }
}
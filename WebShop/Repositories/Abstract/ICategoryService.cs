using Humanizer.Localisation;

namespace WebShop.Repositories.Abstract
{
    public interface ICategoryService
    {
        bool Add (Category model);
        bool Update (Category model);
        bool Delete (int id);
        Category FindById (int id);
        IEnumerable<Category> GetAll ();
    }
}

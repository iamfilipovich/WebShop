namespace WebShop.Repositories.Abstract
{
    public interface ICategoryService
    {
        bool Add (Category model);
        bool Update (Category model);
        bool Delete (int id);
        bool FindById (int id);
        IEnumerable<Category> GetAll ();
    }
}

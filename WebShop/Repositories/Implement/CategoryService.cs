using Humanizer.Localisation;
using WebShop.Repositories.Abstract;

namespace WebShop.Repositories.Implement
{
    public class CategoryService : ICategoryService
    {
        private readonly WebShopDbContext _db;

        public CategoryService(WebShopDbContext db)
        {
            _db = db;
        }
        public bool Add(Category model)
        {
            try
            {
                _db.Category.Add(model);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.FindById(id);
                if (data == null)
                {
                    return false;
                }
                _db.Category.Remove(data);
                _db.SaveChanges();  
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Category FindById(int id)
        {
            return _db.Category.Find(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _db.Category.ToList();
        }

        public bool Update(Category model)
        {
            try
            {
                _db.Category.Update(model);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}

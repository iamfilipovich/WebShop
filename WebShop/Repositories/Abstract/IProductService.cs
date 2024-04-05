namespace WebShop.Repositories.Abstract
{
    public interface IProductService
    {
        bool Add(Products model);
        bool Update(Products model);
        bool Delete(int id);
        Products FindById (int id);
        IEnumerable<Products> GetAll();
        Task<bool> ReduceQuantity(int productId, int quantity);
    }
}

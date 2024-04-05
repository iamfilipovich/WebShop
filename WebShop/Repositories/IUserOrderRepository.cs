namespace WebShop.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders();
        Task<IEnumerable<(Order order, ApplicationUser user, string productName, int quantity)>> GetAllOrdersWithUser();
    }
}
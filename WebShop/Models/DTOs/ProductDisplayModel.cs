namespace WebShop.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Products> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public string STerm { get; set; } = "";
        public int categoryID { get; set; } = 0;
    }
}

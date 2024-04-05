namespace WebShop.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
    }
}

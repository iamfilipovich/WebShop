global using Microsoft.EntityFrameworkCore.Metadata.Internal;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.ComponentModel.DataAnnotations;
global using Microsoft.AspNetCore.Identity;
using Humanizer.Localisation;

namespace WebShop.Models
{
    public class Products 
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        public string Seller { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
        public int StockQuantity { get; set; }

        //Relations
        public int CategoryID { get; set; }
        [NotMapped]
        public string CategoryName { get; set; }
        public Category Category { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }

        

    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
    }
}

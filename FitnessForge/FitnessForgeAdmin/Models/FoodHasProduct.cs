using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    [Table("food_has_product")]
    public class FoodHasProduct
    {
        [Column("foodId")]
        public int FoodId { get; set; }
        public Food Food { get; set; }
        [Column("productId")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Column("amount")]
        public double Amount { get; set; }
    }
}

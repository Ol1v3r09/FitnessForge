using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    [Table("food")]
    public class Food
    {
        [Key,Column("foodId")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("unitId")]
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Meal> Meals { get; set;}

        public Food()
        {
            Products = new HashSet<Product>();
            Meals = new List<Meal>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    [Table("product")]
    public class Product
    {
        [Key, Column("productId")]
        public int Id { get; set; }
        [Column("brand")]
        public string Brand { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("calorie")]
        public double Calorie { get; set; }
        [Column("carbohydrate")]
        public double Carbohydrate { get; set; }
        [Column("sugar")]
        public double Sugar { get; set; }
        [Column("protein")]
        public double Protein { get; set; }
        [Column("fat")]
        public double Fat { get; set; }
        [Column("saturated_fat")]
        public double SaturatedFat { get; set; }
        [Column("salt")]
        public double Salt { get; set; }
        [Column("fiber")]
        public double Fiber { get; set; }
        [Column("unitId")]
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public ICollection<Food> Foods { get; set; }
        public Product()
        {
            Foods = new HashSet<Food>();
        }
    }
}

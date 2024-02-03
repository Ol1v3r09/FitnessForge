using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    [Table("unit")]
    public class Unit
    {
        [Key, Column("unitId")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Food> Foods { get; set; }
        public Unit()
        {
            Products = new HashSet<Product>();
            Foods = new HashSet<Food>();
        }
    }
}

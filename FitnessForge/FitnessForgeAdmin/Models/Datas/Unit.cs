using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Food> Foods { get; set; }
        public Unit()
        {
            Products = new HashSet<Product>();
            Foods = new HashSet<Food>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public double Calorie { get; set; }
        public double Carbohydrate { get; set; }
        public double Sugar { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public double SaturatedFat { get; set; }
        public double Salt { get; set; }
        public double Fiber { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public ICollection<Food> Foods { get; set; }
        public Product()
        {
            Foods = new HashSet<Food>();
        }
    }
}

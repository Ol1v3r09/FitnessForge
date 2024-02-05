using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    [Table("meal_type")]
    public class MealType
    {
        [Key,Column("meal_typeId")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        public ICollection<Meal> Meals { get; set; }

        public MealType()
        {
            Meals = new List<Meal>();
        }
    }
}

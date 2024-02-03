using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    [Table("meal")]
    public class Meal
    {
        [Key,Column("mealId")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        public ICollection<User_Meal> UserMeals { get; set; }

        public Meal()
        {
            UserMeals = new List<User_Meal>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    [Table("user_meal")]
    public class User_Meal
    {
        [Column("user_mealId")]
        public int Id { get; set; }
        [Column("mealId")]
        public int MealId { get; set; }
        public Meal Meal { get; set; }
        [Column("foodId")]
        public int FoodId { get; set; }
        public Food Food { get; set; }
        [Column("userId")]
        public int UserId { get; set; }
        public User User { get; set; }
        [Column("amount")]
        public double Amount { get; set; }
    }
}

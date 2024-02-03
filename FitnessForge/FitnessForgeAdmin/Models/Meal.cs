using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    [Table("meal")]
    public class Meal
    {
        [Column("mealId")]
        public int Id { get; set; }
        [Column("meal_typeId")]
        public int MealTypeId { get; set; }
        public MealType MealType { get; set; }
        [Column("foodId")]
        public int FoodId { get; set; }
        public Food Food { get; set; }
        [Column("intakeId")]
        public int IntakeId { get; set; }
        public DailyIntake DailyIntake { get; set; }
        [Column("amount")]
        public double Amount { get; set; }
    }
}

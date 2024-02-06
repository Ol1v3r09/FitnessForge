using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    public class Meal
    {
        public int Id { get; set; }
        public int MealTypeId { get; set; }
        public MealType MealType { get; set; }
        public int FoodId { get; set; }
        public Food Food { get; set; }]
        public int IntakeId { get; set; }
        public DailyIntake DailyIntake { get; set; }
        public double Amount { get; set; }
    }
}

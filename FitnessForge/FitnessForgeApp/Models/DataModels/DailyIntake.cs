using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeApp.Models
{
    [Table("daily_intake")]
    public class DailyIntake
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double CalorieIntake { get; set; }
        public double FluidIntake { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public ICollection<Meal> Meals { get; set; }
        public DailyIntake()
        {
            Meals = new List<Meal>();
        }
    }
}

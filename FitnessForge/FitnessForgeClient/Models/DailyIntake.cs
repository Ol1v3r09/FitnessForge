using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    [Table("daily_intake")]
    public class DailyIntake
    {
        [Column("intakeId")]
        public int Id { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("calorie_intake")]
        public double CalorieIntake { get; set; }
        [Column("fluid_intake")]
        public double FluidIntake { get; set; }
        public User User { get; set; }
        [Column("userId")]
        public int UserId { get; set; }
        public ICollection<Meal> Meals { get; set; }
        public DailyIntake()
        {
            Meals = new List<Meal>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    [Table("user")]
    public class User
    {
        [Key, Column("userId")]
        public int Id { get; set; }
        [Column("username")]
        public string Username { get; set;}
        [Column("email")]
        public string Email { get; set;}
        [Column("password")]
        public string Password {private get; set;}
        [Column("age")]
        public int Age { get; set;}
        [Column("sex")]
        public string Sex {  get; set;}
        [Column("weight")]
        public double Weight { get; set;}
        [Column("height")]
        public double Height { get; set; }
        [Column("weight_goal")]
        public double WeightGoal { get; set;}
        [Column("weekly_weight_goal")]
        public double WeeklyWeightGoal { get; set; }
        public ActivityLevel ActivityLevel { get; set;}
        [Column("activityId")]
        public int ActivityId { get; set;}
        public NutrientGoal NutrientGoal { get; set;}
        [Column("nutrientId")]
        public int NutrientId { get;set;}
        public ICollection<DailyIntake> DailyIntakes { get; set;}
        public User()
        {
            DailyIntakes = new HashSet<DailyIntake>();
        }
    }
}

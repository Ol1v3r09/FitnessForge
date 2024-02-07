using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessForgeApp.Models
{
    [Table("user_workout_history")]
    public class UserWorkoutHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }
        public DateTime Date { get; set; }
    }
}

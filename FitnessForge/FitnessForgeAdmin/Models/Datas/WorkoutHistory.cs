using FitnessForgeAdmin.Models;

namespace FitnessForgeApp.Models.Datas
{
    public class WorkoutHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }
        public DateTime Date { get; set; }
    }
}

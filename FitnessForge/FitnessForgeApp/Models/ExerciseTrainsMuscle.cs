using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessForgeApp.Models
{
    [Table("exercise_trains_muscle")]
    public class ExerciseTrainsMuscle
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public int MuscleId { get; set; }
        public Muscle Muscle { get; set; }
    }
}

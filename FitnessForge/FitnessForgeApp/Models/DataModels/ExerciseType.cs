using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessForgeApp.Models
{
    [Table("exercise_type")]
    public class ExerciseType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
        public ExerciseType()
        {
            Exercises = new HashSet<Exercise>();
        }
    }
}

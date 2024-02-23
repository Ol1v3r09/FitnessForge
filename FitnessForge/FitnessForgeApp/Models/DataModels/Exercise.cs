using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessForgeApp.Models
{
    [Table("exercise")]
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Instructions { get; set; }
        public ExerciseType ExerciseType { get; set; }
        public int TypeId { get; set; }
        public ICollection<Muscle> Muscles { get; set; }
        public ICollection<Equipment> Equipments { get; set; }
        public ICollection<Workout> Workouts { get; set; }
        public Exercise()
        {
            Muscles = new HashSet<Muscle>();
            Equipments = new HashSet<Equipment>();
            Workouts = new HashSet<Workout>();
        }
    }
}

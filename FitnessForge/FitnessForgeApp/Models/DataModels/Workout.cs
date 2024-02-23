using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessForgeApp.Models
{
    [Table("workout")]
    public class Workout
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
        public Workout()
        {
            Users = new HashSet<ApplicationUser>();
            Exercises = new HashSet<Exercise>();
        }
    }
}

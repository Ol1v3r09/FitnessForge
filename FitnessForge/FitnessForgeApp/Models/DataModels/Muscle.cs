using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessForgeApp.Models
{
    [Table("muscle")]
    public class Muscle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
        public Muscle()
        {
            Exercises = new HashSet<Exercise>();
        }
    }
}

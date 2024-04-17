using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessForgeApp.Models
{
    [Table("equipment")]
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
        public Equipment()
        {
            Exercises = new HashSet<Exercise>();
        }
    }
}

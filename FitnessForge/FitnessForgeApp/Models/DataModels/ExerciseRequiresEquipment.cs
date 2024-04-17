using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessForgeApp.Models
{
    [Table("exercise_requires_equipment")]
    public class ExerciseRequiresEquipment
    {
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }
    }
}

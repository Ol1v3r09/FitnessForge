namespace FitnessForgeApp.Models.Datas
{
    public class ExerciseRequiresEquipment
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }
    }
}

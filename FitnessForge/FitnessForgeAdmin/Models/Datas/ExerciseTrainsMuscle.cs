namespace FitnessForgeApp.Models.Datas
{
    public class ExerciseTrainsMuscle
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public int MuscleId { get; set; }
        public Muscle Muscle { get; set; }
    }
}

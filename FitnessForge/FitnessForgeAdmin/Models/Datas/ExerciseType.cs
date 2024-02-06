namespace FitnessForgeApp.Models.Datas
{
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

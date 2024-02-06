namespace FitnessForgeApp.Models.Datas
{
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

using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessForgeApp.Models.DataModels
{
    [Table("feedback")]
    public class Feedback
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
    }
}

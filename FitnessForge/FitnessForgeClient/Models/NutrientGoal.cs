using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    [Table("nutrient_goal")]
    public class NutrientGoal
    {
        [Column("nutrientId")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("carbohydrate_percentage")]
        public int CarbohydratePercentage { get; set; }
        [Column("protein_percentage")]
        public int ProteinPercentage { get; set; }
        [Column("fat_percentage")]
        public int FatPercentage {get; set; }
        public ICollection<User> Users { get; set; }
        public NutrientGoal()
        {
            Users = new HashSet<User>();
        }
    }
}

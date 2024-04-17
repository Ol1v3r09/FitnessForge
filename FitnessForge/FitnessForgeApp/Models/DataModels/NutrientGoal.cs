using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeApp.Models
{
    [Table("nutrient_goal")]
    public class NutrientGoal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CarbohydratePercentage { get; set; }
        public int ProteinPercentage { get; set; }
        public int FatPercentage { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public NutrientGoal()
        {
            Users = new HashSet<ApplicationUser>();
        }
    }
}

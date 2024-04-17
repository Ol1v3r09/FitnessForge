using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeApp.Models
{
    [Table("activity_level")]
    public class ActivityLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double BmrMultiplier { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ActivityLevel()
        {
            Users = new HashSet<ApplicationUser>();
        }
    }
}

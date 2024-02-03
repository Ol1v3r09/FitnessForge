using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    [Table("activity_level")]
    public class ActivityLevel
    {
        [Column("activityId")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("bmr_multiplier")]
        public double BmrMultiplier { get; set; }
        public ICollection<User> Users { get; set; }
        public ActivityLevel()
        {
            Users = new HashSet<User>();
        }
    }
}

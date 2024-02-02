using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models
{
    [Table("user")]
    public class User
    {
        [Key, Column("userId")]
        public int Id { get; set; }
        public ICollection<User_Meal> Meals { get; set; }
    }
}

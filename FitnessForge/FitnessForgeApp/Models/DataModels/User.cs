﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateOnly DateOfBirth { get; set; }
        public string? Sex { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public double? WeightGoal { get; set; }
        public double? WeeklyWeightGoal { get; set; }
        public ActivityLevel ActivityLevel { get; set; }
        public int ActivityId { get; set; }
        public NutrientGoal NutrientGoal { get; set; }
        public int NutrientId { get; set; }
        public ICollection<DailyIntake>? DailyIntakes { get; set; }
        public ICollection<Workout>? Workouts { get; set; }
        public ApplicationUser()
        {
            DailyIntakes = new HashSet<DailyIntake>();
            Workouts = new HashSet<Workout>();
        }
    }
}

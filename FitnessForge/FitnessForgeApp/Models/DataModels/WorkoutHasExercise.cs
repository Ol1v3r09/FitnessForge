﻿using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessForgeApp.Models
{
    [Table("workout_has_exercise")]
    public class WorkoutHasExercise
    {
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public int SetCount { get; set; }
        public int RepetitionCount { get; set; }
    }
}

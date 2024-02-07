using Microsoft.EntityFrameworkCore;

namespace FitnessForgeApp.Models.Contexts
{
    public class UserExerciseContext : DbContext
    {
        string connStr = "server=localhost;port=3307;userid=root;database=fitnessforge;";
        public DbSet<Exercise> exercises { get; set; }
        public DbSet<ExerciseTrainsMuscle> exerciseTrainsMuscles { get; set; }
        public DbSet<Workout> workouts { get; set; }
        public DbSet<UserWorkoutHistory> userWorkoutHistory { get; set; }
        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<ExerciseRequiresEquipment> exerciseRequiresEquipment { get; set;}
        public DbSet<Equipment> equipments { get; set; }
        public DbSet<Muscle> muscles { get; set;}
        public DbSet<ExerciseType> exerciseTypes { get; set; }
        public  DbSet<WorkoutHasExercise> workoutHasExercises { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connStr,ServerVersion.AutoDetect(connStr));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().Ignore(d => d.DailyIntakes);
            //User Workout kapcsolat UserWorkoutHistory modellel
            modelBuilder.Entity<Workout>()
                .HasMany(f => f.Users)
                .WithMany(p => p.Workouts)
                .UsingEntity<UserWorkoutHistory>(
                    j => j
                        .HasOne(fp => fp.User)
                        .WithMany()
                        .HasForeignKey(fp => fp.UserId),
                    j => j
                        .HasOne(fp => fp.Workout)
                        .WithMany()
                        .HasForeignKey(fp => fp.WorkoutId)
                );
            //Workout Exercise kapcsolat WorkoutHasExercise modellel
            modelBuilder.Entity<Workout>()
                .HasMany(f => f.Exercises)
                .WithMany(p => p.Workouts)
                .UsingEntity<WorkoutHasExercise>(
                    j => j
                        .HasOne(fp => fp.Exercise)
                        .WithMany()
                        .HasForeignKey(fp => fp.ExerciseId),
                    j => j
                        .HasOne(fp => fp.Workout)
                        .WithMany()
                        .HasForeignKey(fp => fp.WorkoutId)
                );
            //Exercise Equipment kapcsolat ExerciseRequiresEquipment modellel
            modelBuilder.Entity<Exercise>()
                .HasMany(f => f.Equipments)
                .WithMany(p => p.Exercises)
                .UsingEntity<ExerciseRequiresEquipment>(
                    j => j
                        .HasOne(fp => fp.Equipment)
                        .WithMany()
                        .HasForeignKey(fp => fp.EquipmentId),
                    j => j
                        .HasOne(fp => fp.Exercise)
                        .WithMany()
                        .HasForeignKey(fp => fp.ExerciseId)
                );
            //Exercise Muscle kapcsolat ExerciseTrainsMuscle modellel
            modelBuilder.Entity<Exercise>()
                .HasMany(f => f.Muscles)
                .WithMany(p => p.Exercises)
                .UsingEntity<ExerciseTrainsMuscle>(
                    j => j
                        .HasOne(fp => fp.Muscle)
                        .WithMany()
                        .HasForeignKey(fp => fp.MuscleId),
                    j => j
                        .HasOne(fp => fp.Exercise)
                        .WithMany()
                        .HasForeignKey(fp => fp.ExerciseId)
                );
            //ExerciseType Exercise kapcsolat
            modelBuilder.Entity<ExerciseType>()
                .HasMany(e => e.Exercises)
                .WithOne(e => e.ExerciseType)
                .HasForeignKey(e => e.TypeId)
                .IsRequired();
        }
    }
}

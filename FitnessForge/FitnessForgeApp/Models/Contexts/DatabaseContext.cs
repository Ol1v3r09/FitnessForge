using Microsoft.EntityFrameworkCore;

namespace FitnessForgeApp.Models.Contexts
{
    public class DatabaseContext : DbContext
    {
        string connStr = "server=localhost;port=3307;userid=root;database=fitnessforge;";
        public DbSet<Exercise> exercises { get; set; }
        public DbSet<ExerciseTrainsMuscle> exerciseTrainsMuscles { get; set; }
        public DbSet<Workout> workouts { get; set; }
        public DbSet<UserWorkoutHistory> userWorkoutHistory { get; set; }
        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<ExerciseRequiresEquipment> exerciseRequiresEquipment { get; set; }
        public DbSet<Equipment> equipments { get; set; }
        public DbSet<Muscle> muscles { get; set; }
        public DbSet<ExerciseType> exerciseTypes { get; set; }
        public DbSet<WorkoutHasExercise> workoutHasExercises { get; set; }
        public DbSet<Food> foods { get; set; }
        public DbSet<FoodHasProduct> foodsHasProducts { get; set; }
        public DbSet<MealType> mealTypes { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Meal> meals { get; set; }
        public DbSet<Unit> units { get; set; }
        public DbSet<DailyIntake> dailyIntakes { get; set; }
        public DbSet<NutrientGoal> nutrientGoals { get; set; }
        public DbSet<ActivityLevel> activityLevels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            //Unit és Food model kapcsolata
            modelBuilder.Entity<Unit>()
                .HasMany(e => e.Foods)
                .WithOne(e => e.Unit)
                .HasForeignKey(e => e.UnitId)
                .IsRequired();

            //Unit és Product model kapcsolata
            modelBuilder.Entity<Unit>()
                .HasMany(e => e.Products)
                .WithOne(e => e.Unit)
                .HasForeignKey(e => e.UnitId)
                .IsRequired();

            //Food és Product model összekapcsolása a Food_has_Product tábla segítségével
            modelBuilder.Entity<Food>()
                .HasMany(f => f.Products)
                .WithMany(p => p.Foods)
                .UsingEntity<FoodHasProduct>(
                    j => j
                        .HasOne(fp => fp.Product)
                        .WithMany()
                        .HasForeignKey(fp => fp.ProductId),
                    j => j
                        .HasOne(fp => fp.Food)
                        .WithMany()
                        .HasForeignKey(fp => fp.FoodId)
                );

            //MealType Meal model kapcsolata
            modelBuilder.Entity<MealType>()
                .HasMany(e => e.Meals)
                .WithOne(e => e.MealType)
                .HasForeignKey(e => e.MealTypeId)
                .IsRequired();

            //DailyIntake Meal model kapcsolata
            modelBuilder.Entity<DailyIntake>()
                .HasMany(e => e.Meals)
                .WithOne(e => e.DailyIntake)
                .HasForeignKey(e => e.IntakeId)
                .IsRequired();

            //Food Meal model kapcsolata
            modelBuilder.Entity<Food>()
                .HasMany(e => e.Meals)
                .WithOne(e => e.Food)
                .HasForeignKey(e => e.FoodId)
                .IsRequired();

            //User ActivityLevel model kapcsolata
            modelBuilder.Entity<ActivityLevel>()
                .HasMany(e => e.Users)
                .WithOne(e => e.ActivityLevel)
                .HasForeignKey(e => e.ActivityId)
                .IsRequired();

            //User DailyIntake model kapcsolata
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.DailyIntakes)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            //User NutrientGoal model kapcsolata
            modelBuilder.Entity<NutrientGoal>()
                .HasMany(e => e.Users)
                .WithOne(e => e.NutrientGoal)
                .HasForeignKey(e => e.NutrientId)
                .IsRequired();
        }
    }
}

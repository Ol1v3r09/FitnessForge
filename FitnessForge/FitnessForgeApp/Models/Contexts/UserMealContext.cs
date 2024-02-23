using FitnessForgeApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models.Contexts
{
    public class UserMealContext : DbContext
    {
        string connStr = "server=localhost;port=3306;userid=root;database=fitnessforge;password=1234";

        public DbSet<Food> foods { get; set; }
        public DbSet<FoodHasProduct> foodsHasProducts { get; set; }
        public DbSet<MealType> mealTypes { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Meal> meals { get; set; }
        public DbSet<Unit> units { get; set; }
        public DbSet<ApplicationUser> aspnetusers { get; set; }
        public DbSet<DailyIntake> dailyIntakes { get; set; }
        public DbSet<NutrientGoal> nutrientGoals { get; set; }
        public DbSet<ActivityLevel> activityLevels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().Ignore(w => w.Workouts);
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

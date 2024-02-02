using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FitnessForgeAdmin.Models.Contexts
{
    public class MealContext : DbContext
    {
        string connStr = "server=localhost;userid=root;database=fitnessforge;";

        public DbSet<Food> foods { get; set; }
        public DbSet<Food_has_Product> foods_has_products { get; set; }
        public DbSet<Meal> meals { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Unit> units { get; set; }
        public DbSet<User> users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connStr,ServerVersion.AutoDetect(connStr));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                .UsingEntity<Food_has_Product>(
                    j => j
                        .HasOne(fp => fp.Product)
                        .WithMany()
                        .HasForeignKey(fp => fp.ProductId),
                    j => j
                        .HasOne(fp => fp.Food)
                        .WithMany()
                        .HasForeignKey(fp => fp.FoodId)
                );

            //Meal UserMeal model kapcsolata
            modelBuilder.Entity<Meal>()
                .HasMany(e => e.UserMeals)
                .WithOne(e => e.Meal)
                .HasForeignKey(e => e.MealId)
                .IsRequired();

            //User UserMeal model kapcsolata
            modelBuilder.Entity<User>()
                .HasMany(e => e.Meals)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            //Food UserMeal model kapcsolata
            modelBuilder.Entity<Food>()
                .HasMany(e => e.UserMeals)
                .WithOne(e => e.Food)
                .HasForeignKey(e => e.FoodId)
                .IsRequired();
        }
    }
}

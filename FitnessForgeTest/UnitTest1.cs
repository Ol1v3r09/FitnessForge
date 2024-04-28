using FitnessForgeApp.Controllers;
using FitnessForgeApp.Models.ViewModels;
using FitnessForgeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using FitnessForgeApp.Data;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Net.Http;
using System.Security.Claims;

namespace FitnessForgeTest
{
    public class Tests
    {
        private WorkoutController _controller;
        private ApplicationDbContext _mockDbContext;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;

        public Tests()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var _mockUser = new ApplicationUser { Id = "1", UserName = "test" };
            _mockUserManager
                .Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockUser);

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "test") }, "mock"));

            var conn = "server=localhost;user=root;database=fitnessforge;password=1234";
            _mockDbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(conn, ServerVersion.AutoDetect(conn))
                .Options);
            _mockDbContext.Database.EnsureDeleted();
            _mockDbContext.Database.Migrate();

            _controller = new WorkoutController(_mockDbContext, _mockUserManager.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext,
            };

            
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Database_Insert_Test()
        {
            var equipment = new Equipment { Id = 1, Name = "Teszt" };
            var muscle = new Muscle { Id = 1, Name = "Teszt" };
            var exercise_type = new ExerciseType { Id = 1, Name = "Teszt" };
            var exercise = new Exercise { Id = 1, ExerciseType = exercise_type, Instructions = "Teszt", Name = "Teszt", Muscles = new List<Muscle>() { muscle}, Equipments = new List<Equipment>() { equipment} };
            var workout = new Workout { Id = 1, CreatorUserId = "teszt123", Name = "Teszt", Exercises = new List<Exercise>() { exercise } };

            _mockDbContext.equipments.Add(equipment);
            _mockDbContext.muscles.Add(muscle);
            _mockDbContext.exerciseTypes.Add(exercise_type);
            _mockDbContext.exercises.Add(exercise);
            _mockDbContext.workouts.Add(workout);

            _mockDbContext.SaveChanges();

            Assert.IsNotNull( _mockDbContext );
            Assert.IsNotNull(_mockDbContext.equipments );
            Assert.IsNotNull(_mockDbContext.muscles);
            Assert.IsNotNull(_mockDbContext.exerciseTypes);
            Assert.IsNotNull(_mockDbContext.exercises);
            Assert.IsNotNull(_mockDbContext.workouts);
        }

        [Test]
        public async Task DetailsAction_Test()
        {
            var result = await _controller.Details(1, "List");

            Assert.IsNotNull(result);

            var viewResult = (ViewResult)result;

            var model = viewResult.Model;
            Assert.IsNotNull(model);
        }
    }
}
using FitnessForgeApp.Data;
using FitnessForgeApp.Models;
using FitnessForgeApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessForgeApp.Controllers
{
    [Authorize(Roles = "User")]
    public class WorkoutController : Controller
    {
        ApplicationDbContext db;
        UserManager<ApplicationUser> userManager;
        public WorkoutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            var viewModel = new WorkoutListViewModel();
            var currUser = await userManager.GetUserAsync(User);
            viewModel.Workouts = await db.workouts.Where(x => x.CreatorUserId == currUser.Id).ToListAsync();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id, string returnAction)
        {
            try
            {
                var viewModel = new WorkoutDetailsViewModel();
                viewModel.Workout = await db.workouts.FindAsync(id);
                viewModel.WorkoutHasExercises = await db.workoutHasExercises.Where(x => x.WorkoutId == viewModel.Workout.Id).Include(x => x.Exercise).Include(x => x.Workout).ToListAsync();
                viewModel.ReturnAction = returnAction;
                return View(viewModel);
            }catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return Ok();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var viewModel = new WorkoutCreateViewModel();
            viewModel.Exercises = db.exercises.ToList();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(IFormCollection form)
        {
            var name = form["Name"];
            var exerciseIds = form["exerciseId"];
            var setCounts = form["exerciseSetCount"];
            var repCounts = form["exerciseRepCount"];
            var currUser = await userManager.GetUserAsync(User);

            Workout workout = new Workout
            {
                Name = name,
                CreatorUserId = currUser.Id
            };

            await db.workouts.AddAsync(workout);
            await db.SaveChangesAsync();

            for (int i = 0; i < exerciseIds.Count; i++)
            {

                    var exercise = await db.exercises.FindAsync(int.Parse(exerciseIds[i]));
                    if (exercise != null)
                    {
                        var workoutHasExercise = new WorkoutHasExercise
                        {
                            Exercise = exercise,
                            SetCount = int.Parse(setCounts[i]),
                            RepetitionCount = int.Parse(repCounts[i]),
                            Workout = workout
                        };

                        db.workoutHasExercises.Add(workoutHasExercise);
                    }
            }


            await db.SaveChangesAsync();

            return RedirectToAction("List");
        }


        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var workout = db.workouts.Find(id);
            db.workouts.Remove(workout);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> SearchExercise(string search)
        {
            var viewModel = new WorkoutCreateViewModel();
            viewModel.Exercises = await db.exercises.ToListAsync();

            if (search != null)
            {
                viewModel.Exercises = await db.exercises.Where(x => x.Name.Contains(search)).ToListAsync();
            }

            return PartialView("_ExercisePartial", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateList(int selectedValue)
        {
            var viewModel = new WorkoutListViewModel();
            var currUser = await userManager.GetUserAsync(User);
            switch (selectedValue)
            {
                case 0:
                    viewModel.Workouts = await db.workouts.Where(x => x.CreatorUserId == currUser.Id).ToListAsync();
                    break;
                case 1:
                    viewModel.Workouts = await db.workouts.ToListAsync();
                    break;
            }
            return PartialView("_WorkoutPartial", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id)
        {
            var workout = await db.workouts.FindAsync(id);
            var currUser = await userManager.GetUserAsync(User);
            currUser.Workouts.Add(workout);
            workout.Users.Add(currUser);
            await db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> UserWorkouts()
        {
            var viewModel = new UserWorkoutsViewModel();
            var currUser = await userManager.GetUserAsync(User);
            viewModel.userWorkouts = await db.userWorkoutHistory.Where(x => x.UserId == currUser.Id).Include(x => x.Workout).Include(x => x.User).ToListAsync();
            return View(viewModel);
        }
    }
}

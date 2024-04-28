namespace FitnessForgeApp.Models.ViewModels
{
    public class MealEditIntakeViewModel
    {
        public List<Meal> userMeals {  get; set; }
        public string mealType { get; set; }
        public List<FoodHasProduct> userMealsFoodHasProducts { get; set; }
    }
}

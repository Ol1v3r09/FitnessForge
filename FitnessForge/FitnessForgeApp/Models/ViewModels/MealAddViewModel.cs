namespace FitnessForgeApp.Models.ViewModels
{
    public class MealAddViewModel
    {
        public List<FoodHasProduct> allFoodHasProducts { get; set; }
        public List<int> foods {  get; set; }
        public string mealType { get; set; }
    }
}

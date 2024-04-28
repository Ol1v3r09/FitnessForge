
using static System.Net.WebRequestMethods;

namespace FitnessForgeApp.Models.ViewModels
{
    public class MealAddViewModel
    {
        public List<FoodHasProduct> groupedFoodHasProducts { get; set; }
        public List<FoodHasProduct> allFoodHasProducts { get; set; }
        public List<int> foods {  get; set; }
        public string mealType { get; set; }

        public double[] GetFoodDatas(int foodId)
        {
            double[] result = new double[4];
            var fhps = allFoodHasProducts.Where(x => x.FoodId == foodId).ToList();
            foreach (var fhp in fhps) 
            {
                result[0] += fhp.Amount / 100 * fhp.Product.Calorie;
                result[1] += fhp.Amount / 100 * fhp.Product.Carbohydrate;
                result[2] += fhp.Amount / 100 * fhp.Product.Protein;
                result[3] += fhp.Amount / 100 * fhp.Product.Fat;
            }

            return result;
        }

        public double[] GetProductDatas(int productId, int foodId)
        {
            double[] result = new double[4];
            var fhp = allFoodHasProducts.Where(x => x.ProductId == productId && x.FoodId == foodId).First();
            result[0] += fhp.Amount / 100 * fhp.Product.Calorie;
            result[1] += fhp.Amount / 100 * fhp.Product.Carbohydrate;
            result[2] += fhp.Amount / 100 * fhp.Product.Protein;
            result[3] += fhp.Amount / 100 * fhp.Product.Fat;
            return result;
        }
    }
}

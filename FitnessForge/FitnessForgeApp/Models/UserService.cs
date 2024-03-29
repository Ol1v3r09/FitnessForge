﻿using FitnessForgeAdmin.Models.Contexts;

namespace FitnessForgeApp.Models
{
    public class UserService
    {
        static UserMealContext _userMealContext;
        public UserService(UserMealContext userMealContext)
        {
            _userMealContext = userMealContext;
        }

        public double UserDailyCalorie(ApplicationUser user)
        {
            var act = _userMealContext.activityLevels.Where(x => x.Id == user.ActivityId).First();
            switch (user.Sex) 
            {
                case "Férfi":
                    return Convert.ToDouble(((10 * user.Weight) + (6.25 * user.Height) - (5 * UserAge(user)) + 5) * act.BmrMultiplier + (user.WeeklyWeightGoal * 1000));
                case "Nő":
                    return Convert.ToDouble(((10 * user.Weight) + (6.25 * user.Height) - (5 * UserAge(user)) - 161) * act.BmrMultiplier + (user.WeeklyWeightGoal * 1000));
                default:
                    return 0;
            }
        }
        
        public double[] UserDailyNutrients(ApplicationUser user)
        {
            var nutrientGoal = _userMealContext.nutrientGoals.Where(x=> x.Id == user.NutrientId).First();
            return [
                UserDailyCalorie(user) * ((double)nutrientGoal.CarbohydratePercentage / 100) / 4,
                UserDailyCalorie(user) * ((double)nutrientGoal.ProteinPercentage / 100) / 4,
                UserDailyCalorie(user) * ((double)nutrientGoal.FatPercentage / 100) / 8
                ];
        }

        public int UserAge(ApplicationUser user)
        {
            DateTime birthDate = new DateTime(user.DateOfBirth.Year, user.DateOfBirth.Month, user.DateOfBirth.Day);
            TimeSpan time = DateTime.Today - birthDate;
            double age = time.Days / 365.25;
            return int.Parse(Math.Round(age, 0).ToString());
        }
    }
}

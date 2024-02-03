using FitnessForgeAdmin.Models;
using FitnessForgeAdmin.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FitnessForgeAdmin.Controls
{
    /// <summary>
    /// Interaction logic for AddProductControl.xaml
    /// </summary>
    public partial class AddProductControl : UserControl
    {
        UserMealContext db;
        public AddProductControl(UserMealContext context)
        {
            InitializeComponent();
            db = context;
        }

        public void databaseUpdated() {
            cbUnit.Items.Clear();
            var units = (from u in db.units select u).ToList();
            foreach (var unit in units)
            {
                cbUnit.Items.Add(unit.Name);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product{
                Brand = tbBrand.Text,
                Name = tbName.Text,
                Unit = (from u in db.units where u.Name == cbUnit.Text select u).FirstOrDefault(),
                UnitId = (from u in db.units where u.Name == cbUnit.Text select u.Id).FirstOrDefault(),
                Calorie = Convert.ToDouble(tbCalorie.Text),
                Carbohydrate = Convert.ToDouble(tbCarbohydrate.Text),
                Sugar = Convert.ToDouble(tbSugar.Text),
                Protein = Convert.ToDouble(tbProtein.Text),
                Fat = Convert.ToDouble(tbFat.Text),
                SaturatedFat = Convert.ToDouble(tbSaturatedFat.Text),
                Salt = Convert.ToDouble(tbSalt.Text),
                Fiber = Convert.ToDouble(tbFiber.Text)
            };

            db.products.Add(product);
            db.SaveChanges();
        }
    }
}

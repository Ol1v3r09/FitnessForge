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
    /// Interaction logic for AddFoodControl.xaml
    /// </summary>
    public partial class AddFoodControl : UserControl
    {
        class ProductModel 
        {
            public string Name { get; set; }
            public bool isChecked { get; set; }
            public Unit Unit { get; set; }
            public ProductModel(Product p) 
            {
                Name = p.Name;
                isChecked = false;
                Unit = p.Unit;
            }
        }

        MealContext db;
        public AddFoodControl(MealContext context)
        {
            InitializeComponent();
            db = context;
        }

        public void databaseUpdated()
        {
            cbUnits.Items.Clear();
            var prods = (from p in db.products select p).ToList();
            foreach (var p in prods)
            {
                var c = new CheckBox();
                c.Content = p.Name;
            }
            var units = (from u in db.units select u).ToList();
            foreach (var unit in units)
            {
                cbUnits.Items.Add(unit.Name);
            }
            List<ProductModel> products = new List<ProductModel>();
            foreach (var item in (from p in db.products select p).ToList())
            {
                products.Add(new ProductModel(item));
            }
            dgProducts.ItemsSource = products;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Food f = new Food();
            f.Name = tbName.Text;
            f.Unit = (from u in db.units where u.Name == cbUnits.SelectedValue select u).FirstOrDefault();
            f.UnitId = (from u in db.units where u.Name == cbUnits.SelectedValue select u.Id).FirstOrDefault();
            db.foods.Add(f);
            db.SaveChanges();
        }
    }
}

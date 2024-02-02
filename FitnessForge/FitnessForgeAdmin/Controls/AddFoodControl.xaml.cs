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
            public int ProductId;
            public string Brand { get; set; }
            public string Name { get; set; }
            public bool isChecked { get; set; }
            public int Amount { get; set; }
            public Unit Unit { get; set; }
            public ProductModel(Product p) 
            {
                ProductId = p.Id;
                Brand = p.Brand;
                Name = p.Name;
                isChecked = false;
                Amount = 0;
                Unit = p.Unit;
            }
        }

        MealContext db;
        Food f;
        public AddFoodControl(MealContext context)
        {
            InitializeComponent();
            db = context;
            f = new Food();
        }

        public void databaseUpdated()
        {
            cbUnits.Items.Clear();
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

        private void cbContainsProduct_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            ProductModel p = (ProductModel)checkBox.DataContext;
            var product = (from pr in db.products where p.ProductId == pr.Id select pr).FirstOrDefault();
            f.Products.Add(product);
        }

        private void cbContainsProduct_UnChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            ProductModel p = (ProductModel)checkBox.DataContext;
            var product = (from pr in db.products where p.ProductId == pr.Id select pr).FirstOrDefault();
            f.Products.Remove(product);
        }

        private void tbAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            ProductModel p = (ProductModel)textBox.DataContext;
            p.Amount = int.Parse(textBox.Text);
            var product = (from pr in db.products where p.ProductId == pr.Id select pr).FirstOrDefault();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            f.Name = tbName.Text;
            f.Unit = (from u in db.units where u.Name == cbUnits.SelectedValue select u).FirstOrDefault();
            f.UnitId = (from u in db.units where u.Name == cbUnits.SelectedValue select u.Id).FirstOrDefault();
            db.foods.Add(f);
            db.SaveChanges();
        }
    }
}

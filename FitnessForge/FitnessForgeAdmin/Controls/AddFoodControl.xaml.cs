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
        //Osztály ami alapján a Binding működik a DataGrid-ben
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

        UserMealContext db;
        Food f;
        List<ProductModel> products;

        public AddFoodControl(UserMealContext context)
        {
            InitializeComponent();
            db = context;
            f = new Food();
            products = new List<ProductModel>();
        }

        //A UserControl megnyitásakor lefutó frissítés
        public void databaseUpdated()
        {
            cbUnits.Items.Clear();
            var units = (from u in db.units select u).ToList();
            foreach (var unit in units)
            {
                cbUnits.Items.Add(unit.Name);
            }
            foreach (var item in (from p in db.products select p).ToList())
            {
                products.Add(new ProductModel(item));
            }
            dgProducts.ItemsSource = products;
        }

        //Egy Tartalmazza-e oszlop Checkboxának kipipálásakor lefutó event
        private void cbContainsProduct_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            ProductModel p = (ProductModel)checkBox.DataContext;
            var product = (from pr in products where p.ProductId == pr.ProductId select pr).FirstOrDefault();
            product.isChecked = true;
        }

        //Egy Tartalmazza-e oszlop Checkboxának kipipálásának visszavonásakor lefutó event
        private void cbContainsProduct_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            ProductModel p = (ProductModel)checkBox.DataContext;
            var product = (from pr in products where p.ProductId == pr.ProductId select pr).FirstOrDefault();
            product.isChecked = false;
        }

        //Egy Mennyiség textbox szöveg változásakor lefutó event
        private void tbAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            ProductModel p = (ProductModel)textBox.DataContext;
            var product = (from pr in products where p.ProductId == pr.ProductId select pr).FirstOrDefault();
            product.Amount = int.Parse(textBox.Text);
        }

        //Étel mentése az adatbázisba
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Form-ból az adatok kinyerése
            f.Name = tbName.Text;
            f.Unit = (from u in db.units where u.Name == cbUnits.SelectedValue select u).FirstOrDefault();
            f.UnitId = f.Unit.Id;
            //A List<ProductModel> products listán végig menő ciklus ami megnézi hogy ki van e pipálva a termék ha igen akkor kikeresi az adatbázisból a terméket
            //és hozzáadja az étel termékeihez
            foreach (var p in products)
            {
                if (p.isChecked == true)
                {
                    var pr = (from prod in db.products where p.ProductId == prod.Id select prod).FirstOrDefault();
                    if (pr != null) f.Products.Add(pr);
                }
            }
            db.foods.Add(f);
            db.SaveChanges();
            //Kikeresi a bejegyzéseket a kapcsolótáblából és a mennyiségét is beállítja
            foreach (var p in products)
            {
                if (p.isChecked == true)
                {
                    var productFoodRelation = (from fhp in db.foodsHasProducts where fhp.ProductId == p.ProductId && fhp.FoodId == f.Id select fhp).FirstOrDefault();
                    if (productFoodRelation != null) productFoodRelation.Amount = p.Amount;
                }
            }
            db.SaveChanges();
        }
    }
}

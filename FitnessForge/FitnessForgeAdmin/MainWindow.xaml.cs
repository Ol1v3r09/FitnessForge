using FitnessForgeAdmin.Controls;
using FitnessForgeAdmin.Models.Contexts;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FitnessForgeAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MealContext db;

        AddProductControl apc;
        AddUnitControl auc;
        AddFoodControl afc;

        public MainWindow()
        {
            InitializeComponent();
            db = new MealContext();
            db.Database.EnsureCreated();
            apc = new AddProductControl(db);
            auc = new AddUnitControl(db);
            afc = new AddFoodControl(db);
        }

        private void btnDbEnsure_Click(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
        }

        private void miCreateProduct_Click(object sender, RoutedEventArgs e)
        {
            apc.databaseUpdated();
            ccTartalom.Content = apc;
        }

        private void miCreateUnit_Click(object sender, RoutedEventArgs e)
        {
            ccTartalom.Content = auc;
        }

        private void miCreateFood_Click(object sender, RoutedEventArgs e)
        {
            afc.databaseUpdated();
            ccTartalom.Content = afc;
        }
    }
}
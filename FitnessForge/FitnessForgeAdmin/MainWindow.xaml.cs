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
        UserMealContext db;

        AddProductControl apc;
        AddFoodControl afc;

        public MainWindow()
        {
            InitializeComponent();
            db = new UserMealContext();
            db.Database.EnsureCreated();
            apc = new AddProductControl(db);
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

        private void miCreateFood_Click(object sender, RoutedEventArgs e)
        {
            afc.databaseUpdated();
            ccTartalom.Content = afc;
        }
    }
}
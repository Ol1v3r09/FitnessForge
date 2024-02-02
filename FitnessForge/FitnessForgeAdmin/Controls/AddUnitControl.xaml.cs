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
    /// Interaction logic for AddUnitControl.xaml
    /// </summary>
    public partial class AddUnitControl : UserControl
    {
        MealContext db;
        public AddUnitControl(MealContext context)
        {
            InitializeComponent();
            db = context;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var units = (from u in db.units select u).ToList();
            bool database_has_unit = false;
            foreach (var item in units)
            {
                if (item.Name == tbName.Text)
                {
                    database_has_unit = true;
                }
            }
            if (database_has_unit)
            {
                MessageBox.Show("Ez a mértékegység már létezik!","Hiba");
            }
            else
            {
                db.units.Add(new Unit { Name = tbName.Text });
                db.SaveChanges();
            }     
        }
    }
}

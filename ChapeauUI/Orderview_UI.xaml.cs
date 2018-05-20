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
using System.Data;
using ChapeauLogic;
using ChapeauModel;


namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Orderview_UI.xaml
    /// </summary>
    public partial class Orderview_UI : Page
    {
        public Orderview_UI()
        {
            InitializeComponent();
            //Menu menu, int category
            //Testing 
            Item_Service item = new Item_Service();
            MenuCategory menu = MenuCategory.Dinner;
            Dinner dinner = Dinner.Desserts;
            //int number = 4;
            DataTable Table = item.GetMenu(menu, (int)dinner);
            TestView.DataContext = Table.DefaultView;
        }
    }
}

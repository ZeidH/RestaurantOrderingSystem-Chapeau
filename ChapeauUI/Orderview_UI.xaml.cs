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
            //Testing 
            Item_Service item = new Item_Service();
            int number = 4;
            DataTable Table = item.GetItems(number);
            TestView.DataContext = Table.DefaultView;
        }
    }
}

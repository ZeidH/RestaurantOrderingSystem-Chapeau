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
using ChapeauLogic;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Login_UI.xaml
    /// </summary>
    public partial class Login_UI : Page
    {
        public Login_UI()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Order_Service ser = new Order_Service();
            ser.Test();
        }
    }
}

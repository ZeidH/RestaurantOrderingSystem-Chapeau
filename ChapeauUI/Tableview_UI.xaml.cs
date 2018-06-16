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
using ChapeauModel;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Tableview_UI.xaml
    /// </summary>
    public partial class Tableview_UI : Page
    {
        public Tableview_UI(Employee employee)
        {
            InitializeComponent();
        }
        public Tableview_UI()
        {
            InitializeComponent();
        }
    }
}

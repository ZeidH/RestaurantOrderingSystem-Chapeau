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
using System.Windows.Media.Animation;
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
        private Employee employee;
        public Tableview_UI(Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
            Animation.AnimateIn(this, 1);
            GetTables();
            lbl_logged_user.Content = employee.Name;
        }
        public Tableview_UI()
        {
            InitializeComponent();
        }
        private void GetTables()
        {
            table_panel.Children.Clear();
            table_panel.Children.Add(new Table_UC(this));
        }
        internal void GenerateSidePanel(Tafel table)
        {
            table_sidePanel.Children.Clear();
            table_sidePanel.Children.Add(new TableSidePanel(this, table, employee));
            Animation.AnimateSlide(table_sidePanel, 500, 200, 0);
        }
        internal void GenerateCreatePanel(int tableID)
        {
            table_sidePanel.Children.Clear();
            table_sidePanel.Children.Add(new TableSidePanel(this, tableID, employee));
            Animation.AnimateSlide(table_sidePanel, 500, 200, 0);
        }
        public void CloseSidePanel()
        {
            Animation.AnimateSlide(table_sidePanel, 500, 200, -500);
            Table_UC.dispatcherTimer.Stop();
            GetTables();
        }

        private void Btn_logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Logging out", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                Table_UC.dispatcherTimer.Stop();
                NavigationService.Navigate(new Login_UI());
            }
        }
    }
}

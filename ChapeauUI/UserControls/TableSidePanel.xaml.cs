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
using System.Windows.Media.Animation;
using ChapeauLogic;
using ChapeauModel;
using WpfAnimatedGif;
using System.Windows.Threading;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for TableSidePanel_UC.xaml
    /// </summary>
    public partial class TableSidePanel : UserControl
    {
        Tableview_UI ParentPage;
        private Tafel table;
        private Employee employee;
        public TableSidePanel(Tableview_UI ParentPage, Tafel table, Employee employee)
        {
            InitializeComponent();
            this.table = table;
            this.ParentPage = ParentPage;
            this.employee = employee;
            lbl_table.Content = $"Table {table.ID}";
            inner_panel.Children.Add(new OrderList(table.OrderID));
            if (table.Status != TableStatus.Ready)
            {
                btn_Served.Visibility = Visibility.Collapsed;
            }
        }
        public TableSidePanel(Tableview_UI ParentPage, int tableID, Employee employee)
        {
            InitializeComponent();
            this.ParentPage = ParentPage;
            btn_NewOrder.Visibility = Visibility.Collapsed;
            btn_Payment.Visibility = Visibility.Collapsed;
            btn_Served.Visibility = Visibility.Collapsed;
            lbl_table.Content = $"Table {tableID}";
            inner_panel.Children.Add(new OccupyTable(ParentPage, employee.ID, tableID));
        }

        private void Btn_NewOrder_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.NavigationService.Navigate(new Orderview_UI(table.OrderID, table.NumberOfGuests, table.ID, employee));
        }
        private void Btn_Payment_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.NavigationService.Navigate(new Payment_UI(table.OrderID, table.NumberOfGuests, employee));
        }
        private void Btn_ClosePanel_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.CloseSidePanel();
        }
        private void Btn_Served_Click(object sender, RoutedEventArgs e)
        {
            Table_Service logic = new Table_Service();
            logic.SetTableStatus(TableStatus.Busy, table.ID);
            ParentPage.CloseSidePanel();
        }
    }
}

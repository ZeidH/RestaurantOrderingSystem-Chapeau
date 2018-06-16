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
        Page ParentPage;
        private int tableID;
        private int order_id;
        private int guestCount;
        private Employee employee;
        public TableSidePanel(Page ParentPage, int tableID, int order_id, int guestCount, Employee employee)
        {
            InitializeComponent();
            this.order_id = order_id;
            this.tableID = tableID;
            this.employee = employee;
            this.guestCount = guestCount;
            this.ParentPage = ParentPage;
            lbl_table.Content = $"Table {tableID}";
            order_list.Children.Add(new OrderList(order_id));
        }

        private void Btn_NewOrder_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.NavigationService.Navigate(new Orderview_UI(order_id, guestCount, tableID, employee));
        }
        private void Btn_Payment_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.NavigationService.Navigate(new Payment_UI(order_id, guestCount, employee));
        }
    }
}

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
    /// Interaction logic for OrderList.xaml
    /// </summary>
    public partial class OrderList : UserControl
    {
        public OrderList(int order_id)
        {
            InitializeComponent();
            FillList(order_id);
        }

        private void FillList(int order_id)
        {
            try
            {
                Payment_Service payment_logic = new Payment_Service();
                receipt_ListView.ItemsSource = payment_logic.GetReceipt(order_id);
            }
            catch (Exception)
            {
                throw new Exception("The Order List could not be loaded");
            }

        }
    }
}

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
        public OrderList(Payment payment)
        {
            InitializeComponent();
            FillList(payment);
        }

        private void FillList(Payment payment)
        {
            Payment_Service payment_logic = new Payment_Service();
            receipt_ListView.ItemsSource = payment_logic.GetReceipt(payment.Order_id);
        }
    }
}

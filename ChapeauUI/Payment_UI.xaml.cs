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
    /// Interaction logic for Payment_UI.xaml
    /// </summary>
    public partial class Payment_UI : Page
    {
        private Payment payment_Model = new Payment();
        private Payment_Service payment_Logic = new Payment_Service();
        private List<Item> order;
        private int order_id;
        private PayMethod method;
        public Payment_UI(int order_id)
        {
            InitializeComponent();
            this.order_id = order_id;
            FillReceipt(this.order_id);
        }

        private void FillReceipt(int order_id)
        { 
            DataTable dataTable = payment_Logic.GetReceipt(order_id);
            Receipt_LV.DataContext = dataTable.DefaultView;
            order = payment_Logic.ReadTable(dataTable);
            payment_Logic.GetTotalPrice(order, payment_Model);
        }

        private void Btn_Payment_Finish_Click(object sender, RoutedEventArgs e)
        {
            float tip = float.Parse(Tip_Box.Text);
            string comment = Comment_Box.Text;
            payment_Logic.SetPayment(payment_Model,order_id, tip, method, comment);

            payment_Logic.InsertPayment(payment_Model);
        }

        private void Pin_rBtn_Checked(object sender, RoutedEventArgs e)
        {
            PayMethodCheck(PayMethod.Pin);
        }

        private void Credit_rBtn_Checked(object sender, RoutedEventArgs e)
        {
            PayMethodCheck(PayMethod.Credit);
        }

        private void Cash_rBtn_Checked(object sender, RoutedEventArgs e)
        {
            PayMethodCheck(PayMethod.Cash);
        }
        private void PayMethodCheck(PayMethod method)
        {
            this.method = method;
        }

        private void Btn_Split_Click(object sender, RoutedEventArgs e)
        {
            //change customer nr
            payment_Logic.SplitPrice(payment_Model.Price, 4);
        }
    }
}

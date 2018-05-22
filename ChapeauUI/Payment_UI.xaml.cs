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
        private int customer_count;
        public Payment_UI(int order_id, int customer_count)
        {
            InitializeComponent();
            this.customer_count = customer_count;
            this.order_id = order_id;
            FillReceipt(this.order_id);
            lbl_order.Content = $"Order ID: {order_id}";
        }

        private void FillReceipt(int order_id)
        {
            // Get receipt from db and display
            DataTable dataTable = payment_Logic.GetReceipt(order_id);
            Receipt_LV.DataContext = dataTable.DefaultView;

            // Process the data and fill the model + calc price
            order = payment_Logic.ReadTable(dataTable);
            payment_Logic.GetTotalPrice(order, payment_Model);

            // Display price on the labels
            total_price.Content = $"Total Price: {payment_Model.Price}";
            vat_price.Content = $"Vat Price: {payment_Model.Vat}";
            Btn_Payment_Finish.IsEnabled = false;
            Tip_Box.IsEnabled = false;
        }


        private void Btn_Payment_Finish_Click(object sender, RoutedEventArgs e)
        {
            // Get information from textboxes
            float tip = float.Parse(Tip_Box.Text);
            string comment = Comment_Box.Text;

            // Fill the model with information then send information to db
            payment_Logic.SetPayment(payment_Model, order_id, tip, method, comment);
            payment_Logic.InsertPayment(payment_Model);

            // Direct to tableview when order is finalized.
            NavigationService.Navigate(new Tableview_UI());
        }

        private void Pin_rBtn_Checked(object sender, RoutedEventArgs e)
        {
            PayMethodCheck(PayMethod.Pin);
            Btn_Payment_Finish.IsEnabled = true;
            Tip_Box.IsEnabled = false;
        }

        private void Credit_rBtn_Checked(object sender, RoutedEventArgs e)
        {
            PayMethodCheck(PayMethod.Credit);
            Btn_Payment_Finish.IsEnabled = true;
            Tip_Box.IsEnabled = false;
        }

        private void Cash_rBtn_Checked(object sender, RoutedEventArgs e)
        {
            PayMethodCheck(PayMethod.Cash);
            Btn_Payment_Finish.IsEnabled = true;          
            Tip_Box.IsEnabled = true;                      
        }

        private void PayMethodCheck(PayMethod method){
            this.method = method;
        }

        //What exactly does this need to do...? Ask nymp
        private void Btn_Split_Click(object sender, RoutedEventArgs e){
            float splitted = payment_Logic.SplitPrice(payment_Model.Price, customer_count);
            total_price.Content = $"Total Price: {splitted} x{customer_count}";
        }
    }
}

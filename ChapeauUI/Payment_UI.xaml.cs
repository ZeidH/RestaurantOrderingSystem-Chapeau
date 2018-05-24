using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
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
        private List<Item> menu;
        private int order_id;
        private PayMethod method;
        private int customer_count;

        public Payment_UI(int order_id, int customer_count, List<Item> menu)
        {
            InitializeComponent();
            this.customer_count = customer_count;
            this.order_id = order_id;
            this.menu = menu;
            FillReceipt(this.order_id);
            lbl_order.Content = $"Order ID: {order_id}";
        }

        private void FillReceipt(int order_id)
        {
            // Get receipt from db and display
            List<int> order_itemId = payment_Logic.GetOrderItemID(order_id, menu);
            order = payment_Logic.GetReceipt(menu, order_itemId);
            receipt_ListView.DataContext = order;

            // Process the data and fill the model + calc price
            payment_Logic.GetTotalPrice(order, payment_Model);

            // Display price on the labels
            total_price.Content = $"Total Price: {payment_Model.Price.ToString("0.00")}";
            vat_price.Content = $"Vat Price: {payment_Model.Vat.ToString("0.00")}";
            btn_Payment_Finish.IsEnabled = false;
            tip_Box.IsEnabled = false;
        }

        private void Btn_Payment_Finish_Click(object sender, RoutedEventArgs e)
        {
            // Get information from textboxes
            float tip = float.Parse(tip_Box.Text);
            string comment = comment_Box.Text;

            // Fill the model with information then send information to db
            payment_Logic.SetPayment(payment_Model, order_id, tip, method, comment);
            payment_Logic.InsertPayment(payment_Model);

            // Direct to tableview when order is finalized.
            NavigationService.Navigate(new Tableview_UI());
        }

        private void Pin_rBtn_Checked(object sender, RoutedEventArgs e)
        {
            PayMethodCheck(PayMethod.Pin);
            btn_Payment_Finish.IsEnabled = true;
            tip_Box.IsEnabled = false;
        }

        private void Credit_rBtn_Checked(object sender, RoutedEventArgs e)
        {
            PayMethodCheck(PayMethod.Credit);
            btn_Payment_Finish.IsEnabled = true;
            tip_Box.IsEnabled = false;
        }

        private void Cash_rBtn_Checked(object sender, RoutedEventArgs e)
        {
            PayMethodCheck(PayMethod.Cash);
            btn_Payment_Finish.IsEnabled = true;          
            tip_Box.IsEnabled = true;                      
        }

        private void PayMethodCheck(PayMethod method){
            this.method = method;
        }

        //What exactly does this need to do...? Ask Nymp/Erwin/Gerwin
        private void Btn_Split_Click(object sender, RoutedEventArgs e){
            float splitted = payment_Logic.SplitPrice(payment_Model.Price, customer_count);
            total_price.Content = $"Total Price: {splitted.ToString("0.00")} x{customer_count}";
        }
    }
}

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data;
using ChapeauLogic;
using ChapeauModel;
using System;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Payment_UI.xaml
    /// </summary>
    public partial class Payment_UI : Page
    {
        private Payment order_payment = new Payment();
        private Payment_Service payment_Logic = new Payment_Service();
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
            receipt_ListView.ItemsSource = payment_Logic.GetReceipt(order_id);

            // Process the data and fill the model + calc price
            order_payment = payment_Logic.GetTotalPrice(payment_Logic.GetReceipt(order_id));

            // Display price on the labels
            total_price.Content = $"Total Price: {order_payment.Price.ToString("0.00 €")}";
            vat_price.Content = $"Vat Price: {order_payment.Vat.ToString("0.00 €")}";
            btn_Payment_Finish.IsEnabled = false;
            tip_Box.IsEnabled = false;
        }

        private void Btn_Payment_Finish_Click(object sender, RoutedEventArgs e)
        {
            // Get information from textboxes
            float tip = float.Parse(tip_Box.Text);
            string comment = comment_Box.Text;

            // Fill the payment with information then send information to db
            payment_Logic.SetPayment(order_payment, order_id, tip, method, comment);
            payment_Logic.InsertPayment(order_payment);

            // Direct to tableview when order is finalized.
            NavigationService.Navigate(new Tableview_UI());
        }

        private void Radio_Btn_Checked(object sender, RoutedEventArgs e)
        {
            PayMethodCheck(payment_Logic.GetPayMethod(e.Source.ToString()));
            btn_Payment_Finish.IsEnabled = true;
        }

        private void PayMethodCheck(PayMethod method){
            this.method = method;
            if (method == PayMethod.Cash){
                tip_Box.IsEnabled = true;
            }
            else{
                tip_Box.IsEnabled = false;
            }
        }

        //What exactly does this need to do...? Ask Nymp/Erwin/Gerwin
        private void Btn_Split_Click(object sender, RoutedEventArgs e){
            total_price.Content = $"Total Price: {payment_Logic.SplitPrice(order_payment.Price, customer_count).ToString("0.00")} x{customer_count}";
        }
    }
}

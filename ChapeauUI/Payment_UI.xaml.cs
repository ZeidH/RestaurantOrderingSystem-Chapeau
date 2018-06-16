using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Animation;
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
        private Payment payment;
        private Payment_Service payment_Logic = new Payment_Service();
        private Payment_Split split;
        private Payment_Tip tip;

        #region Constructor & Initialize OrderList
        public Payment_UI(int order_id, int customer_count)
        {
            InitializeComponent();
            payment = new Payment(customer_count, order_id);
            FillReceipt();
            lbl_order.Content = $"Order ID: {order_id}";
        }

        private void FillReceipt()
        {
            try
            {
                // Get listview from UC
                order_list.Children.Add(new OrderList(payment.Order_id));
            }
            catch (Exception)
            {
                if(ErrorMessage(new Exception("Cannot connect to server \nClick 'OK' to try again")))
                    FillReceipt();               
            }

            // Process the data and fill the model
            payment_Logic.GetTotalPrice(payment_Logic.GetReceipt(payment.Order_id), payment);
            // Display price on the labels
            total_price.Content = $"Total Price: {payment.Price.ToString("0.00 €")}";
            vat_price.Content = $"Vat Price: {payment.ReadVat.ToString("0.00 €")}";
            btn_Payment_Finish.IsEnabled = false;

            if (payment.CustomerCount < 2 && payment.CustomerCount > 4)
            {
                btn_Split.IsEnabled = false;
            }
        }
        #endregion

        private void Btn_Payment_Finish_Click(object sender, RoutedEventArgs e)
        {
            // Get information from textbox
            payment.Comment = comment_Box.Text;

            SplitButtonCheck();

            // Send information to db, if there are no payments left then go to tableview
            try
            {
                if (payment_Logic.InsertPayment(payment))
                {
                    // Direct to tableview when order is finalized
                    NavigationService.Navigate(new Redirect("Payment Complete"));
                }
            }
            catch (Exception exp)
            {
                if (ErrorMessage(exp))
                    Btn_Payment_Finish_Click(sender, e);
            }

            RefreshTip();
            btn_even_split.IsEnabled = false;
            btn_Undo_Split.IsEnabled = false;
            btn_Payment_Finish.Content = $"Finalize Guest {payment.NextCustomer + 1}";
        }
        private bool ErrorMessage(Exception e)
        {
            return MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK;   
        }

        #region Payment Method Radiobuttons
        private void Radio_Btn_Checked(object sender, RoutedEventArgs e)
        {
            PayMethodCheck(payment_Logic.GetPayMethod((sender as RadioButton).Content.ToString()));
            btn_Payment_Finish.IsEnabled = true;
        }

        private void PayMethodCheck(PayMethod method)
        {
            payment.Method = method;
            if (payment.Method == PayMethod.Cash)
            {
                tip = new Payment_Tip(payment);
                lbl_tip.Visibility = Visibility.Hidden;
                tip_panel.Children.Add(tip);
            }
            else
            {
                payment.Tip = 0;
                lbl_tip.Visibility = Visibility.Visible;
                tip_panel.Children.Clear();
            }
        }
        #endregion

        #region Payment Split Mode
        private void SplitButtonCheck()
        {
            // Disable the Guests increase/decrease and delete buttons when they're finalized.
            if ((payment.NextCustomer + 1) != payment.CustomerCount && (payment.SplitPayment))
            {
                for (int i = 0; i < split.add_buttons.GetLength(1); i++)
                {
                    split.add_buttons[payment.NextCustomer, i].IsEnabled = false;
                }
                foreach (Button delete_btn in split.delete_buttons)
                {
                    delete_btn.IsEnabled = false;
                }
            }
        }
        private void Btn_Split_Click(object sender, RoutedEventArgs e)
        {
            payment.SplitPayment = true;
            payment.Tip = 0;
            SplitPanel();
        }
        private void SplitPanel()
        {
            split = new Payment_Split(payment);
            receipt_panel.Children.Add(split);
            btn_Payment_Finish.Content = $"Finalize Guest {payment.NextCustomer + 1}";

            RefreshTip();
            btn_Split.Visibility = Visibility.Hidden;
            btn_Undo_Split.Visibility = Visibility.Visible;
            btn_even_split.Visibility = Visibility.Visible;
        }
        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Tableview_UI());
        }
        private void Btn_Undo_Split_Click(object sender, RoutedEventArgs e)
        {
            payment.Tip = 0;
            payment.SplitPayment = false;
            receipt_panel.Children.Clear();
            RefreshTip();
            btn_Payment_Finish.Content = "Finalize";

            btn_Undo_Split.Visibility = Visibility.Hidden;
            btn_even_split.Visibility = Visibility.Hidden;
            btn_Split.Visibility = Visibility.Visible;
        }

        private void Btn_even_split_Click(object sender, RoutedEventArgs e)
        {
            receipt_panel.Children.Clear();
            payment_Logic.ResetSplit(payment);
            SplitPanel();
        }

        private void RefreshTip()
        {
            if (payment.Method == PayMethod.Cash)
            {
                tip.UpdateLabel();
            }
        }
        #endregion
    }
}

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
    /// Interaction logic for Payment_Tip.xaml
    /// </summary>
    public partial class Payment_Tip : UserControl
    {
        private Payment payment;
        public Payment_Tip(Payment payment)
        {
            InitializeComponent();
            this.payment = payment;
            UpdateLabel();
        }

        public void UpdateLabel()
        {
            lbl_tip.Content = payment.Price + payment.Tip;
        }

        #region Payment Tip Click Events
        private void Btn_1_Click(object sender, RoutedEventArgs e)
        {
            payment.Tip += 1;
            UpdateLabel();
        }

        private void Btn_5_Click(object sender, RoutedEventArgs e)
        {
            payment.Tip += 5;
            UpdateLabel();
        }

        private void Btn_Zero_Click(object sender, RoutedEventArgs e)
        {
            payment.Tip = 0;
            UpdateLabel();
        } 
        #endregion
    }
}

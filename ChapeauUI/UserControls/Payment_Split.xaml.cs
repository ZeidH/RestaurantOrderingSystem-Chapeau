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
using ChapeauModel;
using ChapeauLogic;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Payment_Split.xaml
    /// </summary>
    public partial class Payment_Split : UserControl
    {
        private Payment_Service payment_logic = new Payment_Service();
        public List<Button> delete_buttons = new List<Button>();
        public Button[,] add_buttons;
        private Payment payment;
        public Payment_Split(Payment payment)
        {
            InitializeComponent();
            this.payment = payment;
            add_buttons = new Button[payment.CustomerCount, 2];
            FillStackPanel();
            if (payment.CustomerCount >= 4)
                delete_buttons[0].IsEnabled = false;
            UpdateLabels();
        }

        private void FillStackPanel()
        {
            payment.GuestPrice = new List<int>();
            int i = 0;
            while (i < payment.CustomerCount)
            {
                foreach (Label guest in stackpanel_Guest_Price.Children)
                {
                    if (i < payment.CustomerCount)
                    {
                        payment.GuestPrice.Add(payment.SplittedPrice);
                        guest.Visibility = Visibility.Visible;
                        CreateButton(i, payment);
                        i++;
                    }
                }
            }
        }
        #region Button Creation
        private void CreateButton(int i, Payment payment)
        {
            if (i != (payment.CustomerCount - 1))
            {
                CreateEuroButton(i);
            }
            if (i > 1)
            {
                CreateDeleteButton(i, payment);
            }

        }


        private void CreateEuroButton(int i)
        {
            Style style = Application.Current.FindResource("Split") as Style;
            Button button1 = new Button
            {
                Content = " +1€ ",
                Style = style,
                Name = $"btn_1Euro_{i}",
                Width = 40,
                Height = 27,
                Margin = new Thickness(0, 0, 5, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Grid.SetColumn(button1, 1);
            Grid.SetRow(button1, i);
            btnGrid.Children.Add(button1);
            button1.Click += new RoutedEventHandler(Button_1Euro_Click);

            Button button5 = new Button
            {
                Content = " +5€ ",
                Style = style,
                Name = $"btn_5Euro_{i}",
                Width = 40,
                Height = 27,
                Margin = new Thickness(0, 0, 8, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetColumn(button5, 2);
            Grid.SetRow(button5, i);
            btnGrid.Children.Add(button5);
            button5.Click += new RoutedEventHandler(Button_5Euro_Click);

            add_buttons[i, 0] = button1;
            add_buttons[i, 1] = button5;
        }
        private void CreateDeleteButton(int i, Payment payment)
        {
            Style style = Application.Current.FindResource("Close") as Style;
            Button delete = new Button
            {
                Content = " DEL ",
                Style = style,
                Name = $"btn_Delete_{i}",
                Width = 35,
                Height = 27,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            delete_buttons.Add(delete);

            Grid.SetColumn(delete, 3);

            Grid.SetRow(delete, i);
            btnGrid.Children.Add(delete);
            delete.Click += new RoutedEventHandler(Button_Null_Click);
        }
        #endregion

        #region Button Click Events
        private void Button_Null_Click(object sender, RoutedEventArgs e)
        {
            ChangeGuest(Splitter((sender as Button).Name.ToString()), 0);
        }

        private void Button_5Euro_Click(object sender, RoutedEventArgs e)
        {
            ChangeGuest(Splitter((sender as Button).Name.ToString()), 50000);
        }

        private void Button_1Euro_Click(object sender, RoutedEventArgs e)
        {
            ChangeGuest(Splitter((sender as Button).Name.ToString()), 10000);
        } 
        #endregion

        // Splits the label/button names to get which row
        private int Splitter(string value)
        {
            string[] splitted = value.Split('_');
            return int.Parse(splitted[2]);
        }

        // Math
        private void ChangeGuest(int guestNr, int change)
        {

            int id = Splitter((stackpanel_Guest_Price.Children[guestNr] as Label).Name.ToString());

            if (change == 0)
            {
                payment_logic.CalculateGuestPriceDelete(payment, id);
                if (payment.CustomerCount == 4)
                {

                }
            }
            else
            {
                payment_logic.CalculateGuestPriceAdd(payment, change, id);
            }
            ButtonCheck();
            UpdateLabels();
        }

        // Supposed to unenable buttons when their price is 0
        private void ButtonCheck()
        {
            int alive = payment_logic.GuestsOverZero(payment);

            if (alive == 4)
                return;
            if (alive == 3 && payment.GuestPrice.Count == 4)
            {
                delete_buttons[0].IsEnabled = true;
                delete_buttons[1].IsEnabled = false;
                add_buttons[2, 0].IsEnabled = false;
                add_buttons[2, 1].IsEnabled = false;

            }
            else if (alive == 2 && payment.CustomerCount > 2)
            {
                add_buttons[1, 0].IsEnabled = false;
                add_buttons[1, 1].IsEnabled = false;
                delete_buttons[0].IsEnabled = false;
            }
            else if (alive == 1)
            {
                SplitException(new Exception("There's no point in using split if only one person is paying."));
            }

        }
        private void SplitException(Exception exp)
        {
            MessageBox.Show(exp.Message,"Warning",MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void UpdateLabels()
        {
            int i = 0;
            foreach (Label guest in stackpanel_Guest_Price.Children)
            {
                if (i < payment.GuestPrice.Count)
                {
                    guest.Content = $"Guest {i + 1} Price: {(float)payment.GuestPrice[i]/10000}";
                    i++;
                }
            }
        }
    }
}

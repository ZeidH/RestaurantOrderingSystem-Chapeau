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
        }

        private void FillStackPanel()
        {
            int i = 0;
            while (i < payment.CustomerCount)
            {
                foreach (Label guest in stackpanel_Guest_Price.Children)
                {
                    if (i < payment.CustomerCount)
                    {
                        guest.Content = $"Guest {i + 1} Price: {payment.SplittedPrice}";
                        payment.GuestPrice.Add(payment.SplittedPrice);
                        guest.Visibility = Visibility.Visible;
                        CreateButton(i, payment);
                        i++;
                    }
                }
            }

        }

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
            Button button1 = new Button
            {
                Content = " +1€ ",
                Name = $"btn_1Euro_{i}",
                Width = 30,
                Height = 25,
                Margin = new Thickness(0, 0, 5, 0),
                Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 153))
            };

            Grid.SetColumn(button1, 1);
            Grid.SetRow(button1, i);
            btnGrid.Children.Add(button1);
            button1.Click += new RoutedEventHandler(Button_1Euro_Click);

            Button button5 = new Button
            {
                Content = " +5€ ",
                Name = $"btn_5Euro_{i}",
                Width = 30,
                Height = 25,
                Margin = new Thickness(0, 0, 8, 0),
                Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 153))
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
            Button delete = new Button
            {
                Content = " DEL ",
                Name = $"btn_Delete_{i}",
                Width = 30,
                Height = 25,
                Background = new SolidColorBrush(Color.FromArgb(255, 249, 85, 85))
            };
            delete_buttons.Add(delete);
            if (i == (payment.CustomerCount - 1))
            {
                Grid.SetColumn(delete, 1);
                Grid.SetColumnSpan(delete, 3);
                delete.Width = 70;
            }
            else
            {
                Grid.SetColumn(delete, 3);
            }
            Grid.SetRow(delete, i);
            btnGrid.Children.Add(delete);
            delete.Click += new RoutedEventHandler(Button_Null_Click);
        }

        // Click handlers
        private void Button_Null_Click(object sender, RoutedEventArgs e)
        {
            ChangeGuest(Splitter((sender as Button).Name.ToString()), 0);
        }

        private void Button_5Euro_Click(object sender, RoutedEventArgs e)
        {
            ChangeGuest(Splitter((sender as Button).Name.ToString()), 5);
        }

        private void Button_1Euro_Click(object sender, RoutedEventArgs e)
        {
            ChangeGuest(Splitter((sender as Button).Name.ToString()), 1);
        }

        // Splits the label/button names to get which row
        private int Splitter(string value)
        {
            string[] splitted = value.Split('_');
            return int.Parse(splitted[2]);
        }

        // Math
        private void ChangeGuest(int guestNr, float change)
        {
            foreach (Label label in stackpanel_Guest_Price.Children)
            {
                int id = Splitter(label.Name);
                if (id == guestNr)
                {
                    for (int i = id; i < payment.GuestPrice.Count; i++)
                    {
                        if (change == 0)
                        {
                            payment_logic.CalculateGuestPriceDelete(payment, id, i);
                            if (payment.CustomerCount >= 4)
                            {
                                delete_buttons[0].IsEnabled = true;
                                delete_buttons[1].IsEnabled = false;
                            }

                            break;
                        }
                        else
                        {
                            payment_logic.CalculateGuestPriceAdd(i + 1, payment, change, id);
                            break;
                        }
                    }
                }
            }
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            int i = 0;
            foreach (Label guest in stackpanel_Guest_Price.Children)
            {
                if (i < payment.GuestPrice.Count)
                {
                    guest.Content = $"Guest {i + 1} Price: {payment.GuestPrice[i]}";
                    i++;
                }
            }
        }
    }
}

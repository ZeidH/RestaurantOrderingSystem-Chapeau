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

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Payment_Split.xaml
    /// </summary>
    public partial class Payment_Split : UserControl
    {
        private List<float> guestPrice = new List<float>();
        public Payment_Split(Payment payment)
        {
            InitializeComponent();

            FillStackPanel(payment);
        }

        private void FillStackPanel(Payment payment)
        {
            int i = 0;
            while (i < payment.CustomerCount)
            {
                foreach (Label guest in stackpanel_Guest_Price.Children)
                {
                    if (i < payment.CustomerCount)
                    {
                        guest.Content = $"Guest {i + 1} Price: {payment.SplittedPrice}";
                        guestPrice.Add(payment.SplittedPrice);
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
                Button button1 = new Button
                {
                    Content = " +1€ ",
                    Name = $"btn_1Euro_{i}",
                    Width = 30,
                    Height = 30
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
                    Height = 30
                };
                Grid.SetColumn(button5, 2);
                Grid.SetRow(button5, i);
                btnGrid.Children.Add(button5);
                button5.Click += new RoutedEventHandler(Button_5Euro_Click);

            }
            if (i != 0 && i != 1)
            {
                Button delete = new Button
                {
                    Content = " DEL ",
                    Name = $"btn_Delete_{i}",
                    Width = 30,
                    Height = 30
                };
                Grid.SetColumn(delete, 3);
                Grid.SetRow(delete, i);
                btnGrid.Children.Add(delete);
                delete.Click += new RoutedEventHandler(Button_Null_Click);
            }

        }

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
        private int Splitter(string value)
        {
            string[] splitted = value.Split('_');
            return int.Parse(splitted[2]);
        }
        //Logic, move to logic layer
        private void ChangeGuest(int guestNr, float change)
        {
            foreach (Label label in stackpanel_Guest_Price.Children)
            {
                int id = Splitter(label.Name);
                if (id == guestNr)
                {
                    for (int i = 0; i < guestPrice.Count; i++)
                    {
                        if (i >= id)
                        {
                            if (change == 0)
                            {
                                //guestPrice[i] += (guestPrice[id] / (guestPrice.Count - (id + 1)));

                                for (int x = 0; x < i; x++)
                                {
                                    guestPrice[x] += (guestPrice[id] / i);
                                }
                                break;
                            }
                            else
                            {
                                switch (i)
                                {
                                    case 0:
                                        for (int x = 1; x < guestPrice.Count; x++)
                                        {
                                            guestPrice[x] -= (change / (guestPrice.Count - (id + 1)));
                                        }
                                        break;
                                    case 1:
                                        for (int x = 2; x < guestPrice.Count; x++)
                                        {
                                            guestPrice[x] -= (change / (guestPrice.Count - (id + 1)));
                                        }
                                        break;
                                    case 2:
                                        for (int x = 3; x < guestPrice.Count; x++)
                                        {
                                            guestPrice[x] -= (change / (guestPrice.Count - (id + 1)));
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;

                            }
                        }
                    }
                    if (change == 0)
                    {
                        guestPrice[id] = change;
                    }
                    else
                    {
                        guestPrice[id] += change;
                    }
                }
            }
            UpdateLabels();
        }
        private void UpdateLabels()
        {
            int i = 1;

            foreach (Label guest in stackpanel_Guest_Price.Children)
            {
                if (i - 1 < guestPrice.Count)
                {
                    guest.Content = $"Guest {i} Price: {guestPrice[i - 1]}";
                    i++;
                }
            }
        }
    }
}

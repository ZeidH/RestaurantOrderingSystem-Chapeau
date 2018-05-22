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
using System.Collections.ObjectModel;
using System.Data;
using ChapeauLogic;
using ChapeauModel;


namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Orderview_UI.xaml
    /// </summary>
    public partial class Orderview_UI : Page
    {
        private int order_id;
        private MenuCategory category;
        private Item_Service item = new Item_Service();
        private Item order_item;
        private List<Item> order = new List<Item>();
        //private Item order_item = new Item();

        public Orderview_UI(int order_id)
        {
            this.order_id = order_id;
            InitializeComponent();
        }

        private void Btn_return_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Tableview_UI());
        }

        private void Btn_lunch_Click(object sender, RoutedEventArgs e)
        {
            category = MenuCategory.Lunch;
            StackPanel_sub_category.Children.Clear();
            foreach (Lunch sub_category in Enum.GetValues(typeof(Lunch)))
            {
                Button button = new Button
                {
                    Content = sub_category,
                    Name = $"{sub_category}"
                };
                StackPanel_sub_category.Children.Add(button);
            }
            int amount = StackPanel_sub_category.Children.Count;
            double width = StackPanel_sub_category.Width / amount;

            foreach (Button button in StackPanel_sub_category.Children)
            {
                button.Width = width - 2;
                button.Click += new RoutedEventHandler(ButtonLunch_Click);
            }
        }

        private void Btn_dinner_Click(object sender, RoutedEventArgs e)
        {
            category = MenuCategory.Dinner;
            StackPanel_sub_category.Children.Clear();
            foreach (Dinner sub_category in Enum.GetValues(typeof(Dinner)))
            {
                Button button = new Button
                {
                    Content = sub_category,
                    Name = $"{sub_category}"
                };
                StackPanel_sub_category.Children.Add(button);
            }
            int amount = StackPanel_sub_category.Children.Count;
            double width = StackPanel_sub_category.Width / amount;

            foreach (Button button in StackPanel_sub_category.Children)
            {
                button.Width = width - 2;
                button.Click += new RoutedEventHandler(ButtonDinner_Click);
            }
        }

        private void Btn_drinks_Click(object sender, RoutedEventArgs e)
        {
            category = MenuCategory.Drink;
            StackPanel_sub_category.Children.Clear();
            foreach (Drink sub_category in Enum.GetValues(typeof(Drink)))
            {
                Button button = new Button
                {
                    Content = sub_category,
                    Name = "Btn_" + $"{sub_category}",
                };
                StackPanel_sub_category.Children.Add(button);

            }
            int amount = StackPanel_sub_category.Children.Count;
            double width = StackPanel_sub_category.Width / amount;

            foreach (Button button in StackPanel_sub_category.Children)
            {
                button.Width = width - 2;
                button.Click += new RoutedEventHandler(ButtonDrink_Click);
            }
        }

        private void ButtonDrink_Click(object sender, RoutedEventArgs e)
        {
            LoadMenu(e);
        }

        private void ButtonDinner_Click(object sender, RoutedEventArgs e)
        {
            LoadMenu(e);
        }

        private void ButtonLunch_Click(object sender, RoutedEventArgs e)
        {
            LoadMenu(e);
        }

        private void LoadMenu(RoutedEventArgs e)
        {
            int sub_category = item.FindCategory(e.Source.ToString(), category);
            DataTable dataTable = item.GetMenu(category, sub_category);
            Listview_menu.DataContext = dataTable.DefaultView;
        }

        private void Listview_menu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddToOrder();
        }

        private void Listview_menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (DataRowView item in Listview_menu.SelectedItems)
            {
                order_item = new Item
                {
                    Item_id = (int)item.Row.ItemArray[0],
                    Name = item.Row.ItemArray[1].ToString(),
                    Category = category,
                    Amount = 1,
                    Cost = (float)(double)item.Row.ItemArray[2],
                    Stock = (int)item.Row.ItemArray[3] - 1,
                    Order_id = order_id
                };
                if (order_item.Stock < 0)
                {
                    Btn_add_order_item.IsEnabled = false;
                    Btn_add_order_item.Content = "no stock!";
                    return;
                }
            }
            Btn_add_order_item.Content = "Add";
            Btn_add_order_item.IsEnabled = true;
        }

        private void AddToOrder()
        {
            if (Txt_comments.Text != "")
            {
                order_item.Name += "\n Comment: " + Txt_comments.Text;
                order_item.Comment = Txt_comments.Text;
            }

            for (int i = 0; i < order.Count; i++)
            {
                if (order[i].Name == order_item.Name)
                {
                    item.IncreaseAmount(order[i]);
                    DataGrid_order.Items.Refresh();
                    Lbl_total_price.Content = item.GetTotalCost(order).ToString("0.00");
                    Txt_comments.Text = "";
                    Btn_complete_order.IsEnabled = true;
                    return;
                }
            }
            order.Add(order_item);
            DataGrid_order.Items.Add(order[order.Count - 1]);
            Lbl_total_price.Content = item.GetTotalCost(order).ToString("0.00");
            Txt_comments.Text = "";
            Btn_complete_order.IsEnabled = true;
            Listview_menu.UnselectAll();
            Btn_add_order_item.IsEnabled = false;
        }

        private void Btn_add_order_item_Click(object sender, RoutedEventArgs e)
        {
            AddToOrder();
        }
        private void Btn_increase_item_Click(object sender, RoutedEventArgs e)
        {
            item.IncreaseAmount(order_item);
            Lbl_total_price.Content = item.GetTotalCost(order).ToString("0.00");
            DataGrid_order.Items.Refresh();
            if (order_item.Stock == 0)
            {
                Btn_increase_item.IsEnabled = false;
                Btn_increase_item.Content = "  no\nstock";
            }
            Btn_decrease_item.IsEnabled = true;
        }
        private void Btn_decrease_item_Click(object sender, RoutedEventArgs e)
        {
            item.DecreaseAmount(order_item);
            Lbl_total_price.Content = item.GetTotalCost(order).ToString("0.00");
            DataGrid_order.Items.Refresh();
            if (order_item.Amount == 1)
            {
                Btn_decrease_item.IsEnabled = false;
            }
            if (!Btn_increase_item.IsEnabled)
            {
                Btn_increase_item.IsEnabled = true;
                Btn_increase_item.Content = "+";
            }
        }

        private void DataGrid_order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Item)DataGrid_order.SelectedItem == null)
            {
                Btn_decrease_item.IsEnabled = false;
                Btn_increase_item.IsEnabled = false;
                Btn_remove_item.IsEnabled = false;
                Btn_complete_order.IsEnabled = false;
                return;
            }
            order_item = (Item)DataGrid_order.SelectedItem;
            Btn_remove_item.IsEnabled = true;
            if (order_item.Amount > 1)
            {
                Btn_decrease_item.IsEnabled = true;
            }
            else
            {
                Btn_decrease_item.IsEnabled = false;
            }
            if (order_item.Stock > 1)
            {
                Btn_increase_item.IsEnabled = true;
                Btn_increase_item.Content = "+";
            }
            else
            {
                Btn_increase_item.IsEnabled = false;
                Btn_increase_item.Content = "  no\nstock";
            }
        }

        private void Btn_remove_item_Click(object sender, RoutedEventArgs e)
        {
            DataGrid_order.Items.Remove(order_item);
            order = item.DeleteOrderItem(order, order_item);
            Lbl_total_price.Content = item.GetTotalCost(order).ToString("0.00");
            Btn_complete_order.IsEnabled = true;
            if (order.Count == 0)
            {
                Btn_complete_order.IsEnabled = false;
            }
        }

        private void Btn_complete_order_Click(object sender, RoutedEventArgs e)
        {
            item.NewOrderItem(order);
            NavigationService.Navigate(new Tableview_UI());
        }


    }
}

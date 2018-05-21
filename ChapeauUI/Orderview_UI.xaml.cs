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
        public Orderview_UI(int order_id)
        {
            this.order_id = order_id;
            InitializeComponent();

            //Menu menu, int category
            //Testing 
            //Item_Service item = new Item_Service();
            //MenuCategory menu = MenuCategory.Dinner;
            //Dinner dinner = Dinner.Desserts;
            ////int number = 4;
            //DataTable Table = item.GetMenu(menu, (int)dinner);
            //TestView.DataContext = Table.DefaultView;
        }

        private void Btn_return_Click(object sender, RoutedEventArgs e)
        {
            //return to the tableview page
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
            int sub_category = item.FindCategory(e.Source.ToString(), category);

            DataTable dataTable = item.GetMenu(category, sub_category);
            Listview_menu.DataContext = dataTable.DefaultView;
        }

        private void ButtonDinner_Click(object sender, RoutedEventArgs e)
        {
            int sub_category = item.FindCategory(e.Source.ToString(), category);

            DataTable dataTable = item.GetMenu(category, sub_category);
            Listview_menu.DataContext = dataTable.DefaultView;
        }

        private void ButtonLunch_Click(object sender, RoutedEventArgs e)
        {
            int sub_category = item.FindCategory(e.Source.ToString(), category);

            DataTable dataTable = item.GetMenu(category, sub_category);
            Listview_menu.DataContext = dataTable.DefaultView;
        }

        Item order_item;
        ObservableCollection<Item> order = new ObservableCollection<Item>();
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
                    Stock = (int)item.Row.ItemArray[3],
                    Order_id = order_id
                };
            }
        }

        private void Btn_add_order_item_Click(object sender, RoutedEventArgs e)
        {
            order_item.Comment = Txt_comments.Text;
            
            order.Add(order_item);
            Listview_order.ItemsSource = order;
        }

        //SHIT STUFF BY PLEZ NOT ME
        private void Btn_increase_item_Click(object sender, RoutedEventArgs e)
        {
            item.IncreaseAmount(ordersoem);
            Listview_order.ItemsSource = order;
        }
        private Item ordersoem = new Item();
        private void Listview_order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (DataRowView item in Listview_menu.SelectedItems)
            {
                for (int i = 0; i < order.Count; i++)
                {
                    if (order[i].Item_id == ((int)item.Row.ItemArray[0]))
                    {
                        ordersoem = order[i];
                    }
                ;
                }

            }
        }
    }
}

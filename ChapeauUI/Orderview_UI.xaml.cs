using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Media.Animation;
using System.Data;
using ChapeauLogic;
using ChapeauModel;
using System.Windows.Media;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Orderview_UI.xaml
    /// </summary>
    public partial class Orderview_UI : Page
    {
        private int order_id;
        private int amount_drinks = 0;
        private int amount_lunch = 0;
        private int amount_dinner = 0;
        private Item_Service item_logic = new Item_Service();
        private Item selectedMenuItem;
        private OrderItem selectedOrderItem;
        private List<OrderItem> order = new List<OrderItem>();
        private List<Item> menu = new List<Item>();

        //Save the given order id and get the menu from the database
        public Orderview_UI(int order_id, int customer_count, int table_nr)
        {
            InitializeComponent();
            this.order_id = order_id;
            menu = item_logic.ReadMenu();
            AssignCategoryAmounts(customer_count, table_nr);
        }

        private void AssignCategoryAmounts(int customer_count, int table_nr)
        {
            lbl_table_nr.Content = $"table {table_nr}";
            lbl_amount_customers.Content = $"/ {customer_count}";
            lbl_amount_customers_Copy.Content = $"/ {customer_count}";
            lbl_amount_customers_Copy1.Content = $"/ {customer_count}";
            lbl_amount_drinks.Content = amount_drinks;
            lbl_amount_lunch.Content = amount_lunch;
            lbl_amount_dinner.Content = amount_dinner;
        }

        private void Btn_return_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Tableview_UI());
        }

        private void Btn_lunch_Click(object sender, RoutedEventArgs e)
        {
            btn_lunch.Background = new SolidColorBrush(Color.FromArgb(a: 255, r: 52, g: 73, b: 94));
            if (!item_logic.CheckLunchTime())
            {
                MessageBoxResult result = MessageBox.Show("It is not lunch time!", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ClearStackPanelChildren();
            foreach (Lunch sub_category in Enum.GetValues(typeof(Lunch)))
            {
                CreateSubCategoryButtons(sub_category.ToString());
            }
            SizeStackPanelChildren();
        }

        private void Btn_dinner_Click(object sender, RoutedEventArgs e)
        {
            ClearStackPanelChildren();
            CreateSubCategoryButtons(btn_dinner.Content.ToString());

            foreach (Dinner sub_category in Enum.GetValues(typeof(Dinner)))
            {
                CreateSubCategoryButtons(sub_category.ToString());
            }
            SizeStackPanelChildren();
        }

        private void Btn_drinks_Click(object sender, RoutedEventArgs e)
        {
            ClearStackPanelChildren();
            foreach (Drink sub_category in Enum.GetValues(typeof(Drink)))
            {
                Orderview_sub_category sub_category_button = new Orderview_sub_category(sub_category.ToString());
                wrappanel_sub_category.Children.Add(sub_category_button);
                //CreateSubCategoryButtons(sub_category.ToString());
            }
            SizeStackPanelChildren();
        }

        //Size the buttons inside the stackpanel
        private void SizeStackPanelChildren()
        {
            double buttonWidth = StackPanel_sub_category.Width / StackPanel_sub_category.Children.Count;
            foreach (Button button in StackPanel_sub_category.Children)
            {
                button.Click += new RoutedEventHandler(ButtonSubCategory_Click);
                button.Width = buttonWidth - 4;
            }
        }

        //Create buttons, add them to stackpanel and assign .Click event handler
        private void CreateSubCategoryButtons(string sub_category)
        {
            Button button = new Button
            {
                Content = sub_category,
                Cursor = Cursors.Hand,
                Name = sub_category,
                Background = new SolidColorBrush(Color.FromArgb(a: 255, r: 199, g: 203, b: 207)),
                FontFamily = new FontFamily("Arial"),
                Opacity = 0

            };
            StackPanel_sub_category.Children.Add(button);
            DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromMilliseconds(600));
            button.BeginAnimation(Button.OpacityProperty, animation);
        }

        private void ClearStackPanelChildren()
        {
            StackPanel_sub_category.Children.Clear();
        }

        public void ButtonSubCategory_Click(object sender, RoutedEventArgs e)
        {
            menu = item_logic.RefreshStock(menu);
            listview_menu.ItemsSource = null;
            List<Item> subMenu = item_logic.GetSubMenu(menu, e.Source.ToString());
            listview_menu.ItemsSource = subMenu;
        }

        private void Listview_menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listview_menu.SelectedItem != null)
            {
                lbl_comments.Margin = new Thickness(left: 398, top: 419, right: 0, bottom: 0);
                selectedMenuItem = (Item)listview_menu.SelectedItem;
                if (item_logic.CheckDinnerItem(selectedMenuItem))
                {
                    lbl_comments.Margin = new Thickness(left: 398, top: 385, right: 0, bottom: 0);
                    Orderview_MeatComments _MeatComments = new Orderview_MeatComments();
                    wrappanel_meat_comment.Children.Add(_MeatComments);
                }
                btn_add_order_item.IsEnabled = item_logic.CheckStock(selectedMenuItem.Stock);
            }
        }

        private void Listview_menu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listview_menu.SelectedItem != null)
            {
                AddToOrder();
            }
        }

        private void Btn_add_order_item_Click(object sender, RoutedEventArgs e)
        {
            AddToOrder();
        }

        private void AddToOrder()
        {
            if (!item_logic.VerifyStock(selectedMenuItem))
            {
                MessageBox.Show("This item is out of stock!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                menu = item_logic.RefreshStock(menu);
                listview_menu.Items.Refresh();
                btn_add_order_item.IsEnabled = false;
                return;
            }

            selectedMenuItem.Order_id = order_id;
            IncreaseCategoryAmount(selectedMenuItem.Category);
            OrderItem orderItem = new OrderItem
            {
                Item = selectedMenuItem,
                Comment = txt_comments.Text,
            };

            //Check if the new item already exists in the orderlist
            for (int i = 0; i < order.Count; i++)
            {
                if ((order[i].Comment == orderItem.Comment) && (order[i].Item.Name == orderItem.Item.Name))
                {
                    item_logic.IncreaseAmount(order[i]);
                    dataGrid_order.Items.Refresh();
                    UpdateOrder(orderItem);
                    return;
                }
            }
            item_logic.IncreaseAmount(orderItem);
            order.Add(orderItem);
            dataGrid_order.Items.Add(order[order.Count - 1]);
            UpdateOrder(orderItem);
            btn_add_order_item.IsEnabled = false;

        }

        //Get the total price including VAT from the payment logic layer
        private void UpdateOrder(OrderItem orderItem)
        {
            Payment_Service payment_logic = new Payment_Service();
            Payment payment = payment_logic.GetTotalPrice(order);
            lbl_total_price.Content = payment.Price.ToString("0.00€");
            txt_comments.Text = "";
            btn_complete_order.IsEnabled = true;
            listview_menu.UnselectAll();

            item_logic.UpdateStock(orderItem);
            menu = item_logic.RefreshStock(menu);
            listview_menu.Items.Refresh();
        }

        private void IncreaseCategoryAmount(MenuCategory category)
        {
            switch (category)
            {
                case MenuCategory.Lunch:
                    amount_lunch++;
                    lbl_amount_lunch.Content = amount_lunch;
                    break;
                case MenuCategory.Dinner:
                    amount_dinner++;
                    lbl_amount_dinner.Content = amount_dinner;
                    break;
                case MenuCategory.Drink:
                    amount_drinks++;
                    lbl_amount_drinks.Content = amount_drinks;
                    break;
                default:
                    break;
            }
        }
        private void DecreaseCategoryAmount(MenuCategory category)
        {
            switch (category)
            {
                case MenuCategory.Lunch:
                    amount_lunch--;
                    lbl_amount_lunch.Content = amount_lunch;
                    break;
                case MenuCategory.Dinner:
                    amount_dinner--;
                    lbl_amount_dinner.Content = amount_dinner;
                    break;
                case MenuCategory.Drink:
                    amount_drinks--;
                    lbl_amount_drinks.Content = amount_drinks;
                    break;
                default:
                    break;
            }
        }

        private void DataGrid_order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_order.SelectedItem != null)
            {
                selectedOrderItem = (OrderItem)dataGrid_order.SelectedItems[0];
                CheckIncreaseDecreaseButton(selectedOrderItem);
                btn_remove_item.IsEnabled = true;
            }
            else
            {
                DisableButtons();
            }
        }

        //Increase the amount of the selected item, recalculate total price, refresh datagrid and check which buttons should be enabled
        private void Btn_increase_item_Click(object sender, RoutedEventArgs e)
        {
            if (!item_logic.VerifyStock(selectedOrderItem.Item))
            {
                MessageBox.Show("This item is out of stock!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                menu = item_logic.RefreshStock(menu);
                listview_menu.Items.Refresh();
                return;
            }
            item_logic.IncreaseAmount(selectedOrderItem);
            lbl_total_price.Content = item_logic.GetTotalCost(order).ToString("0.00");
            dataGrid_order.Items.Refresh();

            CheckIncreaseDecreaseButton(selectedOrderItem);
            IncreaseCategoryAmount(selectedOrderItem.Item.Category);

            item_logic.UpdateStock(selectedOrderItem);
            menu = item_logic.RefreshStock(menu);
            listview_menu.Items.Refresh();
        }

        private void Btn_decrease_item_Click(object sender, RoutedEventArgs e)
        {
            item_logic.DecreaseAmount(selectedOrderItem);
            lbl_total_price.Content = item_logic.GetTotalCost(order).ToString("0.00");
            dataGrid_order.Items.Refresh();
            CheckIncreaseDecreaseButton(selectedOrderItem);
            DecreaseCategoryAmount(selectedOrderItem.Item.Category);

            item_logic.UpdateStock(selectedOrderItem);
            menu = item_logic.RefreshStock(menu);
            listview_menu.Items.Refresh();
        }

        private void CheckIncreaseDecreaseButton(OrderItem orderItem)
        {
            btn_decrease_item.IsEnabled = item_logic.CheckAmount(orderItem.Amount);
            btn_increase_item.IsEnabled = item_logic.CheckStock(orderItem.Item.Stock);
        }

        //Remove selected item from orderlist, gridview and recalculate total price
        private void Btn_remove_item_Click(object sender, RoutedEventArgs e)
        {
            dataGrid_order.Items.Remove(selectedOrderItem);
            order = item_logic.DeleteOrderItem(order, selectedOrderItem);
            lbl_total_price.Content = item_logic.GetTotalCost(order).ToString("0.00");
            btn_complete_order.IsEnabled = item_logic.CheckOrderCount(order);
            RemoveCategoryAmount(selectedOrderItem);

            item_logic.UpdateStock(selectedOrderItem);
            menu = item_logic.RefreshStock(menu);
            listview_menu.Items.Refresh();
        }

        private void RemoveCategoryAmount(OrderItem selectedOrderItem)
        {
            switch (selectedOrderItem.Item.Category)
            {
                case MenuCategory.Lunch:
                    amount_lunch = amount_lunch - selectedOrderItem.Amount;
                    lbl_amount_lunch.Content = amount_lunch;
                    break;
                case MenuCategory.Dinner:
                    amount_dinner = amount_dinner - selectedOrderItem.Amount;
                    lbl_amount_dinner.Content = amount_dinner;
                    break;
                case MenuCategory.Drink:
                    amount_drinks = amount_drinks - selectedOrderItem.Amount;
                    lbl_amount_drinks.Content = amount_drinks;
                    break;
                default:
                    break;
            }
        }

        private void DisableButtons()
        {
            btn_decrease_item.IsEnabled = false;
            btn_increase_item.IsEnabled = false;
            btn_remove_item.IsEnabled = false;
        }

        private void Btn_complete_order_Click(object sender, RoutedEventArgs e)
        {
            item_logic.CompleteOrder(order);
            NavigationService.Navigate(new Tableview_UI());
        }
    }
}

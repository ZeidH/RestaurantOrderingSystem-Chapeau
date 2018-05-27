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
        private Item_Service item_logic = new Item_Service();
        private Item selectedMenuItem;
        private OrderItem selectedOrderItem;
        private List<OrderItem> order = new List<OrderItem>();
        private List<Item> menu = new List<Item>();

        public Orderview_UI(int order_id)
        {
            InitializeComponent();
            this.order_id = order_id;
            menu = item_logic.ReadMenu();
        }

        //return to table view
        private void Btn_return_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Tableview_UI());
        }

        private void Btn_lunch_Click(object sender, RoutedEventArgs e)
        {
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
                CreateSubCategoryButtons(sub_category.ToString());
            }
            SizeStackPanelChildren();
        }

        private void SizeStackPanelChildren()
        {
            double buttonWidth = StackPanel_sub_category.Width / StackPanel_sub_category.Children.Count;
            foreach (Button button in StackPanel_sub_category.Children)
            {
                button.Click += new RoutedEventHandler(ButtonSubCategory_Click);
                button.Width = buttonWidth - 2;
            }
        }

        private void CreateSubCategoryButtons(string sub_category)
        {
            Button button = new Button
            {
                Content = sub_category,
                Cursor = Cursors.Hand,
                Name = sub_category,
                Opacity = 0

            };
            StackPanel_sub_category.Children.Add(button);
            DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromMilliseconds(600));
            button.BeginAnimation(Button.OpacityProperty, animation);
            button.Click += new RoutedEventHandler(ButtonSubCategory_Click);
        }

        private void ClearStackPanelChildren()
        {
            StackPanel_sub_category.Children.Clear();
        }

        private void ButtonSubCategory_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.AppStarting;
            listview_menu.ItemsSource = null;
            List<Item> subMenu = item_logic.GetSubMenu(menu, e.Source.ToString());
            List<bool> op = item_logic.CheckOrderStock(subMenu);
            listview_menu.ItemsSource = subMenu;

            Cursor = Cursors.Arrow;
        }

        private void Listview_menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listview_menu.SelectedItem != null)
            {
                selectedMenuItem = (Item)listview_menu.SelectedItem;
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
            selectedMenuItem.Order_id = order_id;
            OrderItem orderItem = new OrderItem
            {
                Item = selectedMenuItem,
                Comment = txt_comments.Text,
            };
            item_logic.IncreaseAmount(orderItem);
            for (int i = 0; i < order.Count; i++)
            {
                if ((order[i].Comment == orderItem.Comment) && (order[i].Item.Name == orderItem.Item.Name))
                {
                    item_logic.IncreaseAmount(order[i]);
                    dataGrid_order.Items.Refresh();
                    UpdateOrder();
                    return;
                }
            }
            order.Add(orderItem);
            dataGrid_order.Items.Add(order[order.Count - 1]);
            UpdateOrder();
            btn_add_order_item.IsEnabled = false;
        }

        private void UpdateOrder()
        {
            Payment_Service payment_logic = new Payment_Service();
            Payment payment = payment_logic.GetTotalPrice(order);
            lbl_total_price.Content = payment.Price.ToString("0.00€");
            txt_comments.Text = "";
            btn_complete_order.IsEnabled = true;
            listview_menu.UnselectAll();
        }

        private void DataGrid_order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_order.SelectedItem != null)
            {
                selectedOrderItem = (OrderItem)dataGrid_order.SelectedItems[0];
                CheckIncreaseDecrease(selectedOrderItem);
                btn_remove_item.IsEnabled = true;
            }
            else
            {
                DisableButtons();
            }
        }

        private void Btn_increase_item_Click(object sender, RoutedEventArgs e)
        {
            item_logic.IncreaseAmount(selectedOrderItem);
            lbl_total_price.Content = item_logic.GetTotalCost(order).ToString("0.00");
            dataGrid_order.Items.Refresh();
            CheckIncreaseDecrease(selectedOrderItem);
        }

        private void Btn_decrease_item_Click(object sender, RoutedEventArgs e)
        {
            item_logic.DecreaseAmount(selectedOrderItem);
            lbl_total_price.Content = item_logic.GetTotalCost(order).ToString("0.00");
            dataGrid_order.Items.Refresh();
            CheckIncreaseDecrease(selectedOrderItem);
        }

        private void CheckIncreaseDecrease(OrderItem orderItem)
        {
            btn_decrease_item.IsEnabled = item_logic.CheckAmount(orderItem.Amount);
            btn_increase_item.IsEnabled = item_logic.CheckStock(orderItem.Item.Stock);
        }

        private void Btn_remove_item_Click(object sender, RoutedEventArgs e)
        {
            dataGrid_order.Items.Remove(selectedOrderItem);
            order = item_logic.DeleteOrderItem(order, selectedOrderItem);
            lbl_total_price.Content = item_logic.GetTotalCost(order).ToString("0.00");
            btn_complete_order.IsEnabled = item_logic.CheckOrderCount(order);
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

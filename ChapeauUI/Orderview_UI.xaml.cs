using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
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
        //private MenuCategory category;
        private Item_Service item_logic = new Item_Service();
        private int selected_menu_item_id;
        private int selected_order_index;
        private List<OrderItem> order = new List<OrderItem>();
        private List<Item> menu = new List<Item>();

        public Orderview_UI(int order_id)
        {
            this.order_id = order_id;
            menu = item_logic.ReadMenu();
            InitializeComponent();
        }

        //return to table view
        private void Btn_return_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Tableview_UI());
        }

        //Make the subcategory buttons according to the menu category
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
                Name = sub_category
            };
            StackPanel_sub_category.Children.Add(button);
            button.Click += new RoutedEventHandler(ButtonSubCategory_Click);
        }

        private void ClearStackPanelChildren()
        {
            StackPanel_sub_category.Children.Clear();
        }

        private void ButtonSubCategory_Click(object sender, RoutedEventArgs e)
        {
            List<Item> subMenu = item_logic.GetSubMenu(menu, e.Source.ToString());
            listview_menu.DataContext = subMenu;
        }

        private void Listview_menu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listview_menu.SelectedItem != null)
            {
                AddToOrder();
            }
        }

        private void Listview_menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)listview_menu.SelectedItems[0];
            selected_menu_item_id = (int)row["item_id"];
            btn_add_order_item.IsEnabled = item_logic.CheckStock(menu[selected_menu_item_id]);
        }

        private void AddToOrder()
        {
            OrderItem orderItem = new OrderItem();
            orderItem.Item = menu[selected_menu_item_id];

            if (txt_comments.Text != "")
            {
                menu[selected_menu_item_id].Name += "\n Comment: " + txt_comments.Text;
                orderItem.Comment = txt_comments.Text;
            }

            for (int i = 0; i < order.Count; i++)
            {
                if (order[i].Item.Name == menu[selected_menu_item_id].Name)
                {
                    item_logic.IncreaseAmount(orderItem);
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
            lbl_total_price.Content = item_logic.GetTotalCost(order).ToString("0.00");
            txt_comments.Text = "";
            btn_complete_order.IsEnabled = true;
            listview_menu.UnselectAll();
        }

        private void Btn_add_order_item_Click(object sender, RoutedEventArgs e)
        {
            AddToOrder();
        }
        private void Btn_increase_item_Click(object sender, RoutedEventArgs e)
        {
            item_logic.IncreaseAmount(order[selected_order_index]);
            lbl_total_price.Content = item_logic.GetTotalCost(order).ToString("0.00");
            dataGrid_order.Items.Refresh();
            if (order[selected_order_index].Item.Stock == 0)
            {
                btn_increase_item.IsEnabled = false;
                btn_increase_item.Content = "  no\nstock";
            }
            btn_decrease_item.IsEnabled = true;
        }
        private void Btn_decrease_item_Click(object sender, RoutedEventArgs e)
        {
            item_logic.DecreaseAmount(order[selected_order_index]);
            lbl_total_price.Content = item_logic.GetTotalCost(order).ToString("0.00");
            dataGrid_order.Items.Refresh();
            if (order[selected_order_index].Amount == 1)
            {
                btn_decrease_item.IsEnabled = false;
            }
            if (!btn_increase_item.IsEnabled)
            {
                btn_increase_item.IsEnabled = true;
                btn_increase_item.Content = "+";
            }
        }

        private void DataGrid_order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Item)dataGrid_order.SelectedItem == null)
            {
                btn_decrease_item.IsEnabled = false;
                btn_increase_item.IsEnabled = false;
                btn_remove_item.IsEnabled = false;
                btn_complete_order.IsEnabled = false;
                return;
            }

            DataRowView row = (DataRowView)dataGrid_order.SelectedItem;
            selected_menu_item_id = (int)row["item_id"];
            for (int i = 0; i < order.Count; i++)
            {
                if (order[i].Item.Item_id == selected_menu_item_id)
                {
                    selected_order_index = i;
                    btn_decrease_item.IsEnabled = item_logic.CheckAmount(order[i].Amount);

                    if (order[i].Item.Stock > 1)
                    {
                        btn_increase_item.IsEnabled = true;
                        btn_increase_item.Content = "+";
                    }
                    else
                    {
                        btn_increase_item.IsEnabled = false;
                        btn_increase_item.Content = "  no\nstock";
                    }
                    //enable remove button?
                    break;
                }
            }
        }

        private void Btn_remove_item_Click(object sender, RoutedEventArgs e)
        {
            dataGrid_order.Items.Remove(selected_order_index);
            order = item_logic.DeleteOrderItem(order, order[selected_order_index]);
            lbl_total_price.Content = item_logic.GetTotalCost(order).ToString("0.00");
            btn_complete_order.IsEnabled = item_logic.CheckOrderCount(order);
        }

        private void Btn_complete_order_Click(object sender, RoutedEventArgs e)
        {
            item_logic.CompleteOrder(order);
            NavigationService.Navigate(new Tableview_UI());
        }


    }
}

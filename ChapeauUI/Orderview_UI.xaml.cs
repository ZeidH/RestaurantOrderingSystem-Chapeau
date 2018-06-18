using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Media.Animation;
using ChapeauLogic;
using ChapeauModel;
using System.Threading.Tasks;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Orderview_UI.xaml
    /// </summary>
    public partial class Orderview_UI : Page
    {
        private int orderId;
        private int amountDrinks;
        private int amountLunch;
        private int amountDinner;
        private Employee employee;
        private Orderview_MeatComments meatComments = new Orderview_MeatComments();
        private Item_Service itemLogic = new Item_Service();
        private Item selectedMenuItem;
        private OrderItem selectedOrderItem;
        private List<OrderItem> order = new List<OrderItem>();
        private List<Item> menu = new List<Item>();

        //Save the given order id and employee, get the menu from the database
        public Orderview_UI(int orderId, int customerCount, int tableNr, Employee employee)
        {
            InitializeComponent();
            this.employee = employee;
            this.orderId = orderId;
            try
            {
                menu = itemLogic.GetMenu();
            }
            catch (Exception exp)
            {
                HandleException(exp);
            }
            AssignCategoryAmounts(customerCount, tableNr);
            amountDrinks = 0;
            amountLunch = 0;
            amountDinner = 0;
            Animation.AnimateIn(this, 1);
        }

        //Assign values to the labels
        private void AssignCategoryAmounts(int customerCount, int tableNr)
        {
            lblTableNr.Content = $"table {tableNr}";
            lblAmountCustomers1.Content = $"/ {customerCount}";
            lblAmountCustomers2.Content = $"/ {customerCount}";
            lblAmountCustomers3.Content = $"/ {customerCount}";
            lblAmountDrinks.Content = amountDrinks;
            lblAmountLunch.Content = amountLunch;
            lblAmountDinner.Content = amountDinner;
        }

        #region MenuButtons
        private void BtnLunch_Click(object sender, RoutedEventArgs e)
        {
            //Warn user if lunch is clicked outside of lunchtime (11h-15h)
            if (!itemLogic.CheckLunchTime())
            {
                HandleException(new Exception("It is not lunch time!"));
            }
            listviewMenu.ItemsSource = null;
            ClearStackPanelChildren();
            btnLunch.Style = (Style)FindResource("ClickedMenuCategory");
            foreach (Lunch subCategory in Enum.GetValues(typeof(Lunch)))
            {
                CreateSubCategoryButtons(subCategory.ToString());
            }
            SizeStackPanelChildren();
        }

        private void BtnDinner_Click(object sender, RoutedEventArgs e)
        {
            ClearStackPanelChildren();
            btnDinner.Style = (Style)FindResource("ClickedMenuCategory");
            listviewMenu.ItemsSource = null;
            foreach (Dinner subCategory in Enum.GetValues(typeof(Dinner)))
            {
                CreateSubCategoryButtons(subCategory.ToString());
            }
            SizeStackPanelChildren();
        }

        private void BtnDrinks_Click(object sender, RoutedEventArgs e)
        {
            ClearStackPanelChildren();
            btnDrinks.Style = (Style)FindResource("ClickedMenuCategory");
            listviewMenu.ItemsSource = null;
            foreach (Drink subCategory in Enum.GetValues(typeof(Drink)))
            {
                CreateSubCategoryButtons(subCategory.ToString());
            }
            SizeStackPanelChildren();
        }

        //Create buttons, add them to stackpanel and assign .Click event handler
        private void CreateSubCategoryButtons(string subCategory)
        {
            Style style = this.FindResource("MenuCategory") as Style;
            Button button = new Button
            {
                Style = style,
                Content = subCategory,
                Name = subCategory,
                Opacity = 0,
                Margin = new Thickness(2, 0, 2, 0)
            };
            stackPanelSubCategory.Children.Add(button);
            DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromMilliseconds(600));
            button.BeginAnimation(Button.OpacityProperty, animation);
        }

        //Size the sub menu buttons inside the stackpanel
        private void SizeStackPanelChildren()
        {
            double buttonWidth = stackPanelSubCategory.Width / stackPanelSubCategory.Children.Count;
            foreach (Button button in stackPanelSubCategory.Children)
            {
                button.Click += new RoutedEventHandler(ButtonSubCategory_Click);
                button.Width = buttonWidth - 3;
            }
        }

        private void ClearStackPanelChildren()
        {
            stackPanelSubCategory.Children.Clear();
            UndoButtonClickedColours();
        }
        private void UndoButtonClickedColours()
        {
            btnLunch.Style = (Style)FindResource("MenuCategory");
            btnDinner.Style = (Style)FindResource("MenuCategory");
            btnDrinks.Style = (Style)FindResource("MenuCategory");
        }

        private void UndoSubButtonClickedColours()
        {
            foreach (Button button in stackPanelSubCategory.Children)
            {
                button.Style = (Style)FindResource("MenuCategory");
            }
        }

        //Get the menu according to the clicked sub menu button
        public void ButtonSubCategory_Click(object sender, RoutedEventArgs e)
        {
            UndoSubButtonClickedColours();
            (sender as Button).Style = (Style)FindResource("ClickedMenuCategory");
            try
            {
                menu = itemLogic.RefreshStock(menu);
                listviewMenu.ItemsSource = null;
                List<Item> subMenu = itemLogic.GetSubMenu(menu, e.Source.ToString());
                listviewMenu.ItemsSource = subMenu;
            }
            catch (Exception exp)
            {
                HandleException(exp);
            }
        }
        #endregion

        private void ListviewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listviewMenu.SelectedItem != null)
            {
                meatComments.meatComment = "";
                ClearMeatCommentPanel();
                selectedMenuItem = (Item)listviewMenu.SelectedItem;
                try
                {
                    //If a meattype can be chosen for selected item, show the buttons from user control
                    if (itemLogic.CheckDinnerItem(selectedMenuItem))
                    {
                        lblComments.Margin = new Thickness(34.2, 333, 0, 0);
                        meatComments = new Orderview_MeatComments();
                        wrappanelMeatComment.Children.Add(meatComments);
                    }
                    btnAddOrderItem.IsEnabled = itemLogic.VerifyStock(selectedMenuItem);
                }
                catch (Exception exp)
                {
                    HandleException(exp);
                }
            }
        }

        #region AddToOrder
        private void ListviewMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listviewMenu.SelectedItem != null)
            {
                AddToOrder();
            }
        }

        private void BtnAddOrderItem_Click(object sender, RoutedEventArgs e)
        {
            AddToOrder();
        }

        private void AddToOrder()
        {
            try
            {
                if (!VerifySelectedItemStock(selectedMenuItem))
                {
                    return;
                }
                selectedMenuItem.Order_id = orderId;
                IncreaseCategoryAmount(selectedMenuItem.Category);
                OrderItem orderItem = new OrderItem
                {
                    Item = selectedMenuItem,
                    Comment = txtComments.Text + " " + meatComments.meatComment
                };

                //remove the meat type comments section
                if (itemLogic.CheckDinnerItem(selectedMenuItem))
                {
                    meatComments.RevertClicked();
                    ClearMeatCommentPanel();
                }
                //Check if the new item already exists in the orderlist
                foreach (OrderItem order in order)
                {
                    if ((order.Comment == orderItem.Comment) && (order.Item.Name == orderItem.Item.Name))
                    {
                        itemLogic.IncreaseAmount(order);
                        DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromMilliseconds(600));
                        BeginAnimation(DataGridCell.OpacityProperty, animation);
                        dataGridOrder.Items.Refresh();
                        UpdateOrder(orderItem);
                        return;
                    }
                }
                itemLogic.IncreaseAmount(orderItem);
                order.Add(orderItem);
                dataGridOrder.Items.Add(order[order.Count - 1]);
                UpdateOrder(orderItem);
                btnAddOrderItem.IsEnabled = false;
            }
            catch (Exception exp)
            {
                HandleException(exp);
            }
        }

        private bool VerifySelectedItemStock(Item selectedMenuItem)
        {
            try
            {
                if (!itemLogic.VerifyStock(selectedMenuItem))
                {
                    HandleException(new Exception("This item is out of stock!"));
                    try
                    {
                        menu = itemLogic.RefreshStock(menu);
                    }
                    catch (Exception)
                    {
                        HandleException(new Exception("The application could not refresh the stock"));
                    }
                    listviewMenu.Items.Refresh();
                    btnAddOrderItem.IsEnabled = false;
                    return false;
                }
            }
            catch (Exception exp)
            {
                HandleException(exp);
            }
            return true;
        }

        private void ClearMeatCommentPanel()
        {
            wrappanelMeatComment.Children.Clear();
            lblComments.Margin = new Thickness(34.2, 364, 0, 0);
        }

        //Get the total price including VAT
        private void UpdateOrder(OrderItem orderItem)
        {
            lblTotalPrice.Content = itemLogic.GetTotalCost(order).ToString("€ 0.00");
            txtComments.Text = "";
            btnCompleteOrder.IsEnabled = true;
            listviewMenu.UnselectAll();
            UpdateRefreshStock(orderItem);
            listviewMenu.Items.Refresh();
        }
        #endregion

        private void UpdateRefreshStock(OrderItem orderItem)
        {
            try
            {
                itemLogic.UpdateStock(orderItem);
                menu = itemLogic.RefreshStock(menu);
            }
            catch (Exception exp)
            {
                HandleException(exp);
            }
        }

        private void DataGridOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridOrder.SelectedItem != null)
            {
                //Set the selectedOrderItem and check if the in/decrease buttons should be enabled
                selectedOrderItem = (OrderItem)dataGridOrder.SelectedItems[0];
                CheckIncreaseDecreaseButton(selectedOrderItem);
                btnRemoveItem.IsEnabled = true;
            }
            //If nothing is selected, disable buttons
            else
            {
                DisableButtons();
            }
        }

        #region IncreaseDecreaseButtons
        private void BtnIncreaseItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!VerifySelectedItemStock(selectedMenuItem))
                {
                    return;
                }
                //Increase the amount of the selected item
                itemLogic.IncreaseAmount(selectedOrderItem);
            }
            catch (Exception exp)
            {
                HandleException(exp);
            }
            // recalculate total price and refresh datagrid
            lblTotalPrice.Content = itemLogic.GetTotalCost(order).ToString("€ 0.00");
            dataGridOrder.Items.Refresh();

            //Check which buttons should be enabled
            CheckIncreaseDecreaseButton(selectedOrderItem);
            IncreaseCategoryAmount(selectedOrderItem.Item.Category);
            UpdateRefreshStock(selectedOrderItem);
            listviewMenu.Items.Refresh();
        }

        private void BtnDecreaseItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                itemLogic.DecreaseAmount(selectedOrderItem);
            }
            catch (Exception exp)
            {
                HandleException(exp);
            }
            //Get total costs, refresh datagrid,stock and listview and check enabled/disabled buttons
            lblTotalPrice.Content = itemLogic.GetTotalCost(order).ToString("€ 0.00");
            dataGridOrder.Items.Refresh();
            CheckIncreaseDecreaseButton(selectedOrderItem);
            DecreaseCategoryAmount(selectedOrderItem.Item.Category);
            UpdateRefreshStock(selectedOrderItem);
            listviewMenu.Items.Refresh();
        }

        private void CheckIncreaseDecreaseButton(OrderItem orderItem)
        {
            btnDecreaseItem.IsEnabled = itemLogic.CheckAmount(orderItem.Amount);
            btnIncreaseItem.IsEnabled = itemLogic.VerifyStock(orderItem.Item);
        }
        #endregion

        #region RemoveOrderItem
        //Remove selected item from orderlist, gridview and recalculate total price
        private void BtnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            dataGridOrder.Items.Remove(selectedOrderItem);
            order = itemLogic.DeleteOrderItem(order, selectedOrderItem);
            lblTotalPrice.Content = itemLogic.GetTotalCost(order).ToString("€ 0.00");
            btnCompleteOrder.IsEnabled = itemLogic.CheckOrderCount(order);
            RemoveCategoryAmount(selectedOrderItem);
            UpdateRefreshStock(selectedOrderItem);
            listviewMenu.Items.Refresh();
        }
        #endregion

        private void BtnCompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                itemLogic.CompleteOrder(order);
            }
            catch (Exception exp)
            {
                HandleException(exp);
                return;
            }
            try
            {
                itemLogic.SetTableStatus(TableStatus.Running, itemLogic.GetTableIDFromOrderID(orderId));
            }
            catch (Exception exp)
            {
                HandleException(exp);
            }
            //If order is completed, navigate to the redirect screen
            NavigationService.Navigate(new Redirect("Order sent!", employee));
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            itemLogic.DeleteOrderList(order);
            NavigationService.Navigate(new Tableview_UI(employee));
        }

        #region CategoryAmounts
        private void RemoveCategoryAmount(OrderItem selectedOrderItem)
        {
            switch (selectedOrderItem.Item.Category)
            {
                case MenuCategory.Lunch:
                    amountLunch = amountLunch - selectedOrderItem.Amount;
                    lblAmountLunch.Content = amountLunch;
                    break;
                case MenuCategory.Dinner:
                    amountDinner = amountDinner - selectedOrderItem.Amount;
                    lblAmountDinner.Content = amountDinner;
                    break;
                case MenuCategory.Drink:
                    amountDrinks = amountDrinks - selectedOrderItem.Amount;
                    lblAmountDrinks.Content = amountDrinks;
                    break;
                default:
                    break;
            }
        }

        private void IncreaseCategoryAmount(MenuCategory category)
        {
            switch (category)
            {
                case MenuCategory.Lunch:
                    amountLunch++;
                    lblAmountLunch.Content = amountLunch;
                    break;
                case MenuCategory.Dinner:
                    amountDinner++;
                    lblAmountDinner.Content = amountDinner;
                    break;
                case MenuCategory.Drink:
                    amountDrinks++;
                    lblAmountDrinks.Content = amountDrinks;
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
                    amountLunch--;
                    lblAmountLunch.Content = amountLunch;
                    break;
                case MenuCategory.Dinner:
                    amountDinner--;
                    lblAmountDinner.Content = amountDinner;
                    break;
                case MenuCategory.Drink:
                    amountDrinks--;
                    lblAmountDrinks.Content = amountDrinks;
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void DisableButtons()
        {
            btnDecreaseItem.IsEnabled = false;
            btnIncreaseItem.IsEnabled = false;
            btnRemoveItem.IsEnabled = false;
        }

        private void HandleException(Exception exp) => MessageBox.Show("Gosh darnit! " + exp.Message, "Something unexpected happened", MessageBoxButton.OK, MessageBoxImage.Exclamation);
    }
}

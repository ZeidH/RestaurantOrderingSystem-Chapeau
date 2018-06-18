using ChapeauLogic;
using ChapeauModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ChapeauUI
{
	/// <summary>
	/// Interaction logic for Kitchenview_UI.xaml
	/// </summary>
	public partial class ProcessingView_UI : Page
	{
		private Processing_Service service = new Processing_Service();

		// should we show kitchen or bar orders?
		private PreparationLocation preparationLocation;

		// are we on ready or running tab?
		private OrderStatus orderStatus;

		// if we have order side panel open, which order?
		private Order openOrder;

		private DispatcherTimer timer;

		public ProcessingView_UI(PreparationLocation preparationLocation)
		{
			InitializeComponent();
			Animation.AnimateIn(this, 1);

			// start the clock timer (it ticks once a second)
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(15);
			timer.Tick += ClockTick;
			timer.Start();

			// this is determined by the logged in user, it never changes
			this.preparationLocation = preparationLocation;

			UpdateClock();
			ShowRunningOrders();
			UpdateStatusOverview();
		}

		#region event handlers

		private void ClockTick(object sender, EventArgs args)
		{
			UpdateClock();

			// reload the list in case someone else did something to it (e.g. placed new order)
			LoadOrders();

			// reload status in case some one else changed table/order totals
			UpdateStatusOverview();
		}

		private void ReadyTabButtonClicked(object sender, RoutedEventArgs e)
		{
			ShowReadyOrders();
		}

		private void RunningTabButtonClicked(object sender, RoutedEventArgs e)
		{
			ShowRunningOrders();
		}

		private void SelectOrderFromList(object sender, MouseButtonEventArgs e)
		{
			// find the selected order from the list
			Order selectedOrder = (Order)orderListView.SelectedValue;

			// it could be that the user clicked outside the items, then there is no
			// selected order. In that case - do nothing.
			if (selectedOrder != null)
			{
				ShowSidePanel(selectedOrder.Id);
			}
		}

		private void CloseButtonClicked(object sender, RoutedEventArgs e)
		{
			HideSidePanel();
		}

		private void MarkAsReadyClicked(object sender, RoutedEventArgs e)
		{
			service.MarkOrderAndTableAsReady(openOrder);

			// since the order is now no longer running, hide the
			// side panel and reload the list to remove it (user is still on the
			// running tab)
			HideSidePanel();
			LoadOrders();
			UpdateStatusOverview();
		}

		private void LogoutButtonClicked(object sender, RoutedEventArgs e)
		{
			timer.Stop();
			NavigationService.Navigate(new Login_UI());
		}

		#endregion

		#region state changes
		
		private void ShowRunningOrders()
		{
			// make sure the tab checked state is correct (when clicking
			// the same button twice, by default it gets unchecked, but
			// that's not correct in this case)
			showRunningOrdersButton.IsChecked = true;
			showReadyOrdersButton.IsChecked = false;

			// the ready button is available when looking at running orders
			readyButton.IsEnabled = true;

			// if the side panel was open we should close it
			HideSidePanel();

			// set the order status for what we're looking at (so the refresh
			// knows what to load)
			orderStatus = OrderStatus.Processing;

			// load the order list again
			LoadOrders();
		}

		private void ShowReadyOrders()
		{
			// make sure the tab checked state is correct
			showRunningOrdersButton.IsChecked = false;
			showReadyOrdersButton.IsChecked = true;

			// on the ready view you cannot mark orders as ready, because they already are
			readyButton.IsEnabled = false;

			// if the side panel was open we should close it
			HideSidePanel();

			// set the order status for what we're looking at (so the refresh
			// knows what to load)
			orderStatus = OrderStatus.Ready;

			// load the order list again
			LoadOrders();
		}

		private void UpdateClock()
		{
			// get date+time in current timezone
			DateTime time = DateTime.Now;
			timeLabel.Text = time.ToString("dd-MM-yy\nHH:mm");
		}

		private void ShowSidePanel(int orderId)
		{
			Order orderWithDetails;

			// fetch the order details from the database
			try
			{
				orderWithDetails = service.GetOrderDetails(
				   orderId,
				   orderStatus,
				   preparationLocation
			   );
			}
			catch (ApplicationException ex)
			{
				ShowErrorMessage(ex);
				return;
			}

			// unhide the panel by setting its width to 60%
			sidePanelGridColumn.Width = new GridLength(6.0, GridUnitType.Star);

			// set order status label values
			orderIdLabel.Text = "Order: " + orderWithDetails.Id;
			tableNrLabel.Text = "Table: " + orderWithDetails.TableId;
			orderTimeLabel.Text = orderWithDetails.LastOrderTime.ToString("HH:mm");
			employeeNameLabel.Text = orderWithDetails.EmployeeName;

			// set data source for the table
			orderItemsListView.ItemsSource = orderWithDetails.Items;

			// remember which order is open so we have that information later
			openOrder = orderWithDetails;
		}

		private void HideSidePanel()
		{
			// hide the side panel by setting its width to 0
			sidePanelGridColumn.Width = new GridLength(0);

			// since the panel is closed there is no open order anymore
			openOrder = null;

			// all list items must be unselected
			orderListView.UnselectAll();
		}

		private void UpdateStatusOverview()
		{
			RestaurantStatus status;

			try
			{
				status = service.GetRestaurantStatus(preparationLocation);
			}
			catch (ApplicationException ex)
			{
				ShowErrorMessage(ex);
				return;
			}

			tableStatusLabel.Text = "Tables: " + status.RunningTables + "/" + status.TotalTables;
			orderStatusLabel.Text = "Orders: " + status.RunningOrders + "/" + status.TotalOrders;
		}

		private void LoadOrders()
		{
			List<Order> orders;

			try
			{
				orders = service.GetOrders(orderStatus, preparationLocation);
			}
			catch (ApplicationException ex)
			{
				ShowErrorMessage(ex);
				return;
			}

			// since we will change the items, the selection will be lost, so
			// we remember the item that was selected and find it again after
			// adding the new ones

			int selectedOrderId = GetSelectedOrderId();

			// bind items in the list view to the orders from the database
			orderListView.ItemsSource = orders;

			ReselectOrder(orders, selectedOrderId);
		}

		private int GetSelectedOrderId()
		{
			int selectedOrderId = -1;

			if (orderListView.SelectedValue != null)
			{
				Order selectedOrder = (Order)orderListView.SelectedValue;
				selectedOrderId = selectedOrder.Id;
			}

			return selectedOrderId;
		}

		private void ReselectOrder(List<Order> orders, int selectedOrderId)
		{
			// see if we can find an order that has the same id as the one
			// that was selected before
			foreach (Order order in orders)
			{
				if (order.Id == selectedOrderId)
				{
					// found an order with the same id as the one that was
					// selected before, so we select it again
					orderListView.SelectedValue = order;

					// there should only be one so break
					break;
				}
			}
		}

		#endregion

		private void ShowErrorMessage(Exception error)
		{
			MessageBox.Show(error.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}

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

		public ProcessingView_UI(PreparationLocation preparationLocation)
		{
			InitializeComponent();

			// don't use the arguments so just pass null for them
			ShowRunningOrders(null, null);

			UpdateStatusOverview();

			// start the clock timer (it ticks once a second)
			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);

			// call the UpdateClock method every time the timer ticks
			timer.Tick += UpdateClock;

			// call it right away otherwise it won't show for a second
			// don't use the arguments so just pass null for them
			UpdateClock(null, null);

			timer.Start();
			this.preparationLocation = preparationLocation;
		}

		#region event handlers

		private void UpdateClock(object sender, EventArgs args)
		{
			// get date+time in current timezone
			DateTime time = DateTime.Now;
			timeLabel.Text = time.ToString("dd-MM-yy\nhh:mm");
		}

		private void ShowReadyOrders(object sender, RoutedEventArgs e)
		{
			showRunningOrdersButton.IsChecked = false;
			showReadyOrdersButton.IsChecked = true;

			readyButton.IsEnabled = false;

			HideSidePanel();

			orderStatus = OrderStatus.Ready;

			LoadOrders();
		}

		private void ShowRunningOrders(object sender, RoutedEventArgs e)
		{
			showRunningOrdersButton.IsChecked = true;
			showReadyOrdersButton.IsChecked = false;

			readyButton.IsEnabled = true;

			HideSidePanel();

			orderStatus = OrderStatus.Processing;

			LoadOrders();
		}

		private void SelectOrderFromList(object sender, MouseButtonEventArgs e)
		{
			Order selectedOrder = (Order)orderListView.SelectedValue;

			if (selectedOrder != null)
			{
				ShowSidePanel(selectedOrder.Id, selectedOrder.LastOrderStatus);
			}
		}

		private void CloseButtonClicked(object sender, RoutedEventArgs e)
		{
			HideSidePanel();
		}

		private void MarkItemsAsReady(object sender, RoutedEventArgs e)
		{
			service.MarkOrderAsReady(openOrder.Id);
			HideSidePanel();
			LoadOrders();
		}

		#endregion

		#region state changes

		private void ShowSidePanel(int orderId, OrderStatus status)
		{
			Order orderWithDetails = service.GetOrderDetails(
				orderId,
				status,
				preparationLocation
			);

			sidePanelGridColumn.Width = new GridLength(4.0, GridUnitType.Star);

			orderIdLabel.Text = "#" + orderWithDetails.Id;
			tableNrLabel.Text = "Table: " + orderWithDetails.TableId;
			orderTimeLabel.Text = orderWithDetails.LastOrderTime.ToString("hh:mm");
			employeeNameLabel.Text = orderWithDetails.EmployeeName;

			orderItemsListView.ItemsSource = orderWithDetails.Items;

			openOrder = orderWithDetails;
		}

		private void HideSidePanel()
		{
			sidePanelGridColumn.Width = new GridLength(0);
			openOrder = null;
		}

		private void UpdateStatusOverview()
		{
			var status = service.GetRestaurantStatus();

			tableStatusLabel.Text = "Tables: " + status.BusyTables + "/" + status.TotalTables;
			orderStatusLabel.Text = "Orders: " + status.BusyOrders + "/" + status.TotalOrders;
		}

		private void LoadOrders()
		{
			List<Order> orders = service.GetOrders(orderStatus, preparationLocation);

			// bind items in the list view to the orders from the daterbase
			orderListView.ItemsSource = orders;
		}

		#endregion
	}
}

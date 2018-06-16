using ChapeauLogic;
using ChapeauModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ChapeauUI
{
	/// <summary>
	/// Interaction logic for Kitchenview_UI.xaml
	/// </summary>
	public partial class ProcessingView_UI : Page
	{
		private Processing_Service service = new Processing_Service();
		private PreparationLocation preparationLocation;

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

		private void UpdateStatusOverview()
		{
			var status = service.GetRestaurantStatus();

			tableStatusLabel.Text = "Tables: " + status.BusyTables + "/" + status.TotalTables;
			orderStatusLabel.Text = "Orders: " + status.BusyOrders + "/" + status.TotalOrders;
		}

		private void LoadOrders(OrderStatus status)
		{
			List<Order> orders = service.GetOrders(status, preparationLocation);

			// bind items in the list view to the orders from the daterbase
			orderListView.ItemsSource = orders;
		}

		private void UpdateClock(object sender, EventArgs args)
		{
			// get date+time in current timezone
			DateTime time = DateTime.Now;
			timeLabel.Text = time.ToString("dd-MM-yy\nhh:mm");
		}

		private void ShowReadyOrders(object sender, System.Windows.RoutedEventArgs e)
		{
			showRunningOrdersButton.IsChecked = false;
			showReadyOrdersButton.IsChecked = true;
			LoadOrders(OrderStatus.Ready);
		}

		private void ShowRunningOrders(object sender, System.Windows.RoutedEventArgs e)
		{
			showRunningOrdersButton.IsChecked = true;
			showReadyOrdersButton.IsChecked = false;
			LoadOrders(OrderStatus.Processing);
		}

		private void SelectOrderFromList(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			Order selectedOrder = (Order)orderListView.SelectedValue;

			Order orderWithDetails = service.GetOrderDetails(
				selectedOrder.Id,
				selectedOrder.LastOrderStatus,
				preparationLocation
			);

			sidePanelGridColumn.Width = new GridLength(4.0, GridUnitType.Star);

			orderIdLabel.Text = "#" + orderWithDetails.Id;
			tableNrLabel.Text = "Table: " + orderWithDetails.TableId;
			orderTimeLabel.Text = orderWithDetails.LastOrderTime.ToString("hh:mm");
			employeeNameLabel.Text = orderWithDetails.EmployeeName;

			orderItemsListView.ItemsSource = orderWithDetails.Items;
		}

		private void CloseSidePanel(object sender, RoutedEventArgs e)
		{
			sidePanelGridColumn.Width = new GridLength(0);
		}
	}
}

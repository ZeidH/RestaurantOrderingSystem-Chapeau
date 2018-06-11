using ChapeauLogic;
using ChapeauModel;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ChapeauUI
{
	/// <summary>
	/// Interaction logic for Kitchenview_UI.xaml
	/// </summary>
	public partial class Kitchenview_UI : Page
    {
		private Processing_Service service = new Processing_Service();

		public Kitchenview_UI()
        {
            InitializeComponent();

			List<Order> orders = service.GetOrders(OrderStatus.Processing, PreparationLocation.Kitchen);

			ordersListView.ItemsSource = orders;
        }
    }
}

using System.Collections.Generic;
using ChapeauModel;
using ChapeauDAL;
using System;
using System.Data.SqlClient;

namespace ChapeauLogic
{
	public class Processing_Service
	{
		Processing_DAO processingDao = new Processing_DAO();

		public List<Order> GetOrders(OrderStatus status, PreparationLocation location)
		{
			try
			{
				return processingDao.Db_get_orders_by_status_and_location(status, location);
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Cannot connect to server\nAn error log has been saved in the program folder \n Press 'OK' to retry", ex);
			}
		}

		public Order GetOrderDetails(int orderId, OrderStatus status, PreparationLocation location)
		{
			try
			{
				return processingDao.Db_get_order_items(orderId, status, location);
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Cannot connect to server\nAn error log has been saved in the program folder \n Press 'OK' to retry", ex);
			}
		}

		public void MarkOrderAndTableAsReady(Order order)
		{
			try
			{
				CheckOrderItemsReady(order);
				CheckTableReady(order);
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Cannot connect to server\nAn error log has been saved in the program folder \n Press 'OK' to retry", ex);
			}
		}

		private void CheckOrderItemsReady(Order order)
		{
			// there could be other order items on this order (e.g. from bar view if
			// this is a kitchen order) that should not be marked as ready, so the simplest
			// way to do that is to mark the items one by one
			foreach (OrderItem item in order.Items)
			{
				processingDao.Db_mark_order_ready(order.Id, item.Item.Item_id);
			}
		}

		private void CheckTableReady(Order order)
		{
			if (processingDao.Db_is_table_ready(order.Id))
			{
				processingDao.Db_check_table_ready(order.TableId);
			}
		}

		public RestaurantStatus GetRestaurantStatus(PreparationLocation location)
		{
			try
			{
				RestaurantStatus status = new RestaurantStatus();

				status.TotalTables = processingDao.Db_get_number_of_tables();
				status.RunningTables = processingDao.Db_get_number_of_running_tables();

				status.TotalOrders = processingDao.Db_get_number_of_orders(location);
				status.RunningOrders = processingDao.Db_get_number_of_running_orders(location);

				return status;
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Cannot connect to server\nAn error log has been saved in the program folder \n Press 'OK' to retry", ex);
			}
		}

	}
}

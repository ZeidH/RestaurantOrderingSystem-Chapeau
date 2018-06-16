using System.Collections.Generic;
using ChapeauModel;
using ChapeauDAL;
using System;

namespace ChapeauLogic
{
	public class Processing_Service
    {
		Processing_DAO processingDao = new Processing_DAO();

		public List<Order> GetOrders(OrderStatus status, PreparationLocation location)
		{
			return processingDao.Db_get_orders_by_status_and_location(status, location);
		}

		public Order GetOrderDetails(int orderId, OrderStatus status, PreparationLocation location)
		{
			Order result = processingDao.Db_get_order_items(orderId, status, location);

			// we must find the last order detail item

			DateTime maxTime = DateTime.MinValue;
			
			foreach(OrderItem item in result.Items)
			{
				if (item.Time > maxTime)
				{
					maxTime = item.Time;
				}
			}

			result.LastOrderTime = maxTime;

			return result;
		}

		public void MarkOrderItemAsReady(int orderId, int itemId)
		{
			processingDao.Db_mark_order_ready(orderId, itemId);
		}

		public RestaurantStatus GetRestaurantStatus()
		{
			RestaurantStatus status = processingDao.Db_get_restaurant_table_status();

			return status;
		}

    }
}

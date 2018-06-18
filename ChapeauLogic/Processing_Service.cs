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

		public void MarkOrderAsReady(Order order)
		{
			foreach(OrderItem item in order.Items)
			{
				processingDao.Db_mark_order_ready(order.Id, item.Item.Item_id);
			}
            CheckTableReady(order);
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
			RestaurantStatus status = new RestaurantStatus();

			status.TotalTables = processingDao.Db_get_number_of_tables();
			status.RunningTables = processingDao.Db_get_number_of_running_tables();

			status.TotalOrders = processingDao.Db_get_number_of_orders(location);
			status.RunningOrders = processingDao.Db_get_number_of_running_orders(location);

			return status;
		}

    }
}

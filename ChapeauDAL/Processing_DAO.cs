using System.Data.SqlClient;
using System.Data;
using ChapeauModel;
using System.Collections.Generic;
using System;

namespace ChapeauDAL
{
	public class Processing_DAO : Base
	{
		// todo D: change from datatable to Order

		/// <summary>
		/// Used to get the kitchen/bar ready/processing order list
		/// </summary>
		public List<Order> Db_get_orders_by_status_and_location(OrderStatus status, PreparationLocation location)
		{
			string query = @"
				SELECT DISTINCT ([ORDER].order_id) AS order_id, table_id, emp_firstName
				FROM [ORDER]
				LEFT JOIN [EMPLOYEE] on [ORDER].emp_id = [EMPLOYEE].emp_id
				LEFT JOIN [ORDER_LIST] on [ORDER_LIST].order_id = [ORDER].order_id
				LEFT JOIN [ITEM] on [ORDER_LIST].item_id = [ITEM].item_id
				WHERE [ORDER_LIST].order_status = @orderstatus
					AND [ITEM].item_prep_location = @preploc
			";

			SqlParameter[] sqlParameters = new SqlParameter[2];

			sqlParameters[0] = new SqlParameter("@orderstatus", SqlDbType.SmallInt)
			{
				Value = status
			};
			sqlParameters[1] = new SqlParameter("@preploc", SqlDbType.SmallInt)
			{
				Value = location
			};

			DataTable dataTable = ExecuteSelectQuery(query, sqlParameters);

			List<Order> result = ReadOrders(dataTable);

			return result;
		}

		private static List<Order> ReadOrders(DataTable dataTable)
		{
			List<Order> result = new List<Order>();

			foreach (DataRow row in dataTable.Rows)
			{
				// create instance of order class to put the values in
				Order order = new Order();

				order.Id = (int)row["order_id"];
				order.EmployeeName = (string)row["emp_firstName"];
				order.TableId = (int)row["table_id"];
				order.Items = new List<OrderItem>(); // empty because we didn't get them from the db

				result.Add(order);
			}

			return result;
		}

		/// <summary>
		/// Used to get the overview of items for an order
		/// </summary>
		public Order Db_get_order_items(int orderId, OrderStatus status, PreparationLocation location)
		{
			string query = @"
				SELECT 
					[ORDER].order_id AS order_id,
					order_time,
					order_status,
					table_id,
					emp_firstName,
					[ITEM].item_id AS item_id,
					item_name,
					item_amount,
					item_comment
				FROM [ORDER]
				LEFT JOIN [EMPLOYEE] on [ORDER].emp_id = [EMPLOYEE].emp_id
				LEFT JOIN [ORDER_LIST] on [ORDER].order_id = [ORDER_LIST].order_id
				LEFT JOIN [ITEM] on [ORDER_LIST].item_id = [ITEM].item_id
				WHERE [ORDER].order_id = @orderid
					AND [ORDER_LIST].order_status = @orderstatus
					AND [ITEM].item_prep_location = @preploc
			";

			SqlParameter[] sqlParameters = new SqlParameter[3];

			sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
			{
				Value = orderId
			};
			sqlParameters[1] = new SqlParameter("@orderstatus", SqlDbType.SmallInt)
			{
				Value = status
			};
			sqlParameters[2] = new SqlParameter("@preploc", SqlDbType.SmallInt)
			{
				Value = location
			};

			DataTable table = ExecuteSelectQuery(query, sqlParameters);

			Order order = new Order();
			order.Items = new List<OrderItem>();

			foreach (DataRow row in table.Rows)
			{
				// these will be the same for each row so we may as well set them every time
				order.TableId = (int)row["table_id"];
				order.Id = (int)row["order_id"];
				order.EmployeeName = (string)row["emp_firstName"];

				OrderItem item = ReadOrderitem(row);
				order.Items.Add(item);
			}

			return order;
		}

		/// <summary>
		/// Reads an order item from a data row
		/// </summary>
		private OrderItem ReadOrderitem(DataRow row)
		{
			Item item = new Item();

			item.Item_id = (int)row["item_id"];
			item.Name = (string)row["item_name"];

			OrderItem orderItem = new OrderItem();

			orderItem.Item = item;
			orderItem.Status = (OrderStatus)row["order_status"];
			orderItem.Comment = (string)row["item_comment"];
			orderItem.Time = (DateTime)row["order_time"];

			return orderItem;
		}

		public DataTable Db_mark_order_ready(int orderId, int itemId)
		{
			string query = @"
				UPDATE [ORDER_LIST] SET order_status = @orderstatus WHERE order_id = @orderid AND item_id = @itemid
			";

			SqlParameter[] sqlParameters = new SqlParameter[2];
			sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
			{
				Value = orderId
			};
			sqlParameters[1] = new SqlParameter("@itemid", SqlDbType.Int)
			{
				Value = itemId
			};

			return ExecuteSelectQuery(query, sqlParameters);
		}

	}
}

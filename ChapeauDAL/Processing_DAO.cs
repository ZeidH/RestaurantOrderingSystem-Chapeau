using System.Data.SqlClient;
using System.Data;
using ChapeauModel;

namespace ChapeauDAL
{
    public class Processing_DAO : Base
    {
		// todo D: change from datatable to Order

		public DataTable Db_get_orders_by_status_and_location(OrderStatus status, PreparationLocation location)
		{
			string query = @"
				SELECT [ORDER].order_id, table_id, emp_firstName
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

			return ExecuteSelectQuery(query, sqlParameters);
		}

		public DataTable Db_get_order_items(int orderId)
		{
			string query = @"
				SELECT [ORDER].order_id, table_id, emp_firstName, item_name, order_time
				FROM [ORDER]
				LEFT JOIN [EMPLOYEE] on [ORDER].emp_id = [EMPLOYEE].emp_id
				LEFT JOIN [ORDER_LIST] on [ORDER_LIST].order_id = [ORDER].order_id
				LEFT JOIN [ITEM] on [ORDER_LIST].item_id = [ITEM].item_id
				WHERE [ORDER].order_id = @orderid
			";

			SqlParameter[] sqlParameters = new SqlParameter[1];

			sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
			{
				Value = orderId
			};

			return ExecuteSelectQuery(query, sqlParameters);
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

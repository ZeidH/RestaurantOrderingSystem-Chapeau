using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ChapeauModel;

namespace ChapeauDAL
{
    public class Processing_DAO : Base
    {

		public DataTable Db_get_orders_by_status(OrderStatus status, PreparationLocation location)
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

		public DataTable Db_mark_order_ready(int orderId)
		{
			string query = @"
				UPDATE [ORDER_LIST] SET order_status = @orderstatus WHERE order_id = @orderid
			";

			SqlParameter[] sqlParameters = new SqlParameter[2];
			sqlParameters[0] = new SqlParameter("@orderstatus", SqlDbType.SmallInt)
			{
				Value = OrderStatus.Ready
			};
			sqlParameters[1] = new SqlParameter("@orderstatus", SqlDbType.Int)
			{
				Value = orderId
			};

			return ExecuteSelectQuery(query, sqlParameters);
		}

    }
}

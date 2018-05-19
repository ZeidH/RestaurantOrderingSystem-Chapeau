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
    public class Payment_DAO : Base
    {
        private DataTable Db_Payment(int order_id)
        {
            string query = string.Format("SELECT item_id AS order_items FROM ORDER_LIST WHERE order_id = @orderid");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
            {
                Value = order_id
            };
            return ExecuteSelectQuery(query, sqlParameters);
        }
    }
}

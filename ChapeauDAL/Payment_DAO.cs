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
    //Also inherit Customer and Item
    public class Payment_DAO : Base
    {
        public void SetPayment(Payment payment)
        {
            string query = string.Format("INSERT INTO PAYMENT VALUES(@tip, @price, @method, @comment) WHERE order_id = @orderid");
            SqlParameter[] sqlParameters = new SqlParameter[5];
            sqlParameters[0] = new SqlParameter("@orderid", System.Data.SqlDbType.Int)
            {
                Value = payment.Order_id
            };
            sqlParameters[1] = new SqlParameter("@tip", System.Data.SqlDbType.Float)
            {
                Value = payment.Tip
            };
            sqlParameters[2] = new SqlParameter("@price", System.Data.SqlDbType.Float)
            {
                Value = payment.Price
            };
            sqlParameters[3] = new SqlParameter("@method", System.Data.SqlDbType.SmallInt)
            {
                Value = payment.Method
            };
            sqlParameters[4] = new SqlParameter("@comment", System.Data.SqlDbType.NVarChar)
            {
                Value = payment.Comment
            };

            ExecuteEditQuery(query, sqlParameters);
        }
    }
}

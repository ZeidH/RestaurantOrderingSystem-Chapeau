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
        public void Db_set_payment(Payment payment)
        {
            string query = string.Format("INSERT INTO PAYMENT VALUES(@tip, @price, @method, @comment) WHERE order_id = @orderid");
            SqlParameter[] sqlParameters = new SqlParameter[5];
            sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
            {
                Value = payment.Order_id
            };
            sqlParameters[1] = new SqlParameter("@tip", SqlDbType.Float)
            {
                Value = payment.Tip
            };
            sqlParameters[2] = new SqlParameter("@price", SqlDbType.Float)
            {
                Value = payment.Price
            };
            sqlParameters[3] = new SqlParameter("@method", SqlDbType.SmallInt)
            {
                Value = payment.Method
            };
            sqlParameters[4] = new SqlParameter("@comment", SqlDbType.NVarChar)
            {
                Value = payment.Comment
            };

            ExecuteEditQuery(query, sqlParameters);
        }
        public DataTable Db_get_drink_vat(int item_id, Item item)
        {
            string query = string.Format("SELECT drink_vat FROM DRINK WHERE drink_id = @item_id");

            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@item_id", SqlDbType.Int)
            {
                Value = item_id
            };

            DataTable table = ExecuteSelectQuery(query, sqlParameters);
            return table;
        }
    }
}

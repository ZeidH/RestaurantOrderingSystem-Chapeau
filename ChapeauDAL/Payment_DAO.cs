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
        private void SetTip(int order_id, float tip)
        {
            string query = string.Format("INSERT INTO PAYMENT VALUES(@tip) WHERE order_id = @orderid");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@orderid", System.Data.SqlDbType.Int)
            {
                Value = order_id
            };
            sqlParameters[1] = new SqlParameter("@tip", System.Data.SqlDbType.Float)
            {
                Value = tip
            };

            ExecuteEditQuery(query, sqlParameters);
        }
        private void SetPayMethod(int order_id, PayMethod method)
        {
            string query = string.Format("INSERT INTO PAYMENT VALUES(@method) WHERE order_id = @orderid");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@orderid", System.Data.SqlDbType.Int)
            {
                Value = order_id
            };
            sqlParameters[1] = new SqlParameter("@method", System.Data.SqlDbType.SmallInt)
            {
                Value = method
            };

            ExecuteEditQuery(query, sqlParameters);
        }
        private void SetComment(int order_id, string comment)
        {
            // Query to insert order_comment
            string query = string.Format("INSERT INTO [ORDER] VALUES(@comment) WHERE order_id = @orderid");
            // Parameters array
            SqlParameter[] sqlParameters = new SqlParameter[2];

            // Parameters Values
            sqlParameters[0] = new SqlParameter("@orderid", System.Data.SqlDbType.Int)
            {
                Value = order_id
            };
            sqlParameters[1] = new SqlParameter("@comment", System.Data.SqlDbType.NVarChar)
            {
                Value = comment
            };

            //Call :base method
            ExecuteEditQuery(query, sqlParameters);
        }
    }
}

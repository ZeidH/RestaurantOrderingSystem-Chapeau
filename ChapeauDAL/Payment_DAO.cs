using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ChapeauModel;

namespace ChapeauDAL
{
    //Also inherit Customer and Item
    public class Payment_DAO : Base
    {
        private void SetTip(int order_id, float tip)
        {
            SqlConnection connection = OpenConnectionDB();

            //Query - Insert the tip into the database
            sb.Append("INSERT INTO PAYMENT VALUES(@tip) WHERE order_id = @order_id");

            string sql = sb.ToString();

            //Execute Query
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@order_id", System.Data.SqlDbType.Int).Value = order_id;
                command.Parameters.Add("@tip", System.Data.SqlDbType.Float).Value = tip;
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
        private void SetPayMethod(int order_id, PayMethod method)
        {
            SqlConnection connection = OpenConnectionDB();

            //Query - Insert the type of payment method into the database
            sb.Append("INSERT INTO PAYMENT VALUES(@method) WHERE order_id = @order_id");

            string sql = sb.ToString();

            //Execute Query
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@order_id", System.Data.SqlDbType.Int).Value = order_id;
                command.Parameters.Add("@method", System.Data.SqlDbType.SmallInt).Value = method;
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
        private void SetComment(int order_id, string comment)
        {
            SqlConnection connection = OpenConnectionDB();

            //Query - Insert the order_comment into the database
            sb.Append("INSERT INTO PAYMENT VALUES(@comment) WHERE order_id = @order_id");

            string sql = sb.ToString();

            //Execute Query
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@order_id", System.Data.SqlDbType.Int).Value = order_id;
                command.Parameters.Add("@comment", System.Data.SqlDbType.NVarChar).Value = comment;
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}

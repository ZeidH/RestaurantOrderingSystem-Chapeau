using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ChapeauDAL
{
    class Orderview_DAO : Connection
    {
        private void Db_add_item(int order_id, string item_comment, DateTime order_time, OrderStatus order_status, int item_amount, int item_id)
        {
            SqlConnection connection = OpenConnectionDB();
           
            //Insert the order items into the database
            sb.Append("INSERT INTO ORDER_LIST (order_id, item_comment, order_time, order_status, item_amount, item_id) " +
                "VALUES(@orderid, @itemcomment, @ordertime, @orderstatus, @itemamount, @itemid)");
            string sql1 = sb.ToString();

            //Execute Query
            using (SqlCommand command = new SqlCommand(sql1, connection))
            {
                command.Parameters.Add("@orderid", System.Data.SqlDbType.Int).Value = order_id;
                command.Parameters.Add("@itemcomment", System.Data.SqlDbType.NVarChar).Value = item_comment;
                command.Parameters.Add("@ordertime", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                command.Parameters.Add("@orderstatus", System.Data.SqlDbType.SmallInt).Value = order_status;
                command.Parameters.Add("@itemamount", System.Data.SqlDbType.Int).Value = item_amount;
                command.Parameters.Add("@itemid", System.Data.SqlDbType.Int).Value = item_id;
                command.ExecuteScalar();
            }

            // Decrease stock
            sb.Append("UPDATE ITEM SET item_stock = item_stock - @itemamount WHERE item_id = @itemid");
            string sql2 = sb.ToString();

            using (SqlCommand command = new SqlCommand(sql2, connection))
            {
                command.Parameters.Add("@itemamount", System.Data.SqlDbType.Int).Value = item_amount;
                command.Parameters.Add("@orderid", System.Data.SqlDbType.Int).Value = order_id;
                command.ExecuteScalar();
            }
            connection.Close();
        }

        private void Db_add_order(int table_id, int emp_id)
        {
            // make a new order
            //INSERT INTO[ORDER] (order_comment, table_id, emp_id) VALUES('', 1, 1)
            SqlConnection connection = OpenConnectionDB();

            //Insert a new order into the database
            sb.Append("INSERT INTO[ORDER] (table_id, emp_id) VALUES(@tableid, @empid)");
            string sql = sb.ToString();

            //Execute Query
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@tableid", System.Data.SqlDbType.Int).Value = table_id;
                command.Parameters.Add("@empid", System.Data.SqlDbType.Int).Value = emp_id;
                command.ExecuteScalar();
            }
        }

        private void Db_delete_order(int order_id)
        {
            SqlConnection connection = OpenConnectionDB();

            //Delete order
            sb.Append("DELETE FROM [ORDER] WHERE order_id = @orderid");
            string sql = sb.ToString();

            //Execute Query
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@orderid", System.Data.SqlDbType.Int).Value = order_id;
                command.ExecuteScalar();
            }
        }
    }
}

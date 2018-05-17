using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ChapeauModel;

namespace ChapeauDAL
{
    public class Payment_DAO : Base
    {
        public Payment_DAO(int order_id)
        {
            Db_Payment(order_id);
        }
        private void Db_Payment(int order_id)
        {
            SqlConnection connection = OpenConnectionDB();

            //Query - Get all the names and which room they are in
            sb.Append("SELECT item_id AS order_items FROM ORDER_LIST WHERE order_id = @order_id");

            string sql = sb.ToString();

            //Execute Query
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@order_id", System.Data.SqlDbType.Int).Value = order_id;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ReadInfoQuery(reader);
                }
                reader.Close();
            }
            connection.Close();
        }
        private void ReadInfoQuery(SqlDataReader reader)
        {
            //Read and put in model
        }
    }
}

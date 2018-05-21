using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ChapeauModel;

namespace ChapeauDAL
{
    public class Item_DAO : Order_DAO
    {
        public void Db_add_item(Item item)
        {
            string query = string.Format("INSERT INTO ORDER_LIST (order_id, item_comment, order_time, order_status, item_amount, item_id) " +
                    "VALUES(@orderid, @itemcomment, @ordertime, @orderstatus, @itemamount, @itemid)");
            SqlParameter[] sqlParameters = new SqlParameter[6];
            sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
            {
                Value = item.Order_id
            };
            sqlParameters[1] = new SqlParameter("@itemcomment", SqlDbType.NVarChar)
            {
                Value = item.Comment
            };
            sqlParameters[2] = new SqlParameter("@ordertime", SqlDbType.DateTime)
            {
                Value = item.Order_time
            };
            sqlParameters[3] = new SqlParameter("@orderstatus", SqlDbType.SmallInt)
            {
                Value = item.Order_status
            };
            sqlParameters[4] = new SqlParameter("@itemamount", SqlDbType.Int)
            {
                Value = item.Amount
            };
            sqlParameters[5] = new SqlParameter("@itemid", SqlDbType.Int)
            {
                Value = item.Item_id
            };
            ExecuteEditQuery(query, sqlParameters);
        }

        public void Db_update_stock(Item item)
        {
            string query = string.Format("UPDATE ITEM SET item_stock = @itemstock WHERE item_id = @itemid");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@itemstock", SqlDbType.Int)
            {
                Value = item.Stock
            };
            sqlParameters[1] = new SqlParameter("@itemid", SqlDbType.Int)
            {
                Value = item.Item_id
            };
            ExecuteEditQuery(query, sqlParameters);
        }

        public DataTable Db_select_status(int order_id)
        {
            string query = string.Format("SELECT order_id, order_status FROM [ORDER_LIST] WHERE order_id = @orderid");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
            {
                Value = order_id
            };
            return ExecuteSelectQuery(query, sqlParameters);
        }

        public DataTable Db_select_items(int order_id)
        {  
            string query = string.Format("SELECT order_id, item_comment, order_time, order_status, item_amount, item_id FROM [ORDER_LIST] WHERE order_id = @orderid");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
            {
                Value = order_id
            };
            return ExecuteSelectQuery(query, sqlParameters);
        }

        public DataTable Db_select_menu_items(MenuCategory menu, int category)
        {
            string query = string.Format($"SELECT i.item_id, i.item_name, i.item_cost, i.item_stock, x.{@menu}_category " +
                $"FROM ITEM AS i LEFT JOIN {@menu} AS x ON i.item_id = x.{@menu}_id WHERE x.{@menu}_category = @category");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@category", SqlDbType.SmallInt)
            {
                Value = category
            };
            return ExecuteSelectQuery(query, sqlParameters);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ChapeauModel;
using System.Transactions;
/// <summary>
/// We need to stop making queries for ourselves and make them so that everyone can be able to use it. Get whole menu query instead of get part of menu query.
/// </summary>
namespace ChapeauDAL
{
    public class Item_DAO : Order_DAO
    {
        public void Db_add_item(List<OrderItem> orderItems)
        {
            string query = string.Format("INSERT INTO ORDER_LIST (order_id, item_comment, order_time, order_status, item_amount, item_id) " +
            "VALUES(@orderid, @itemcomment, @ordertime, @orderstatus, @itemamount, @itemid)");
            try
            {
                foreach (OrderItem orderItem in orderItems)
                {
                    SqlParameter[] sqlParameter = new SqlParameter[6];
                    sqlParameter[0] = new SqlParameter("@orderid", SqlDbType.Int)
                    {
                        Value = orderItem.Item.Order_id
                    };
                    sqlParameter[1] = new SqlParameter("@itemcomment", SqlDbType.NVarChar)
                    {
                        Value = orderItem.Comment
                    };
                    sqlParameter[2] = new SqlParameter("@ordertime", SqlDbType.DateTime)
                    {
                        Value = orderItem.Time
                    };
                    sqlParameter[3] = new SqlParameter("@orderstatus", SqlDbType.SmallInt)
                    {
                        Value = orderItem.Status
                    };
                    sqlParameter[4] = new SqlParameter("@itemamount", SqlDbType.Int)
                    {
                        Value = orderItem.Amount
                    };
                    sqlParameter[5] = new SqlParameter("@itemid", SqlDbType.Int)
                    {
                        Value = orderItem.Item.Item_id
                    };
                    ExecuteEditQuery(query, sqlParameter);
                    Db_update_stock(orderItem);
                }
            }
            catch (Exception e)
            {
                ErrorFilePrint print = new ErrorFilePrint();
                print.ErrorLog(e);
                throw;
            }

        }

        public void Db_update_stock(OrderItem orderItem)
        {
            string query = string.Format("UPDATE ITEM SET item_stock = @itemstock WHERE item_id = @itemid");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@itemstock", SqlDbType.Int)
            {
                Value = orderItem.Item.Stock
            };
            sqlParameters[1] = new SqlParameter("@itemid", SqlDbType.Int)
            {
                Value = orderItem.Item.Item_id
            };
            ExecuteEditQuery(query, sqlParameters);
        }

        public List<Item> Db_select_meu()
        {
            string query = "SELECT i.item_id, i.item_name, i.item_cost, i.item_stock, d.drink_category, l.lunch_category, di.dinner_category, d.drink_vat " +
                           "FROM((ITEM as i left JOIN drink as d on i.item_id = d.drink_id) left join LUNCH as l on i.item_id = l.lunch_id) left join dinner as di on i.item_id = di.dinner_id";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadMenu(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Item> ReadMenu(DataTable dataTable)
        {
            List<Item> menu = new List<Item>();
            foreach (DataRow dr in dataTable.Rows)
            {
                Item item = new Item()
                {
                    Item_id = (int)dr["item_id"],
                    Name = dr["item_name"].ToString(),
                    Cost = (float)(double)dr["item_cost"],
                    Stock = (int)dr["item_stock"],
                };
                if (!dr.IsNull("drink_category"))
                {
                    item.Category = MenuCategory.Drink;
                    item.DrinkSubCategory = (Drink)Int16.Parse(dr["drink_category"].ToString());
                }
                if (!dr.IsNull("lunch_category"))
                {
                    item.Category = MenuCategory.Lunch;
                    item.LunchSubCategory = (Lunch)Int16.Parse(dr["lunch_category"].ToString());
                }
                if (!dr.IsNull("dinner_category"))
                {
                    item.Category = MenuCategory.Drink;
                    item.DinnerSubCategory = (Dinner)Int16.Parse(dr["dinner_category"].ToString());
                }
                menu.Add(item);
            }
            return menu;
        }
    }
}

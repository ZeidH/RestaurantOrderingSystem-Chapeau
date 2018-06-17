using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using ChapeauModel;

namespace ChapeauDAL
{
    //Also inherit Customer and Item
    public class Payment_DAO : Base
    {
        public void Db_set_payment(Payment payment)
        {
            string query = string.Format("INSERT INTO PAYMENT(order_id, order_tip, order_price, order_pay_method) VALUES(@orderid, @tip, @price, @method)");
            SqlParameter[] sqlParameters = new SqlParameter[4];
            sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
            {
                Value = payment.Order_id
            };
            sqlParameters[1] = new SqlParameter("@tip", SqlDbType.Float)
            {
                Value = payment.Tip
            };
            sqlParameters[2] = new SqlParameter("@price", SqlDbType.Decimal)
            {
                Value = payment.Price
            };
            sqlParameters[3] = new SqlParameter("@method", SqlDbType.SmallInt)
            {
                Value = payment.Method
            };

            ExecuteEditQuery(query, sqlParameters);
        }
        public void Db_set_order_comment(Payment payment)
        {
            string query = string.Format("UPDATE [ORDER] SET order_comment = @comment WHERE order_id = @orderid");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
            {
                Value = payment.Order_id
            };
            sqlParameters[1] = new SqlParameter("@comment", SqlDbType.NVarChar)
            {
                Value = payment.Comment
            };
            ExecuteEditQuery(query, sqlParameters);
        }

        //Where to place?
        public List<OrderItem> Db_select_order_items(int order_id)
        {
            string query = string.Format("SELECT i.item_id, i.item_name, i.item_cost, o.item_amount, o.order_time, d.drink_category, l.lunch_category, di.dinner_category, o.item_comment, d.drink_vat " +
                                     "FROM((ITEM as i left JOIN drink as d on i.item_id = d.drink_id) " +
                                     "left join LUNCH as l on i.item_id = l.lunch_id) " +
                                     "left join dinner as di on i.item_id = di.dinner_id " +
                                     "JOIN ORDER_LIST AS o on o.item_id = i.item_id " +
                                     "WHERE order_id = @orderid ORDER BY item_id");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
            {
                Value = order_id
            };
            return ReadOrder(ExecuteSelectQuery(query, sqlParameters), order_id);
        }

        private List<OrderItem> ReadOrder(DataTable dataTable, int order_id)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (DataRow dr in dataTable.Rows)
            {
                Item item = new Item
                {
                    Item_id = (int)dr["item_id"],
                    Name = dr["item_name"].ToString(),
                    Cost = (float)(double)dr["item_cost"],
                    Order_id = order_id
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
                    item.Category = MenuCategory.Dinner;
                    item.DinnerSubCategory = (Dinner)Int16.Parse(dr["dinner_category"].ToString());
                }
                if (!dr.IsNull("drink_vat"))
                {
                    item.Vat = (Vat)Int16.Parse(dr["drink_vat"].ToString());
                }

                OrderItem orderItem = new OrderItem
                {
                    Item = item,
                    Amount = (int)dr["item_amount"],
                    Time = (DateTime)dr["order_time"],
                    Comment = dr["item_comment"].ToString()
                };

                orderItems.Add(orderItem);

            }
            return orderItems;
        }

        #region Commented -> Get Drink Vat
        //public Vat Db_get_drink_vat(int item_id)
        //{
        //    string query = string.Format("SELECT drink_vat FROM DRINK WHERE drink_id = @item_id");

        //    SqlParameter[] sqlParameters = new SqlParameter[1];
        //    sqlParameters[0] = new SqlParameter("@item_id", SqlDbType.Int)
        //    {
        //        Value = item_id
        //    };

        //    return ReadVat(ExecuteSelectQuery(query, sqlParameters));
        //}

        //private Vat ReadVat(DataTable table)
        //{
        //    Vat vat = Vat.High;
        //    foreach (DataRow dr in table.Rows)
        //    {
        //        vat = (Vat)Int16.Parse(dr["drink_vat"].ToString());
        //    }
        //    return vat;
        //}
        ////change query 
        #endregion

    }
}

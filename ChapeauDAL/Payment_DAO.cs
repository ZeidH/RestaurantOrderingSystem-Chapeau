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
    public class Payment_DAO : Item_DAO
    {
        public void Db_set_payment(Payment payment)
        {
            string query = string.Format("INSERT INTO PAYMENT(order_id, order_tip, order_price, order_comment,  order_pay_method) VALUES(@orderid, @tip, @price, @comment, @method)");
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

            //ExecuteEditQuery(query, sqlParameters);
        }
        public Vat Db_get_drink_vat(int item_id)
        {
            string query = string.Format("SELECT drink_vat FROM DRINK WHERE drink_id = @item_id");

            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@item_id", SqlDbType.Int)
            {
                Value = item_id
            };

            return ReadVat(ExecuteSelectQuery(query, sqlParameters));
        }

        private Vat ReadVat(DataTable table)
        {
            Vat vat = Vat.High;
            foreach (DataRow dr in table.Rows)
            {
                vat = (Vat)dr["drink_vat"];
            }
            return vat;
        }

        //Where to place?
        public List<OrderItem> Db_select_order_items(int order_id)
        {
            //change query
            string query = string.Format("SELECT item_id, item_amount, item_comment FROM ORDER_LIST WHERE order_id = @orderid");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@orderid", SqlDbType.Int)
            {
                Value = order_id
            };
            return ReadOrder(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<OrderItem> ReadOrder(DataTable dataTable)
        { 
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (DataRow dr in dataTable.Rows)
            {
                Item item = new Item();
                item.Item_id = (int)dr["item_id"];

                OrderItem orderItem = new OrderItem
                {
                    Item = item,
                    Amount = (int)dr["item_amount"],
                    Comment = dr["item_comment"].ToString()
                };

                orderItems.Add(orderItem);
               
            }
            return orderItems;
        }

        ////change query
        //string query = string.Format("SELECT i.item_id, i.item_name, i.item_cost, o.item_amount, d.drink_category, l.lunch_category, di.dinner_category " +
        //                             "FROM((ITEM as i left JOIN drink as d on i.item_id = d.drink_id) " +
        //                             "left join LUNCH as l on i.item_id = l.lunch_id) " +
        //                             "left join dinner as di on i.item_id = di.dinner_id " +
        //                             "JOIN ORDER_LIST AS o on o.item_id = i.item_id " +
        //                             "WHERE order_id = @orderid");
    }
}

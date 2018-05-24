using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ChapeauModel;
using ChapeauDAL;

namespace ChapeauLogic
{
    public class Payment_Service
    {
        const double VAT21 = 1.21;
        const double VAT6 = 1.06;
        private Payment_DAO payment_DAO = new Payment_DAO();
        //Split method
        public float SplitPrice(float price, int customers)
        {
            float splittedPrice = price / customers;

            return splittedPrice;
        }

        public void SetPayment(Payment payment, int order_id, float tip, PayMethod method, string comment)
        {
            payment.Order_id = order_id;
            payment.Tip = tip;
            payment.Method = method;
            payment.Comment = comment;
        }

        public List<OrderItem> GetOrderItem(int order_id)
        {
            List<OrderItem> order_itemId = payment_DAO.Db_select_order_items(order_id);
            return order_itemId;

        }
        public void GetReceipt(List<Item> menu, List<OrderItem> orderItem)
        {
            //Get the items that apply to the customers orders
            for (int i = 0; i < orderItem.Count; i++)
            {
                if (orderItem[i].Item.Item_id == menu[i].Item_id)
                {
                    orderItem[i].Item = menu[i];
                }

            }
        }

        public void InsertPayment(Payment payment)
        {
            payment_DAO.Db_set_payment(payment);
        }

        public void GetTotalPrice(List<OrderItem> order)
        {
            Payment payment = new Payment();
            foreach (OrderItem orderItem in order)
            {
                if (orderItem.Item.Category == MenuCategory.Drink)
                {
                    GetVatPrice(orderItem, payment);
                }
                else
                {
                    payment.Price += orderItem.Item.Cost * orderItem.Amount;
                }
            }
        }

        private void GetVatPrice(OrderItem orderItem, Payment payment)
        {
            Vat vat = payment_DAO.Db_get_drink_vat(orderItem.Item.Item_id);

            if (vat == Vat.High)
            {
                payment.Vat += (orderItem.Item.Cost * (float)VAT21) - (orderItem.Item.Cost * orderItem.Amount);
            }
            else
            {
                payment.Vat += (orderItem.Item.Cost * (float)VAT6) - (orderItem.Item.Cost * orderItem.Amount);
            }
            payment.Price += (payment.Vat * orderItem.Amount) + orderItem.Item.Cost;
        }
    }

    //public List<Item> ReadTable(DataTable table)
    //{
    //    List<Item> order = new List<Item>();
    //    foreach (DataRow dr in table.Rows)
    //    {
    //        Item item = new Item
    //        {
    //            Item_id = int.Parse(dr["item_id"].ToString()),
    //            Cost = float.Parse(dr["item_cost"].ToString()),
    //            Amount = int.Parse(dr["item_amount"].ToString())
    //        };

    //        if (!dr.IsNull("drink_category"))
    //        {
    //            item.Category = MenuCategory.Drink;
    //        }
    //        else if (!dr.IsNull("lunch_category"))
    //        {
    //            item.Category = MenuCategory.Lunch;
    //        }
    //        else if(!dr.IsNull("dinner_category"))
    //        {
    //            item.Category = MenuCategory.Dinner;
    //        }
    //        order.Add(item);
    //    }
    //    return order;
    //}
}


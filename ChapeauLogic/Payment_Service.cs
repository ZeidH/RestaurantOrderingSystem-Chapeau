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

        public List<int> GetOrderItemID(int order_id, List<Item> menu)
        {
            List<int> order_itemId = payment_DAO.Db_select_order_items(order_id);
            return order_itemId;

        }
        public List<Item> GetReceipt(List<Item> menu, List<int> order_itemId)
        {
            List<Item> receipt = new List<Item>();

            //Get the items that apply to the customers orders
            for (int i = 0; i < order_itemId.Count; i++)
            {
                receipt.Add(menu[order_itemId[i]]);
            }

            return receipt;
        }
        public void InsertPayment(Payment payment)
        {
            payment_DAO.Db_set_payment(payment);
        }

        public void GetTotalPrice(List<Item> order, Payment payment)
        {
            foreach (Item item in order)
            {
                if (item.Category == MenuCategory.Drink)
                {
                    GetVatPrice(item, payment);
                }
                else
                {
                    payment.Price += item.Cost * item.Amount;
                }
            }
        }

        private void GetVatPrice(Item item, Payment payment)
        {
            Vat vat = payment_DAO.Db_get_drink_vat(item.Item_id, item);
            
            if (vat == Vat.High)
            {
                payment.Vat += (item.Cost * (float)VAT21) - (item.Cost * item.Amount);
            }
            else
            {
                payment.Vat += (item.Cost * (float)VAT6) - (item.Cost * item.Amount);
            }
            payment.Price += (payment.Vat * item.Amount) + item.Cost;
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


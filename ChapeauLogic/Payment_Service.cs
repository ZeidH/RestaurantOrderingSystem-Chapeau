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
    class Payment_Service : Item_Service
    {
        //Split method
        public float SplitPrice(float price, int customers)
        {
            float splittedPrice = price / customers;

            return splittedPrice;
        }
        public void InsertPayment(int order_id, float order_price, float order_tip, PayMethod method, string comment)
        {
            //Insert data into Payment Model 
            Payment payment = new Payment
            {
                Order_id = order_id,
                Price = order_price,
                Tip = order_tip,
                Method = method,
                Comment = comment
            };

            Payment_DAO db_Payment = new Payment_DAO();
            db_Payment.SetPayment(payment);
        }


        //idk
        public List<Item> GetVatPrice(List<Item> order)
        {
            foreach (Item item in order)
            {
                if (item.Category == MenuCategory.Drinks)
                {
                    Payment_DAO payment = new Payment_DAO();

                    DataTable table = payment.Db_Get_Drink_Details(item.Item_id, item);
                    ReadTable(table, item);
                }
            }
            return order;

        }
        private void ReadTable(DataTable table, Item item)
        {
            foreach (DataRow dr in table.Rows)
            {
                item.DrinkVat = (Vat)dr["drink_vat"];
            }
        }
    }
}

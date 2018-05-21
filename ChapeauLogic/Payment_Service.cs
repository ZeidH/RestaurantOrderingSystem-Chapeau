﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ChapeauModel;
using ChapeauDAL;

namespace ChapeauLogic
{
    public class Payment_Service : Item_Service
    {
        const double VAT21 = 1.21;
        const double VAT6 = 1.06;
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
        public List<Item> ReadTable(DataTable table)
        {
            List<Item> order = new List<Item>();
            foreach (DataRow dr in table.Rows)
            {
                Item item = new Item
                {
                    Item_id = int.Parse(dr["item_id"].ToString()),
                    Cost = float.Parse(dr["item_cost"].ToString()),
                    Amount = int.Parse(dr["item_amount"].ToString())
                };

                if (!dr.IsNull("drink_category"))
                {
                    item.Category = MenuCategory.Drinks;
                }
                else if (!dr.IsNull("lunch_category"))
                {
                    item.Category = MenuCategory.Lunch;
                }
                else if(!dr.IsNull("dinner_category"))
                {
                    item.Category = MenuCategory.Dinner;
                }
                order.Add(item);
            }
            return order;
        }
        private Payment_DAO payment_DAO = new Payment_DAO();

        public DataTable GetReceipt(int order_id)
        {
            DataTable table = payment_DAO.Db_select_item_receipt(order_id);

            return table;
        }
        public void InsertPayment(Payment payment)
        {
            payment_DAO.Db_set_payment(payment);
        }

        public void GetTotalPrice(List<Item> order, Payment payment)
        {
            foreach (Item item in order)
            {
                if (item.Category == MenuCategory.Drinks)
                {
                    GetVatPrice(item, payment);
                }
                else
                {
                    payment.Price += item.Cost;
                }
            }
        }

        private void GetVatPrice(Item item, Payment payment)
        {
            DataTable table = payment_DAO.Db_get_drink_vat(item.Item_id, item);
            foreach (DataRow dr in table.Rows)
            {
                Vat DrinkVat = (Vat)dr["drink_vat"];
                if (DrinkVat == Vat.High)
                {
                    payment.Vat += item.Cost * (float)VAT21;
                    payment.Price += payment.Vat;
                }
                else
                {
                    payment.Vat += item.Cost * (float)VAT6;
                    payment.Price += payment.Vat;
                }
            }
        }
    }
}

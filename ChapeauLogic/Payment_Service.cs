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
        const double VAT21 = 1.21;
        const double VAT6 = 1.06;
        //Split method
        public float SplitPrice(float price, int customers)
        {
            float splittedPrice = price / customers;

            return splittedPrice;
        }
        public Payment SetPayment(int order_id, List<Item> order, float tip, PayMethod method, string comment)
        {
            Payment payment = new Payment()
            {
                Order_id = order_id,
                Tip = tip,
                Method = method,
                Comment = comment
            };
            GetTotalPrice(order, payment);

            return payment;

        }
        private Payment_DAO payment_DAO = new Payment_DAO();
        public void InsertPayment(Payment payment)
        {
            payment_DAO.Db_set_payment(payment);
        }

        private Payment GetTotalPrice(List<Item> order, Payment payment)
        {
            foreach (Item item in order)
            {
                if (item.Category == MenuCategory.Drink)
                {
                    GetVatPrice(item, payment);
                }
                else
                {
                    payment.Price += item.Cost;
                }
            }
            return payment;
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

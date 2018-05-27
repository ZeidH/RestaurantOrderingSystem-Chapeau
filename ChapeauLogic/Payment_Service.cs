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
        private Payment_DAO payment_DAO = new Payment_DAO();

        public float SplitPrice(float price, int customers)
        {
            return price / customers;
        }

        public void SetPayment(Payment payment, int order_id, float tip, PayMethod method, string comment)
        {
            payment.Order_id = order_id;
            payment.Tip = tip;
            payment.Method = method;
            payment.Comment = comment;
        }

        public List<OrderItem> GetReceipt(int order_id)
        {
            try
            {
                return payment_DAO.Db_select_order_items(order_id);
            }
            catch (Exception)
            {
                throw;
            }


        }
        public void InsertPayment(Payment payment)
        {
            payment_DAO.Db_set_payment(payment);
        }

        public PayMethod GetPayMethod(string content)
        {
            string[] split = content.Split(new Char[] { ' ', ':' });
            return (PayMethod)Enum.Parse(typeof(PayMethod), split[2], true);
        }

        public Payment GetTotalPrice(List<OrderItem> order)
        {
            Payment payment = new Payment();
            foreach (OrderItem orderItem in order)
            {
                payment.Price += orderItem.TotalPrice;
                payment.Vat += orderItem.VatPrice;
            }
            return payment;
        }
    }
}


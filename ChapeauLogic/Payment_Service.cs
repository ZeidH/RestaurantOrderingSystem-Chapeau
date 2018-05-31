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

        public void SetPayment(Payment payment, float tip, string comment)
        {
            payment.Tip = tip;
            payment.Comment = comment;
        }

        public List<OrderItem> GetReceipt(int order_id)
        {
            List<OrderItem> order = payment_DAO.Db_select_order_items(order_id);
            for (int i = 0; i < order.Count - 1; i++)
            {
                while (order[i].Item.Item_id == order[i + 1].Item.Item_id)
                {

                    order[i].Amount += order[i+1].Amount;
                    order.RemoveAt(i + 1);
                }
            }
            return order;
        }
        public void InsertPayment(Payment payment)
        {
            payment_DAO.Db_set_payment(payment);
        }

        public PayMethod GetPayMethod(string content)
        {
            return (PayMethod)Enum.Parse(typeof(PayMethod), content, true);
        }

        public Payment GetTotalPrice(List<OrderItem> order, Payment payment)
        {
           // Payment payment = new Payment();
            foreach (OrderItem orderItem in order)
            {
                payment.Price += orderItem.TotalPrice;
                payment.Vat += orderItem.VatPrice;
            }
            return payment;
        }
    }
}


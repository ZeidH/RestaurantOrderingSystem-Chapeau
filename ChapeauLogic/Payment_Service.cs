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

        // Main Payment View

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

                    order[i].Amount += order[i + 1].Amount;
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

        // UserControl Payment_Split

        public void CalculateGuestPrice(int i, List<float> guestPrice, float change, int id)
        {
            while (i < guestPrice.Count)
            {
                float collected = DeletePrice(guestPrice, i, id, change);
                AddPrice(guestPrice, id, collected);
                i++;
            }

        }
        private void AddPrice(List<float> guestPrice, int id, float change)
        {
            guestPrice[id] += change;
        }
        private float DeletePrice(List<float> guestPrice, int i, int id, float change)
        {
            float calcCheck = guestPrice[i];
            calcCheck -= (change / (guestPrice.Count - (id + 1)));

            int alive = 0;
            for (int y = 0; y < guestPrice.Count; y++)
            {
                if (guestPrice[y] > 0)
                {
                    alive++;
                }
            }
            if (calcCheck > 0)
            {
                guestPrice[i] -= (change / (alive - (id + 1)));
                return (change / (alive - (id + 1)));
            }
            else if (guestPrice[i] != 0)
            {
                float collected = guestPrice[i];
                guestPrice[i] -= guestPrice[i];
                return collected;
            }
            return 0;
        }
        public void CalculateGuestPriceDelete(List<float> guestPrice, int id, int i)
        {
            for (int x = 0; x < i; x++)
            {
                guestPrice[x] += (guestPrice[id] / i);
            }
            guestPrice[id] -= guestPrice[id];
        }
    }
}


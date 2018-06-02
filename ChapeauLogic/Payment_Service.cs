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
        public bool InsertPayment(Payment payment)
        {
            //Insert payment
            payment_DAO.Db_set_payment(payment);
            if (payment.NextCustomer == 0)
                SetPayingCustomerCount(payment);

            if ((payment.SplitPayment) && ((payment.NextCustomer + 1) != payment.CustomerCount))
            {
                payment.NextCustomer++;
                return false;
            }
            else
            {
                payment_DAO.Db_set_order_comment(payment);
                return true;
            }      
        }

        private void SetPayingCustomerCount(Payment payment)
        {
            foreach (float guest in payment.GuestPrice)
            {
                if (guest == 0)
                {
                    payment.CustomerCount--;
                }
            }
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
        public void CalculateGuestPriceAdd(int i, Payment payment, float change, int id)
        {
            while (i < payment.GuestPrice.Count)
            {
                float collected = DeletePrice(payment, i, id, change);
                AddPrice(payment, id, collected);
                i++;
            }

        }
        private void AddPrice(Payment payment, int id, float change)
        {
            payment.GuestPrice[id] += change;
        }
        public void CalculateGuestPriceDelete(Payment payment, int id, int i)
        {
            for (int x = 0; x < i; x++)
            {
                payment.GuestPrice[x] += (payment.GuestPrice[id] / i);
            }
            payment.GuestPrice[id] -= payment.GuestPrice[id];
        }
        private float DeletePrice(Payment payment, int i, int id, float change)
        {
            float calcCheck = payment.GuestPrice[i];
            calcCheck -= (change / (payment.GuestPrice.Count - (id + 1)));

            int alive = 0;
            for (int y = 0; y < payment.GuestPrice.Count; y++)
            {
                if (payment.GuestPrice[y] > 0)
                {
                    alive++;
                }
            }
            if (calcCheck > 0)
            {
                payment.GuestPrice[i] -= (change / (alive - (id + 1)));
                return (change / (alive - (id + 1)));
            }
            else if (payment.GuestPrice[i] != 0)
            {
                float collected = payment.GuestPrice[i];
                payment.GuestPrice[i] -= payment.GuestPrice[i];
                return collected;
            }
            return 0;
        }

        public void ResetSplit(Payment payment)
        {
            for (int i = 0; i < payment.GuestPrice.Count; i++)
            {
                payment.GuestPrice[i] = payment.SplittedPrice;
            }
        }
    }
}


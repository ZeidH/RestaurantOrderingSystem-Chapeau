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
            // Insert payment
            payment_DAO.Db_set_payment(payment);

            // On first paying customer
            if (payment.NextCustomer == 0)
                SetPayingCustomerCount(payment);

            payment.Tip = 0;
            if ((payment.SplitPayment) && ((payment.NextCustomer + 1) != payment.CustomerCount))
            {
                payment.NextCustomer++;
                return false;
            }
            else
            {
                // If there are no more customers, set the order and finish payment by returning true
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

        public void GetTotalPrice(List<OrderItem> order, Payment payment)
        {
            foreach (OrderItem orderItem in order)
            {
                payment.Price += orderItem.TotalPrice;
                payment.Vat += orderItem.VatPrice;
            }
        }

        // UserControl Payment_Split
        public void CalculateGuestPriceAdd(Payment payment, float change, int guest)
        {
            for (int i = (guest + 1); i < payment.GuestPrice.Count; i++)
            {
                AddPrice(payment, guest, (DeletePrice(payment, i, guest, change)));
            }
        }
        private void AddPrice(Payment payment, int guest, float change)
        {
            // Add the collected money to the user
            payment.GuestPrice[guest] += change;
        }
        private float DeletePrice(Payment payment, int i, int guest, float change)
        {
            // Check how many of the guests that still have more than 0 money
            int alive = GuestsOverZero(payment);

            // Check if calculation ends up under minus
            if (CalculationCheck(payment, guest, i, change))
            {
                // Divide by the amount of living guests and return the amount that was deducted
                payment.GuestPrice[i] -= (change / (alive - guest));
                return (change / (alive - guest));
            }
            else
            {
                // Collect whats left and return that amount
                float collected = payment.GuestPrice[i];
                payment.GuestPrice[i] -= payment.GuestPrice[i];
                return collected;
            }
        }
        public void CalculateGuestPriceDelete(Payment payment, int guest, int i)
        {
            for (int x = 0; x < i; x++)
            {
                payment.GuestPrice[x] += (payment.GuestPrice[guest] / i);
            }
            payment.GuestPrice[guest] -= payment.GuestPrice[guest];
        }
        private int GuestsOverZero(Payment payment)
        {
            int alive = 0;
            for (int y = 0; y < payment.GuestPrice.Count; y++)
            {
                if (payment.GuestPrice[y] > 0)
                {
                    alive++;
                }
            }
            return alive;
        }
        private bool CalculationCheck(Payment payment, int guest, int i, float change)
        {
            float calcCheck = payment.GuestPrice[i];
            calcCheck -= (change / (payment.GuestPrice.Count - guest));
            return calcCheck > 0;
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


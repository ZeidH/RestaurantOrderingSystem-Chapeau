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
    public class Payment_Service : Table_Service
    {
        private Payment_DAO payment_DAO = new Payment_DAO();

        public List<OrderItem> GetReceipt(int order_id)
        {
            List<OrderItem> order;
            try
            {
                order = payment_DAO.Db_select_order_items(order_id);
            }
            catch (Exception)
            {
                throw new Exception("Cannot connect to server \nAn error log has been saved in the program folder \n Press 'OK' to retry");
            }

            //Add amount of the same items from DIFFERENT orders
            return AddDuplicates(order);
        }

        private List<OrderItem> AddDuplicates(List<OrderItem> order)
        {
            List<OrderItem> distinctOrder = order;
            for (int i = 0; i < order.Count; i++)
            {
                for (int k = 0; k < order.Count; k++)
                {
                    if ((order[i].Item.Item_id == order[k].Item.Item_id) && order[i].Time != order[k].Time)
                    {
                        order[i].Amount += order[k].Amount;
                        order.Remove(order[k]);
                    }
                }
            }
            return order;
        }
        public bool InsertPayment(Payment payment)
        {
            try
            {
                payment_DAO.Db_set_payment(payment);
            }
            catch (Exception)
            {
                throw new Exception("Chapeau can't connect to the internet!");
            }

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
                // If there are no more paying customers left, set the order and finish payment by returning true
                try
                {
                    payment_DAO.Db_set_order_comment(payment);
                    payment_DAO.Db_set_item_status(payment.Order_id, OrderStatus.Served)
                }
                catch (Exception)
                {
                    throw new Exception("Chapeau can't connect to the internet!");
                }
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
            foreach (OrderItem orderItem in order)
            {
                payment.SetPrice += orderItem.TotalPrice;
                payment.Vat += orderItem.VatPrice;
            }
            return payment;
        }

        #region UserControl Payment_Split Logic
        // If add Buttons are pressed
        public void CalculateGuestPriceAdd(Payment payment, int change, int guest)
        {
            // Check how many of the guests that still have more than 0 money
            int alive = GuestsOverZero(payment);
            for (int i = (guest + 1); i < alive; i++)
            {
                AddPrice(payment, guest, (DeletePrice(payment, i, guest, change, alive)));
            }
        }
        private void AddPrice(Payment payment, int guest, int change)
        {
            // Add the collected money to the user
            payment.GuestPrice[guest] += change;
        }
        private int DeletePrice(Payment payment, int i, int guest, int change, int alive)
        {
            guest++;
            // Check if calculation ends up under minus
            if (CalculationCheck(payment, guest, i, change, alive))
            {
                // Divide by the amount of living guests and return the amount that was deducted
                payment.GuestPrice[i] -= (change / (alive - guest));
                return (change / (alive - guest));
            }
            else
            {
                // Collect whats left and return that amount
                int collected = payment.GuestPrice[i];
                payment.GuestPrice[i] -= payment.GuestPrice[i];
                return collected;
            }
        }
        private bool CalculationCheck(Payment payment, int guest, int i, int change, int alive)
        {
            float calcCheck = payment.GuestPrice[i];
            if (guest == 1 && calcCheck - change < 0)
                return false;

            calcCheck -= (change / (alive - guest));
            return calcCheck > 0;
        }

        public int GuestsOverZero(Payment payment)
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

        // If Zero button is pressed
        public void CalculateGuestPriceDelete(Payment payment, int guest)
        {
            for (int x = 0; x < (guest + 1); x++)
            {
                payment.GuestPrice[x] += (payment.GuestPrice[guest] / guest);
            }
            payment.GuestPrice[guest] -= payment.GuestPrice[guest];
        }

        public void ResetSplit(Payment payment)
        {
            for (int i = 0; i < payment.GuestPrice.Count; i++)
            {
                payment.GuestPrice[i] = payment.SplittedPrice;
            }
        }
        #endregion
    }
}


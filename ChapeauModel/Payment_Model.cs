using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapeauModel
{
    public class Payment
    {
        //int order_id, float order_price, float order_tip, PayMethod method

        public int Order_id { get; set; }
        public float Price
        {
            get
            {
                if (SplitPayment == true)
                    return GuestPrice[0];
                else
                    return Price;
            }
            set
            {
                Price += value;
            }
        }
        public float Vat { get; set; }
        public float Tip { get; set; }
        public PayMethod? Method { get; set; }
        public string Comment { get; set; }
        public int CustomerCount { get; set; }
        public List<float> GuestPrice { get; set; }
        public bool SplitPayment { get; set; }
        public float TotalPrice
        {
            get
            {
                return Price + Tip + Vat;
            }
        }
        public float SplittedPrice
        {
            get
            {
                return Price / CustomerCount;
            }
        }

        public Payment(int NrOfCustomers, int Order_id)
        {
            this.Order_id = Order_id;
            this.CustomerCount = NrOfCustomers;
            Price = 0;
            SplitPayment = false;
        }
    }
}

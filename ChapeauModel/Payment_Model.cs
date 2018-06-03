using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapeauModel
{
    public class Payment
    {
        public int Order_id { get; set; }
    
        private float price;
        public float Price
        {
            get
            {
                if (SplitPayment == true)
                    return GuestPrice[NextCustomer] + Vat;
                else
                    return price;
            }
            set
            {
                price = value;
            }
        }
        public float Vat { get; set; }
        public float Tip { get; set; }
        public PayMethod? Method { get; set; }
        public string Comment { get; set; }
        public int CustomerCount { get; set; }
        public List<float> GuestPrice { get; set; }
        public int NextCustomer { get; set; }
        public bool SplitPayment { get; set; }

        //Computational Properties
        public float TotalPrice
        {
            get
            {
                return price + Vat + Tip;
            }
        }
        public float SplittedPrice
        {
            get
            {
                return price / CustomerCount;
            }
        }

        //Constructor
        public Payment()
        {
            //Empty constructor
        }
        public Payment(int nrOfCustomers, int order_id)
        {
            this.Order_id = order_id;
            this.CustomerCount = nrOfCustomers;
            Price = 0;
            NextCustomer = 0;
            GuestPrice = new List<float>();
            SplitPayment = false;
        }
    }
}

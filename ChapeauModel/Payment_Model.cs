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
        public int SetPrice { get; set; }
        public int Vat { get; set; }
        public int Tip { get; set; }
        public PayMethod? Method { get; set; }
        public string Comment { get; set; }
        public int CustomerCount { get; set; }
        public List<int> GuestPrice { get; set; }
        public int NextCustomer { get; set; }
        public bool SplitPayment { get; set; }

        //Computational Properties
        public float Price
        {
            get
            {
                if (SplitPayment == true)
                    return (float)GuestPrice[NextCustomer] / 10000;
                else
                    return ((float)SetPrice / 10000);
            }
            set
            {
                SetPrice = (int)value;
            }
        }
        public float ReadVat
        {
            get
            {
                return (float)Vat / 10000;
            }
        }
        public float TotalPrice
        {
            get
            {
                return (float)(SetPrice + Vat) / 10000;
            }
        }
        public int SplittedPrice
        {
            get
            {
                return (SetPrice + Vat) / CustomerCount;
            }
        }

        // Constructors
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
            GuestPrice = new List<int>();
            SplitPayment = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapeauModel;
namespace ChapeauModel
{
    public struct Order
    {
        public float price { get; set; }
        public string item { get; set; }
        public Lunch lunchCategory { get; set; }
        public Dinner dinnerCategory { get; set; }
        public Drink drinkCategory { get; set; }

    }
    //public float Tip { get; set; }
    //public PayMethod PaymentMethod { get; set; }
}

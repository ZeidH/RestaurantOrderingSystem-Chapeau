using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapeauModel;
namespace ChapeauModel
{
    public class Order
    {
        public float Price { get; set; }
        public string Item { get; set; }
        public Lunch LunchCategory { get; set; }
        public Dinner DinnerCategory { get; set; }
        public Drink DrinkCategory { get; set; }

    }


    //public float Tip { get; set; }
    //public PayMethod PaymentMethod { get; set; }
}

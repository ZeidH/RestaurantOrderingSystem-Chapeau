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

    public class Item
    {
        public int Order_id { get; set; }
        public string Item_comment { get; set; }
        public DateTime Order_time { get; set; }
        public OrderStatus Order_status { get; set; }
        public int Item_amount { get; set; }
        public int Item_id { get; set; }
    }
    //public float Tip { get; set; }
    //public PayMethod PaymentMethod { get; set; }
}

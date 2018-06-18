using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChapeauModel
{
    public class Item
    {
        public int Order_id { get; set; }
        public string Name { get; set; }

        public float Cost
        {
            get
            {
                return (float)ReadCost / Payment.CONVERTION;
            }
            set
            {
                value = value * Payment.CONVERTION;
                ReadCost = (int)value;
            }
        }
        public int ReadCost { get; private set; }

        public int Stock { get; set; }
        public MenuCategory Category { get; set; }
        public int Item_id { get; set; }
        public Drink? DrinkSubCategory { get; set; }
        public Lunch? LunchSubCategory { get; set; }
        public Dinner? DinnerSubCategory { get; set; }
        public Vat? Vat { get; set; }
    }
}




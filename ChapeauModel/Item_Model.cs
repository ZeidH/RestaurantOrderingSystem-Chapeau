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
        public string Comment { get; set; }
        public string Order_time { get; set; }
        public OrderStatus Order_status { get; set; }
        public int Amount { get; set; } 
        public float Cost { get; set; } 
        public int Stock { get; set; }
        public MenuCategory Category { get; set; } 
        public int Item_id { get; set; } 



        //public Drink DrinkType { get; set; }
        //public Lunch LunchType { get; set; }
        //public Dinner DinnerType { get; set; }

    }
}
        
    


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
        public string Item_comment { get; set; }
        public DateTime Order_time { get; set; }
        public OrderStatus Order_status { get; set; }
        public int Item_amount { get; set; }
        public float Item_cost { get; set; }
        public int Item_stock { get; set; }
        public MenuCategory Item_category { get; set; }
        public int Item_id { get; set; }
        public Vat DrinkVat { get; set; }



        //public Drink DrinkType { get; set; }
        //public Lunch LunchType { get; set; }
        //public Dinner DinnerType { get; set; }

    }
}
        
    


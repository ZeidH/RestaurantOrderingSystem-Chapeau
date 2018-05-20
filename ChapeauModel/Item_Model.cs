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
        public int Item_id { get; set; }

    }
}

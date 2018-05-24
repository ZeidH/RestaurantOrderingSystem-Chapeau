using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapeauModel
{
    public class OrderItem
    {
        public Item Item { get; set; }
        public string Comment { get; set; }
        public DateTime Time { get; set; }
        public OrderStatus Status { get; set; }
        public int Amount { get; set; }
    }
}

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
        public float TotalPrice { get { return (this.Item.Cost * this.Amount); } }
        public string NameAndComment
        {
            get
            {
                if (Comment != "")
                { return string.Format("{0}\n Comment: {1}", Item.Name, Comment); }
                else
                { return string.Format("{0}", Item.Name); }
            }
        }
    }
}

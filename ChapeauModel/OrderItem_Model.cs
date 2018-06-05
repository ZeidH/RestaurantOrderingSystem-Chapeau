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
        public int TotalPrice
        {
            get
            {
                if (Item.Vat == Vat.High || Item.Vat == Vat.Low)
                    return (Item.ReadCost * Amount) + VatPrice;
                else
                    return Item.ReadCost * Amount;
            }
        }
        public float ReadTotalPrice
        {
            get
            {
                return (float)TotalPrice / 10000;
            }
        }
        public int VatPrice
        {
            get
            {
                if (Item.Vat == Vat.High)
                    return (int)(Item.ReadCost * 0.21) * Amount;
                else if (Item.Vat == Vat.Low)
                    return (int)(Item.ReadCost * 0.06) * Amount;
                else
                    return 0;
            }
        }
        public string NameAndComment
        {
            get
            {
                if (Comment != " ")
                { return string.Format("{0}\n Comment: {1}", Item.Name, Comment); }
                else
                { return string.Format("{0}", Item.Name); }
            }
        }
    }
}

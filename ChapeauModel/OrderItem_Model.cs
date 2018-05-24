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
        public float TotalPrice
        {
            get
            {
                if (Item.Vat == Vat.High)
                    return (Item.Cost * Amount) + VatPrice;
                
                else if (Item.Vat == Vat.Low)
                    return (Item.Cost * Amount) + VatPrice;
                else
                    return Item.Cost * Amount;               
            }
        }
        public float VatPrice
        {
            get
            {
                if (Item.Vat == Vat.High)
                    return (Item.Cost * (float)0.21) * Amount;
                else if (Item.Vat == Vat.Low)
                    return (Item.Cost * (float)0.06) * Amount;
                else
                    return 0;
            }
        }
    }
}

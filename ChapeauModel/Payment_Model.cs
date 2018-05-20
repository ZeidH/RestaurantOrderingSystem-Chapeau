using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapeauModel
{
    public class Payment
    {
        //int order_id, float order_price, float order_tip, PayMethod method

        public int Order_id { get; set; }
        public float Price{ get; set; }
        public float Tip { get; set; }
        public PayMethod Method { get; set; }
        public string Comment { get; set; }
    }
}

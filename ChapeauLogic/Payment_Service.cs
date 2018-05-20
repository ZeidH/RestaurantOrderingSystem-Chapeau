using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapeauModel;
using ChapeauDAL;

namespace ChapeauLogic
{
    class Payment_Service : Item_Service
    {
        //Split method
        public float SplitPrice(float price, int customers)
        {
            float splittedPrice = price / customers;

            return splittedPrice;
        }
        public void InsertPayment(int order_id, float order_price, float order_tip, PayMethod method, string comment)
        {
            //Insert data into Payment Model 
            Payment payment = new Payment
            {
                Order_id = order_id,
                Price = order_price,
                Tip = order_tip,
                Method = method,
                Comment = comment
            };

            Payment_DAO db_Payment = new Payment_DAO();
            db_Payment.SetPayment(payment);
        }
    }
}

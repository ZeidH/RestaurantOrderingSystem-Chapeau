﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapeauModel;
using ChapeauDAL;

namespace ChapeauLogic
{
    class Payment_Service
    {
        //Split method
        public float SplitPrice(float price, int customers)
        {
            float splittedPrice = price / customers;

            return splittedPrice;
        }
        //Insert payment method
        public void InsertPayment(int order_id, float order_price, float order_tip, PayMethod method)
        {
            //insert values in model

            //call db
            //finish
        }
        //Insert Comment of the order
        public void InsertComment(int order_id, string comment)
        {

        }
    }
}

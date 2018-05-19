using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapeauModel;
using ChapeauDAL;

namespace ChapeauLogic
{
    public class Order_Service
    {
        public void Test()
        {
            int emp_id = 1;
            int table_id = 5;

            Order_DAO order = new Order_DAO();
            order.Db_add_order(table_id, emp_id);

            //Db_add_order(int table_id, int emp_id);
        }



    }
}

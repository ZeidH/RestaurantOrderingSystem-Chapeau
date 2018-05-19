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
        public void InsertOrder(int emp_id, int table_id)
        {
            Order_DAO order = new Order_DAO();
            order.Db_add_order(table_id, emp_id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapeauModel;
using ChapeauDAL;
using System.Data;

namespace ChapeauLogic
{
    public class Order_Service
    {
        Order_DAO order_DAO = new Order_DAO();
        public void InsertOrder(int emp_id, int table_id, uint nrOfGuests)
        {
            order_DAO.Db_add_order(table_id, emp_id, nrOfGuests);
        }

        public void DeleteOrder(int order_id)
        {
            order_DAO.Db_delete_order(order_id);
        }

        public DataTable GetOrder(int order_id)
        {
            DataTable dataTable = order_DAO.Db_select_order(order_id);
            return dataTable;
        }
    }
}

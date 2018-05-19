using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapeauModel;
using ChapeauDAL;
using System.Configuration;
using System.Data;

namespace ChapeauLogic
{
    public class Item_Service
    {
        public DataTable GetItems(int order_id)
        {
            Item_DAO item_DAO = new Item_DAO();
            DataTable dataTable = item_DAO.Db_select_items(order_id);
            return dataTable;

            //order_id, item_comment, order_time, order_status, item_amount, item_id
        }
    }
}

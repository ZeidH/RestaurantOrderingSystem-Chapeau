using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapeauModel;
using ChapeauDAL;
using System.Configuration;
using System.Windows.Forms;
using System.Data;

namespace ChapeauLogic
{
    public class Item_Service
    {
        public ListView ListView(int order_id)
        {
            ChapeauDAL.Item_DAO item_DAO = new Item_DAO();
            DataTable dataTable = new DataTable();
            dataTable = item_DAO.Db_select_items(order_id);
        }
    }
}

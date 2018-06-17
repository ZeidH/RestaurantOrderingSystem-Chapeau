using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ChapeauDAL;
using ChapeauModel;

namespace ChapeauLogic
{
    public class Table_Service : Order_Service
    {
        Table_DAO table_db = new Table_DAO();
        public ObservableCollection<Tafel> FillTables()
        {
            ObservableCollection<Tafel> table = table_db.Db_Get_All_Tables();
            return table_db.Db_Get_Busy_Table_Info(table);
        }
        public void SetTableStatus(TableStatus status, int tableID)
        {
            table_db.Db_Update_Table_Status(status, tableID);
        }

        public int GetTableIDFromOrderID(int order_id)
        {
            return table_db.Db_Get_TableID(order_id);
        }
    }
}

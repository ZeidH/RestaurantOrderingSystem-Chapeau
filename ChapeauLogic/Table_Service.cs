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
    public class Table_Service
    {
        public ObservableCollection<Tafel> FillTables()
        {
            Table_DAO table_db = new Table_DAO();
            ObservableCollection<Tafel> table = table_db.Db_Get_All_Tables();
            return table_db.Db_Get_Busy_Table_Info(table);
        }
    }
}

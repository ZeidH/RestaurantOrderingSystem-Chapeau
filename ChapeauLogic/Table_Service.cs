using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapeauDAL;
using ChapeauModel;

namespace ChapeauLogic
{
    public class Table_Service
    {
        public List<Tafel> FillTables()
        {
            Table_DAO table_db = new Table_DAO();
            return table_db.Db_Get_Busy_Table_Info(table_db.Db_Get_All_Tables());
        }
    }
}

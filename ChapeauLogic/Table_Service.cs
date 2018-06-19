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
        Table_DAO table_db = new Table_DAO();
        public ObservableCollection<Tafel> FillTables()
        {
            try
            {
                ObservableCollection<Tafel> table = table_db.Db_Get_All_Tables();
                return table_db.Db_Get_Busy_Table_Info(table);
            }
            catch (Exception)
            {
                throw new Exception("Chapeau couldn't connect to the internet :c");
            }

        }
        public void SetTableStatus(TableStatus status, int tableID)
        {
            try
            {
                table_db.Db_Update_Table_Status(status, tableID);
            }
            catch (Exception)
            {
                throw new Exception("Chapeau couldn't connect to the internet :c");
            }
        }

        public int GetTableIDFromOrderID(int order_id)
        {
            try
            {
                return table_db.Db_Get_TableID(order_id);
            }
            catch (Exception)
            {
                throw new Exception("Chapeau couldn't connect to the internet :c");
            }
        }

        public void InsertOrder(int emp_id, int table_id, uint nrOfGuests)
        {
            try
            {
                table_db.Db_add_order(table_id, emp_id, nrOfGuests);

            }
            catch (Exception)
            {
                throw new Exception("Chapeau couldn't connect to the internet :c");
            }
        }
    }
}

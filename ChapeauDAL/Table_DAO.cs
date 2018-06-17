using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using ChapeauModel;

namespace ChapeauDAL
{
    public class Table_DAO : Base
    {
        public void Db_Update_Table_Status(TableStatus status, int tableID)
        {
            string query = "UPDATE [TABLE] SET table_status = @status WHERE table_id = @table_id";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@status", SqlDbType.Int)
            {
                Value = status
            };
            sqlParameters[1] = new SqlParameter("@table_id", SqlDbType.Int)
            {
                Value = tableID
            };
            ExecuteEditQuery(query, sqlParameters);
        }
        public ObservableCollection<Tafel> Db_Get_All_Tables()
        {
            string query = "SELECT table_id, table_status FROM [TABLE]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public ObservableCollection<Tafel> Db_Get_Busy_Table_Info(ObservableCollection<Tafel> tables)
        {
            foreach (Tafel table in tables)
            {
                if (table.Status != TableStatus.Free)
                {
                    string query = "SELECT o.order_id, o.nr_of_guests, concat(e.emp_firstName,' ', e.emp_lastName) as fullname FROM [ORDER] AS o JOIN EMPLOYEE AS e ON o.emp_id = e.emp_id WHERE o.table_id = 5  AND o.order_id NOT in (SELECT order_id from PAYMENT) ";
                    SqlParameter[] sqlParameters = new SqlParameter[1];
                    sqlParameters[0] = new SqlParameter("@tableid", SqlDbType.Int)
                    {
                        Value = table.ID
                    };
                    InsertTableInfo(ExecuteSelectQuery(query, sqlParameters), table);
                }
            }
            return tables;
        }

        private void InsertTableInfo(DataTable dataTable, Tafel table)
        {
            foreach (DataRow dr in dataTable.Rows)
            {
                table.OrderID = (int)dr["order_id"];
                //table.Employee.ID = (int)dr["emp_id"];
                table.Employee.Name = dr["fullname"].ToString();
                table.NumberOfGuests = (int)dr["nr_of_guests"];
            }
        }

        private ObservableCollection<Tafel> ReadTables(DataTable dataTable)
        {
            ObservableCollection<Tafel> tables = new ObservableCollection<Tafel>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Tafel table = new Tafel()
                {
                    ID = (int)dr["table_id"],
                    Status = (TableStatus)Int16.Parse(dr["table_status"].ToString())
                };
                tables.Add(table);
            }
            return tables;
        }

        public int Db_Get_TableID(int order_id)
        {
            string query = "SELECT table_id FROM [ORDER] WHERE order_id = @orderID";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@orderID", SqlDbType.Int)
            {
                Value = order_id
            };
            return ReadTableID(ExecuteSelectQuery(query, sqlParameters));
        }
        private int ReadTableID(DataTable dataTable)
        {
            int table_id = 0;
            foreach (DataRow dr in dataTable.Rows)
            {
                table_id = (int)dr["table_id"];
            }
            return table_id;
        }
    }
}

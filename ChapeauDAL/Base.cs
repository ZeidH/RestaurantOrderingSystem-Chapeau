using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Transactions;

namespace ChapeauDAL
{
    public class Base
    {
        private SqlDataAdapter adapter;
        private SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ChapeauDatabase"].ConnectionString);
        private ErrorFilePrint print;
        public Base()
        {
            print = new ErrorFilePrint();
            adapter = new SqlDataAdapter();
        }

        protected SqlConnection OpenConnection()
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
            {
                conn.Open();
            }
            return conn;
        }

        private void CloseConnection()
        {
            conn.Close();
        }

        protected TransactionScope BeginTransaction()
        {
            TransactionScope ts = new TransactionScope();
            return ts;
        }

        protected void EndTransaction(TransactionScope ts)
        {
            ts.Complete();
        }

        //For Insert/Update/Delete Queries
        protected void ExecuteEditQuery(String query, SqlParameter[] sqlParameters, SqlTransaction sqlTransaction)
        {
            SqlCommand command = new SqlCommand(query, conn, sqlTransaction);

            try
            {
                command.Parameters.AddRange(sqlParameters);
                adapter.InsertCommand = command;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                print.ErrorLog(e);
            }
            finally
            {
                CloseConnection();
            }
        }

        /* For Select Queries */
        /// <summary>
        /// This returns a so called "DataTable", All the data retrieved from db will be stored in this table.
        /// You will need to read off the information in the datatable and store it in your model. DataTable does NOT leave DAL layer.
        /// </summary>
        protected DataTable ExecuteSelectQuery(String query, SqlParameter[] sqlParameters)
        {
            SqlCommand command = new SqlCommand();
            DataTable dataTable = new DataTable();
            dataTable = null;
            DataSet dataSet = new DataSet();

            try
            {
                command.Connection = OpenConnection();
                command.CommandText = query;
                command.Parameters.AddRange(sqlParameters);
                command.ExecuteNonQuery();
                adapter.SelectCommand = command;
                adapter.Fill(dataSet);
                dataTable = dataSet.Tables[0];
            }
            catch (SqlException e)
            {
                print.ErrorLog(e);
                return null;
            }
            finally
            {
                CloseConnection();
            }
            return dataTable;
        }
    }
}

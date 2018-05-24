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

        public Base()
        {
            adapter = new SqlDataAdapter();
        }

        private SqlConnection OpenConnection()
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
        protected void ExecuteEditQuery(String query, SqlParameter[] sqlParameters)
        {
            SqlCommand command = new SqlCommand();

            try
            {
                command.Connection = OpenConnection();
                command.CommandText = query;
                command.Parameters.AddRange(sqlParameters);
                adapter.InsertCommand = command;
                command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                ErrorFilePrint print = new ErrorFilePrint();
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
        /// There's no need to put data into a model or anything. Just use the DataTable to fill the ListView using Binding Path in the XAML.
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
                ErrorFilePrint print = new ErrorFilePrint();
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

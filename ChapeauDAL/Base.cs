using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ChapeauDAL
{
    public abstract class Base
    {
        private SqlDataAdapter adapter;
        private SqlConnection conn;
        private ErrorFilePrint print;
        public Base()
        {
            print = new ErrorFilePrint();
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ChapeauDatabase"].ConnectionString);
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

        //For Insert/Update/Delete Queries with transaction
        protected void ExecuteEditTranQuery(String query, SqlParameter[] sqlParameters, SqlTransaction sqlTransaction)
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
                throw;
            }
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
                print.ErrorLog(e);
                throw;
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
		protected DataTable ExecuteSelectQuery(String query, params SqlParameter[] sqlParameters)
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
                throw e;
            }
            finally
            {
                CloseConnection();
            }
            return dataTable;
        }
    }
}

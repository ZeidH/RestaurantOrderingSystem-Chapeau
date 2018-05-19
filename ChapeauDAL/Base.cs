using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ChapeauDAL
{
    public class Base
    {
        //delete pls
        protected StringBuilder sb = new StringBuilder();

        private SqlDataAdapter adapter = new SqlDataAdapter();
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

        //to be deleted
        protected SqlConnection OpenConnectionDB()
        {
            //Connection string with creditentials from App.Config
            string connString = ConfigurationManager
                .ConnectionStrings["ChapeauDatabase"].ConnectionString;
            SqlConnection connection = new SqlConnection(connString);
            //Try to open connection to the database
            try
            {
                connection.Open();
            }
            //If exception> write the details in the ErrorLog and throw e
            catch (Exception e)
            {
                ErrorFilePrint print = new ErrorFilePrint();
                print.ErrorLog(e);

            }
            return connection;
        }
    }
}

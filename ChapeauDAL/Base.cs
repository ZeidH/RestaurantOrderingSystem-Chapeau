using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

namespace ChapeauDAL
{
    class Base
    {
        protected StringBuilder sb = new StringBuilder();

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

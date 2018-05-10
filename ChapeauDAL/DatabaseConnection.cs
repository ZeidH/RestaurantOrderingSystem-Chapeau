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
    class DatabaseConnection
    {
        private SqlConnection openConnectionDB()
        {
            //Connection string with creditentials from App.Config
            string connString = ConfigurationManager
                .ConnectionStrings["ChapeauDatabase"].ConnectionString;

            //Try to open connection to the database
            try
            {
                SqlConnection connection = new SqlConnection(connString);
                connection.Open();
                return connection;
            }
            //If exception> write the details in the ErrorLog and throw e
            catch (Exception e)
            {
                writeErrorLog(e);
                throw e;
            }
        }

        //Create new file or if it exists, write the exception message, stacktrace, date time
        public void writeErrorLog(Exception e)
        {
            string logPath = @"..\ErrorLog.txt";

            if (!File.Exists(logPath))
            {
                File.Create(logPath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(logPath))
            {
                sw.WriteLine("=============Error Logging ===========‿︵‿︵(ಥ﹏ಥ)‿︵‿︵");
                sw.WriteLine("===========Start============= " + DateTime.Now);
                sw.WriteLine("Error Message: " + e.Message);
                sw.WriteLine("Stack Trace: " + e.StackTrace);
                sw.WriteLine("===========End============= ┬──┬ ノ( ゜-゜ノ)");
                sw.WriteLine();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChapeauDAL
{
    class ErrorFilePrint
    {
        //Create new file or if it exists, write the exception message, stacktrace, date time
        public void ErrorLog(Exception e)
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

using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommLibrary
{
    public class LogHelper
    {


        static Logger log = LogManager.GetCurrentClassLogger();

        public static void Info(Exception ex)
        {
            
            log.Info(ex.Message + "\r\n堆栈:" + ex.StackTrace);
        }
        public static void Info(string msg)
        {
            log.Info(msg);
        }
        public static void Error(Exception ex)
        {
            log.Error(ex.Message + "\r\n堆栈:" + ex.StackTrace);
        }

        public static void Error(string msg)
        {
            log.Error(msg);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

using log4net;
using log4net.Config;


namespace PADE
{

    

    public class TB4_Exception
    {
        public static List<TB4_Exception> exceptionList = new List<TB4_Exception>();
        
        public static readonly ILog logger = LogManager.GetLogger(typeof(TB4_Exception));

        private log4net.Appender.RollingFileAppender logAppender=new log4net.Appender.RollingFileAppender();

        
        public TB4_Exception()
        {
            //DOMConfigurator.Configure();
            Console.WriteLine("CONFIGURED.");
            XmlConfigurator.Configure();
        }

        public static void logConsoleOnly(string text_msg)
        {
            Console.WriteLine(text_msg);
        }

        public static void logInfo(string text, bool addTimeStamp, bool ConsoleOnly = false)
        {
            Console.WriteLine("logInfo:" + text);
            if (!ConsoleOnly)
            {
                if (addTimeStamp) logger.Info(DateTime.Now.ToString() + "  " + text);
                else logger.Info(text);
            }
        }

        public static void logError(Exception ex, string details, bool addTimeStamp)
        {
            Console.WriteLine("logError:"+details);
            if (addTimeStamp) logger.Error(DateTime.Now.ToString() + "  " + details, ex);
            else logger.Error(details, ex);
            //MessageBox.Show(ex.Message + "  " + details);
        }


        public static void newHeader()
        {
            logInfo("\n\n\n\n\n", false);
           
            logInfo("Application run", true);


        }
        public enum severity
        {
            log = 1, //for simple logging (e.x. log every time Paul runs a test)
            debug = 2, //for testing.  Will print to the console.
            warning = 3, // an exception was caught in a non-essential block of code
            error = 4, //an exception was caught in a subroutine that had to be prematurely exited as a result
            fatal = 5 //the application crashes
        }

        public static Boolean throwSystemExceptionOnFatal=false;

        public static ToolStripStatusLabel statusLabel;

        
        public Exception p_ex;
        public DateTime p_time;




        #region Comparisons
        public static Comparison<TB4_Exception> DateComparison =
        delegate(TB4_Exception e1, TB4_Exception e2)
        {
            return e1.p_time.CompareTo(e2.p_time);
        };

       
        #endregion

    }
  
}

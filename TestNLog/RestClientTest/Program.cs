using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestClientTest
{
    static class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LogingConfiguration();
            Application.Run(new Form1());
        }

        private static void LogingConfiguration()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(SMKOtherErrorHandle);
            Application.ThreadException += ApplicationError;
        }

        static void ApplicationError(object sender, ThreadExceptionEventArgs e)
        {
            logger.Error(e.Exception);
        }

        static void SMKOtherErrorHandle(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error(e.ExceptionObject as Exception);

        } 
    }
}

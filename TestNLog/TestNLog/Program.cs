using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace TestNLog
{
    static class Program
    {
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
            Log(e.Exception);
        }

        static void SMKOtherErrorHandle(object sender, UnhandledExceptionEventArgs e)
        {
            Log(e.ExceptionObject as Exception);

        }

        private static void Log(Exception e)
        {
            //MessageBox.Show("some error");
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info(e);
            //logger.Debug(e);
        }


       
    }
}

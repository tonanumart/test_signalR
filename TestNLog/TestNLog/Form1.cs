using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TestNLog.Background;
using TestNLog.MySystem;

namespace TestNLog
{
    public partial class Form1 : Form
    {
        private static BackgroundStatus singleStatus;
        private static int InitWait = 1000;
        public Form1()
        {
            InitializeComponent();
            this.signalRWorker.DoWork += SignalRController.DoWork;
            this.signalRWorker.ProgressChanged += (sender, progress) =>
            {
                var data = progress.UserState as SignalRSResult;
                if (data != null)
                {
                    singleStatus.SignalR = data.EnumValue;
                    this.signalRStatus.Text = singleStatus.SignalR.ToString();
                }
            };
            this.signalRWorker.RunWorkerCompleted += SignalRController.Complete;
        }

        private void button1_Click(object sender, EventArgs a)
        {
            callStack1();
        }

        private void callStack1()
        {
            callStack2();
        }

        private void callStack2()
        {
            callStack3();
        }

        private void callStack3()
        {
            try
            {
                throw new Exception("Trace Log");
            }
            catch (Exception e)
            {
                var logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Info(e);
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            singleStatus = new BackgroundStatus();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            this.signalRWorker.RunWorkerAsync(InitWait);
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.cancelBtn.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TestThrows.WhereIsMyFile();

        }



    }
}

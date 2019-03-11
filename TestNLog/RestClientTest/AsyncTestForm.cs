using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestClientTest
{
    public partial class AsyncTestForm : Form
    {
        private static Logger logger = LogManager.GetLogger("log2");
        private static Random gen = new Random();
        private static Semaphore semaphore = new Semaphore(1, 1);

        private static object another = new object();

        public Form1 _form1;
        public AsyncTestForm()
        {
            InitializeComponent();
        }

        public AsyncTestForm(Form1 form)
            : this()
        {
            _form1 = form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            _form1.Show();
        }

        private void AsyncTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _form1.Show();
        }

        public static int counter = 0;
        private async void button2_Click(object sender, EventArgs e)
        {
            counter++;
            var id = counter;
            var rand = gen.Next(1);
            await RequestWebMock(id, rand);
           // logger.Info("{0} : {1} sec complete", id, rand);
        }



        private async Task RequestWebMock(int id, int rand)
        {
            //logger.Info("{0} :{1} sec start", id, rand);
            await Task.Delay(1 * rand);
            await SelectLock(id);
        }

        private async Task SelectLock(int id)
        {
            bool canlock = false;
            try
            {
                canlock = semaphore.WaitOne(0);
                if (!canlock)
                {
                    logger.Info("{0} : other lock this finish job", id);
                    return;
                }
                processBtn.Enabled = false;
                logger.Info("{0} : access Lock!!!!", id);
                //await Task.Delay(15000);
                //want to lock here
                await Task.Run(() =>
                {
                    lock (another)
                    {
                        logger.Info("main lock another");
                        Thread.Sleep(10000);
                        logger.Info("main release another");
                    }
                });


                logger.Info("{0} : release Lock!!!!", id);
                await Task.Delay(500);
            }
            finally
            {
                if (canlock)
                {
                    semaphore.Release();
                    processBtn.Enabled = true;
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int sleepTime = gen.Next(3) * 1000;
            for (int i = 0; i < 10; i++)
            {
                int round = i + 1;
                Task.Run(() =>
                {
                    OnePrint(round);
                });
            }
        }

        private void OnePrint(int id)
        {
            int sec = gen.Next(5);
            int sec2 = gen.Next(5);
            logger.Info("before sub(id {0}) (simulate sec : {1})", id, sec2);
            Thread.Sleep(sec2 * 1000); //simulate wait main interrupt lock
            logger.Info("after sub(id {0}) (simulate sec : {1})", id, sec2);
            lock (another)
            {
                logger.Info("sub(id {0}) in lock another {1} sec", id, sec);
                Thread.Sleep(sec * 1000);
            }
        }


    }
}

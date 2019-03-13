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

namespace SemaphoreForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private static SemaphoreSlim mLock = new SemaphoreSlim(3);

        private static int click = 0;
        private static int opened = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            click++;
            NewForm(click);
            label1.Text = click.ToString();
        }

        private async void NewForm(int clicked)
        {
            var frm = await CallingForm(clicked);
            opened++;
            TaskResolve();
            frm.Show();
        } 

        private void TaskResolve()
        {
            label2.Text = opened.ToString();
        }


        private async Task<Form2> CallingForm(int id)
        {
            Form2 frm = new Form2();
            frm.FormClosed += (mSender, arg) =>
            {
                Console.WriteLine("{0} release lock", id);
                mLock.Release(); 
            };
            Console.WriteLine("{0} waiting to access lock", id);
            await mLock.WaitAsync(); //mLock.WaitAsync is Long Process (Until form close);
            Console.WriteLine("{0} access lock", id);
            return frm;
        }
    }
}

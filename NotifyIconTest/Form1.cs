using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotifyIconTest
{
    public partial class Form1 : Form
    {
        public static int cal = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //testIcon();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cal++;
            this.button1.Text = cal + "";
            if (cal % 2 == 0)
            {
                notifyIcon1.Icon = Properties.Resources.red1;
            }
            else
            {
                notifyIcon1.Icon = Properties.Resources.black1;
            }

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (cal % 2 == 0)
            {
                this.button1.Text = "XXXX";
            }
            else {
                this.button1.Text = "YYYY";
            }
        }


    }
}

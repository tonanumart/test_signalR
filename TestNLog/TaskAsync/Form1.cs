using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskAsync
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Full Asyn Recommemnded

        private async void button1_Click(object sender, EventArgs e)
        {
            var task = DoSomethingAsync();
            Console.WriteLine("Your time has cost cannot wait");
            int result = await task;
            Console.WriteLine("Your result DoSomethingAsync() = {0}", result);
            resultLabel.Text = result.ToString();
        }


        public async Task<int> DoSomethingAsync()
        {
            Console.WriteLine("Start DoSomethingAsync()");
            // In the real world, we would do something particular.. 
            // For this example, we are only going to (asynchronously) wait 100ms
            await Task.Delay(5000);
            Console.WriteLine("DoSomethingAsync return result");
            return 1;
        }

        #endregion


        #region Synchonous Recommended
        private void button2_Click(object sender, EventArgs e)
        {
            HardWork();
        }

        private async void HardWork()
        {
            var resultTask = Task.Run(() => DoSomething());
            Console.WriteLine("Your time has cost cannot wait");
            int result = await resultTask;
            resultLabel.Text = result.ToString();
        }

        private int DoSomething()
        {
            Console.WriteLine("Start DoSomething()");
            for (int i = 0; i != Int32.MaxValue; ++i)
                ;
            Console.WriteLine("Finish DoSomething()");
            return 42;
        }

        #endregion

    }
}

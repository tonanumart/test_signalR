using NLog;
using RestClientTest.Cls;
using RestClientTest.Cls.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestClientTest
{
    public partial class Form1 : Form
    {
        public static Logger logger = LogManager.GetLogger("log1");
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Maximum = Convert.ToInt32(numericUpDown1.Value);
            statusTextBox.Text = "Start";
            if (asyncCheckbox.Checked)
            {
                await aSyncTotalReq();
            }
            else
            {
                SyncReq();
            }
            statusTextBox.Text = "Finish";
        }



        private static string NumString(int i)
        {
            return string.Format("{0}", i + 1);
        }

        private RestRequest TestConnection()
        {
            if (apiCheckBox.Checked)
                return new RestRequest("Values", Method.GET);
            return new RestRequest("Values/1", Method.GET);

        }

        private void SyncReq()
        {
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                asyncTextLabel.Text = NumString(i);
                var mRequest = TestConnection();
                SmartkorpApi.Instance.Execute<List<string>>(mRequest);
                progressBar1.Value++;
            }
            logger.Info("All Requests Complete");
        }

        private async Task aSyncTotalReq()
        {
            var task = new List<Task>();
            var progressIndicator = new Progress<string>(ReportProgress);
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                asyncTextLabel.Text = NumString(i);
                var mRequest = TestConnection();
                task.Add(SmartkorpApi.Instance.ExecuteAsync<List<string>>(mRequest, progressIndicator));
            }
            await Task.WhenAll(task);
            logger.Info("All Requests Complete");
        }

        private void ReportProgress(string obj)
        {
            if (progressBar1.Value + 1 > progressBar1.Maximum)
                return;
            progressBar1.Value++;
            counterLabel.Text = "" + progressBar1.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var saveFileDialog1 = new SaveFileDialog();

            var result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                using (saveFileDialog1.OpenFile())
                {
                    MessageBox.Show("Save File :" + saveFileDialog1.FileName);
                }
            }
            else
            {
                MessageBox.Show("Cancel");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}

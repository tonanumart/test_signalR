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

        private void SyncReq()
        {
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                asyncTextLabel.Text = (i + 1).ToString();
                var mRequest = TestConnection();
                SmartkorpApi.Instance.Execute<Result>(mRequest);
                progressBar1.Value++;
            }
        }

        private static RestRequest TestConnection()
        {
            return new RestRequest("Values/ValidateToken", Method.POST);
        }

        private async Task aSyncTotalReq()
        {
            var task = new List<Task>();
            var progressIndicator = new Progress<string>(ReportProgress);
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                asyncTextLabel.Text = (i + 1).ToString();
                var mRequest = TestConnection();
                task.Add(SmartkorpApi.Instance.ExecuteAsync<Result>(mRequest, progressIndicator));
            }
            await Task.WhenAll(task);
        }

        private void ReportProgress(string obj)
        {
            textBox1.Text = string.Format("{0}{1}{2}", textBox1.Text, Environment.NewLine, obj);
            progressBar1.Value++;
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
    }
}

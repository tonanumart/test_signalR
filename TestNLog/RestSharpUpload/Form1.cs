using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestSharpUpload
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //using (StreamWriter writetext = new StreamWriter("write.txt"))
            //{
            //    writetext.WriteLine("writing in text file");
            //}
            StreamReader readtext = new StreamReader("write.txt");
            RestClient restClient = new RestClient("http://localhost:61122/");
            RestRequest restRequest = new RestRequest("api/Upload/LogsFile");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.Method = Method.POST;
            restRequest.AlwaysMultipartFormData = true;
            //restRequest.AddHeader("Authorization", "Authorization"); 
            restRequest.AddParameter("SignalRToken", "49783572-665c-4420-87fc-27404fabd23c", ParameterType.GetOrPost);
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "write.txt");
            var files = new string[] { filePath, filePath, filePath };

            for (int i = 0; i < files.Length; i++)
            {
                restRequest.AddFile("files[" + i + "]", files[i]);
            }
            var response = restClient.Execute(restRequest);



        }
    }
}

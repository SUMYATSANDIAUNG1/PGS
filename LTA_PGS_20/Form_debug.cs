using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.Odbc;

namespace LTA_PGS_20
{
    public partial class Form_debug : Form
    {
        public Form_debug()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            http FileOne = new http();

            FileOne.FileLocalPath = "D:\\1.txt";
            FileOne.Url = System.Configuration.ConfigurationSettings.AppSettings["webserviceurl"];
            FileOne.Url = "http://192.168.126.40/lta_pgs/gdm1/gdm.1.1.txt";
            FileOne.Connect();
            if (FileOne.IsFileExist)
                MessageBox.Show("Exist");
            else
                MessageBox.Show("Not Exist");

            FileOne.Close();

        }
        private Thread mythread;
        
        private void button2_Click(object sender, EventArgs e)
        {
            http FileOne = new http();

            FileOne.FileLocalPath = "D:\\1.txt";
            FileOne.Url = System.Configuration.ConfigurationSettings.AppSettings["webserviceurl"];
            FileOne.Url = "http://192.168.126.40/lta_pgs/gdm1/gdm.1.1.txt";
            FileOne.Connect();
            if (FileOne.IsFileExist)
            {
                FileOne.DownloadFile();
                //mythread = new Thread(new ThreadStart(FileOne.DownloadFile));
                //mythread.Start();
            }
            FileOne.Close();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            http FileOne = new http();
            FileOne.Upload_Request("http://192.168.126.40/lta_pgs/upload.php", "D:\\1.txt", "gdm1/1.txt");
            //"http://192.168.126.160/lta_pgs/upload.php -F gdmid=1 -F file=@readme -F submit=Submit"
            //FileOne.SendFile("D:\\1.txt", "http://192.168.126.40/lta_pgs/gdm1/test.txt", System.Net.CredentialCache.DefaultCredentials);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            OdbcDataAdapter articleDA = new OdbcDataAdapter();
            string sd = "2009-09-01";
            string td = "2009-09-30";
            string sql = "select * from tb_alarmlog where fd_date>='" + sd + "' and fd_date<='" + td + "'";
            articleDA.SelectCommand = new OdbcCommand(sql, inc.db_control.MyConnecttion);
            OdbcCommandBuilder builder = new OdbcCommandBuilder(articleDA);


            articleDA.Fill(ds);
            ds.WriteXml("alarmlog.xml");
        }
        
        //private void timer1_Tick(object sender, System.EventArgs e)
        //{
        //    while (FileOne.IsFileDownload == true)
        //    {
        //        txtBoxMessage.Text += "下载已完成！";
        //        timer1.Stop();
        //    }
        //    long percentD;
        //    percentD = FileOne.TotalDownloaded * 100 / FileOne.FileSize;
        //    labelDownloadPercent.Text = percentD.ToString() + "%";
        //    progressBarDownload.Value = (int)FileOne.TotalDownloaded;
        //}
    }
}

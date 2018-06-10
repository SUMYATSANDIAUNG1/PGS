using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;  
using System.IO;
using System.Windows.Forms;

namespace LTA_PGS_20
{
    class http
    {
        private const int downloadBlockSize = 1024;
        public void DownloadFile()
        {
            if (this.fileLocalPath == null)
                return;
            try
            {
                //   create   the   download   buffer  
                byte[] buffer = new byte[downloadBlockSize];
                int readCount;
                //   read   a   block   of   bytes   and   get   the   number   of   bytes   read  
                while ((int)(readCount = DownloadStream.Read(buffer, 0, downloadBlockSize)) > 0)
                {
                    //   save   block   to   end   of   file  
                    SaveToFile(buffer, readCount, this.fileLocalPath);
                    //   update   total   bytes   read  
                    totalDownloaded += readCount;
                }
            }
            finally
            {
                if (response != null)
                    response.Close();
                this.isFileDownload = true;
            }
            //return   true;  
        }

        //将数据流保存到本地文件  
        private void SaveToFile(byte[] buffer, int count, string fileName)
        {
            FileStream f = null;

            try
            {
                f = File.Open(fileName, FileMode.Append, FileAccess.Write);
                f.Write(buffer, 0, count);
            }
            finally
            {
                if (f != null)
                    f.Close();
            }
        }

        public void Connect()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.url);
            response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                this.isFileExist = false;
                return;
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                fileSize = response.ContentLength;
                fileLastModifyInInternet = response.LastModified;
                this.isFileExist = true;
            }
        }
        private long fileSize;     //文件大小  
        private string fileLocalPath;   //文件本地路径  
        private DateTime fileLastModifyOnLocal;     //本地最后修改时间  
        private DateTime fileLastModifyInInternet;     //网上最后修改时间  
        private HttpWebResponse response;
        private Stream stream;     //取得返回流  
        private long start;     //文件中下载点  
        private bool isFileExist;     //网络中文件是否存在？  
        private bool isFileDownload;     //文件是否下载完成?  
        private string url;     //文件的网络地址  
        private long totalDownloaded;   //已下载的数据量  

        #region     //定义属性
        public Stream DownloadStream
        {
            get
            {
                if (this.start == this.fileSize)
                    return Stream.Null;
                if (this.stream == null)
                    this.stream = this.response.GetResponseStream();
                return this.stream;
            }
        }

        public long TotalDownloaded
        {
            get
            {
                return this.totalDownloaded;
            }
        }
        public string FileLocalPath     //本地文件路径  
        {
            set
            {
                this.fileLocalPath = value;
            }
            get
            {
                return this.fileLocalPath;
            }
        }
        public string Url   //下载文件的网络地址  
        {
            set
            {
                this.url = value;
            }
            get
            {
                return this.url;
            }
        }
        public bool IsFileExist       //互联网上文件是否存在？  
        {
            get
            {
                return this.isFileExist;
            }
        }
        public bool IsFileDownload     //文件是否已下载完成  
        {
            get
            {
                return this.isFileDownload;
            }
        }
        public long FileSize     //文件大小  
        {
            get
            {
                return this.fileSize;
            }
        }
        public DateTime FileLastModifyOnLocal     //本地中文件最近下载的时间  
        {
            set
            {
                this.fileLastModifyOnLocal = value;
            }
            get
            {
                return this.fileLastModifyOnLocal;
            }
        }
        public DateTime FileLastModifyInInternet     //互联网中文件最近修改的时间  
        {
            get
            {
                return this.fileLastModifyInInternet;
            }
        }
        #endregion

        public void Close()
        {
            this.response.Close();
        }
        private bool run(string name, string path)
        {

            //System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
            System.Diagnostics.Process Proc = new System.Diagnostics.Process();

            Proc.StartInfo.FileName = name;
            Proc.StartInfo.Arguments = path;
            Proc.StartInfo.CreateNoWindow = true;
            Proc.StartInfo.UseShellExecute = false;
            Proc.StartInfo.RedirectStandardOutput = true;
            
            try
            {
                //Proc = System.Diagnostics.Process.Start();
                if (Proc.Start())
                {
                    //StreamReader ss = Proc.StandardOutput;
                    string _temp = Proc.StandardOutput.ReadToEnd();
                    //MessageBox.Show(_temp);
                    if (_temp.IndexOf("Stored in") >= 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
//                MessageBox.Show(e.Message);
                return false;
            }
        }



        // <summary>
        /// 将本地文件上传到指定的服务器(HttpWebRequest方法)
        /// </summary>
        /// <param name="address">文件上传到的服务器</param>
        /// <param name="fileNamePath">要上传的本地文件（全路径）</param>
        /// <param name="saveName">文件上传后的名称</param>
        /// <param name="progressBar">上传进度条</param>
        /// <returns>成功返回1，失败返回0</returns>
        //private int Upload_Request(string address, string fileNamePath, string saveName, ProgressBar progressBar)
        public bool Upload_Request(string address, string fileNamePath, string saveName)
        {            
            string _path = fileNamePath;
            if (fileNamePath.Substring(0, 3) == "cms")
                _path = "cmsid=" + fileNamePath.Substring(3);
            else if (fileNamePath.Substring(0, 3) == "gdm")
                _path = "gdmid=" + fileNamePath.Substring(3);
            else
                _path = "setting=1";

            string _para = address + " -F " + _path + " -F file=@" + saveName + " -F submit=Submit";
            try
            {
                if (run("curl.exe", _para))
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }
/*
        private Thread mythread;

        private void Download_Click(object sender, System.EventArgs e)
        {
            FileOne.FileLocalPath = LocalPathOfFile.Text;
            FileOne.Url = UrlOfFile.Text;
            FileOne.Connect();
            if (FileOne.IsFileExist == false)
                return;
            progressBarDownload.Minimum = 0;
            progressBarDownload.Maximum = (int)FileOne.FileSize;
            timer1.Enabled = true;
            txtBoxMessage.Text = FileOne.FileSize.ToString();
            mythread = new Thread(new ThreadStart(FileOne.DownloadFile));
            mythread.Start();

        }
        private Downloader FileOne = new Downloader();
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            if (FileOne.IsFileDownload == true)
            {
                txtBoxMessage.Text += "下载已完成！";
                timer1.Stop();
            }
            long percentD;
            percentD = FileOne.TotalDownloaded * 100 / FileOne.FileSize;
            labelDownloadPercent.Text = percentD.ToString() + "%";
            progressBarDownload.Value = (int)FileOne.TotalDownloaded;
        }
 * */
    }
}
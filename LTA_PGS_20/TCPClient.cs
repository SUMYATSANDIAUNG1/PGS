using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace LTA_PGS_20
{
    class TCPClient
    {
        #region Paramiter
        /// <summary>
        /// Server Address
        /// </summary>
        private string strServerAdd;
        public string ServerAdd
        {
            get
            {
                return strServerAdd;
            }
            set
            {
                strServerAdd = value;
            }
        }

        /// <summary>
        /// Server Port
        /// </summary>
        private int intServerPort;
        public int ServerPort
        {
            get
            {
                return intServerPort;
            }
            set
            {
                intServerPort = value;
            }
        }

        /// <summary>
        /// Server Port
        /// </summary>
        private int intReceviceTimeOut;
        public int ReceviceTimeOut
        {
            get
            {
                return intReceviceTimeOut;
            }
            set
            {
                intReceviceTimeOut = value;
            }
        }

        /// <summary>
        /// Connect Status
        /// </summary>
        private bool boolConnected;
        public bool Connected
        {
            get
            {
                return boolConnected;
            }
            set
            {
                boolConnected = value;
            }
        }

        /// <summary>
        /// Connect Status
        /// </summary>
        private TcpClient tcpclient;

        #endregion

        #region constructor
        ///constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public TCPClient()
        {
            intServerPort = -1;
            boolConnected = false;
            strServerAdd = "";
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="filename"></param>
        public TCPClient(string Address,int port)
        {
            strServerAdd = Address;
            boolConnected = false;
            intServerPort = port;            
        }
        #endregion

        #region Connect to Server
        public void connect()
        {
            try
            {
                tcpclient = new TcpClient();
                
                tcpclient.Connect(strServerAdd, intServerPort);
                
                Connected = true;                
            }
            catch (Exception e)
            {
                Tools.write_log(e.Message);
            }
        }
        #endregion

        #region Send Buffer To Server
        public int SendBuffer(string str)
        {

            try
            {
                byte[] bt = new byte[str.Length];
                int n = 0;
                for (int i = 0; i < str.Length; i += 1)
                {
                    string s = str.Substring(i, 1);
                    int a = (int)Convert.ToChar(s);
                    byte bt1 = Byte.Parse(a.ToString(), System.Globalization.NumberStyles.Integer);
                    bt[n] = bt1;
                    n++;
                }                
                tcpclient.Client.Send(bt);
                return n;
            }
            catch { return 0; }
        }
        #endregion

        #region Send Buffer To Server
        public int ReadBuffer(ref string str)
        {
            int r=0;
            string r_str = "";
            tcpclient.Client.ReceiveTimeout = intReceviceTimeOut * 1000;
            while (true)
            {
                try
                {
                    byte[] bt = new byte[1024];
                    int rev = tcpclient.Client.Receive(bt);

                    r += rev;
                    r_str += Tools.bytes_to_str(bt, rev);

                    if (rev < 1024)
                        break;
                }
                catch (Exception ex)
                {
                    //throw ex;
                    disconnect();
                    return 0; 
                }
            }
            str = r_str;
            return r;
        }
        #endregion

        #region disconnect
        public void disconnect()
        {
            try
            {
                tcpclient.Close();
                Connected = false;
            }
            catch (Exception e)
            {
                Tools.write_log(e.Message);
            }
        }
        #endregion
    }
}
        
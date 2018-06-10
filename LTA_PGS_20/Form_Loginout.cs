using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;

namespace LTA_PGS_20
{
    public delegate void CallBack_LoadMenu();
    public partial class Form_Loginout : Form
    {                
        public event CallBack_LoadMenu MyEnvent;
        public Form_Loginout()
        {
            InitializeComponent();
        }

        private void first_login(DateTime t_pwd)
        {
            int _days = (DateTime.Now - t_pwd).Days;
            _days = (DateTime.Now - t_pwd).Days;
            if (_days > 730)
            {
                Form_Change_PWD f = new Form_Change_PWD();
                f.first_login = true;
                f.ShowDialog();
            }           
        }

        private void confirm()
        {
            inc.username = textBox1.Text;
            string _pwd = textBox2.Text;

            inc.db_control.SQL = "select fd_id,fd_password,fd_trytimes,fd_lasttime from tb_users where fd_username='" + inc.username + "'";
            OdbcDataReader MyReader = inc.db_control.SQLExecuteReader();
            if (MyReader != null)
            {
                while (MyReader.Read())
                {
                    Int32 _trytimes = (Int32)MyReader["fd_trytimes"];
                    if (_trytimes >= 3)
                    {
                        MessageBox.Show("The User ID is blocked,please connect administrator");
                        return;
                    }
                    string _password = (string)MyReader["fd_password"];
                    string _temp_pwd = Tools.UserMd5(_pwd);

                    if ((_password != Tools.UserMd5_old(_pwd)) && (_password != Tools.GetSHA512(_pwd)))
                    {
                        MessageBox.Show("Password Wrong");
                        inc.db_control.SQL = "update tb_users set fd_trytimes=" + (_trytimes + 1).ToString()
                            + " where fd_username='" + inc.username + "'";
                        inc.db_control.SQLExecuteReader();
                        return;
                    }
                    else
                    {
                        inc.currentuser = (int)(Int64)MyReader["fd_id"];
                        inc.currentpwd = _temp_pwd;

                        if (_password == Tools.UserMd5_old(_pwd))
                        {
                            inc.db_control.SQL = "update tb_users set fd_password='" + _temp_pwd + "' where fd_id='" + inc.currentuser + "'";
                            inc.db_control.SQLExecuteReader();                            
                        }
                        

                        TimeSpan tp = new TimeSpan(740, 0, 0, 0, 0);
                        DateTime t_last = MyReader["fd_lasttime"] == DBNull.Value ? (DateTime.Now - tp) : ((DateTime)MyReader["fd_lasttime"]);
                        first_login(t_last);
                        
        
                        inc.db_control.SQL = "update tb_users set fd_trytimes=0"
                             + ", fd_lasttime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                             + "' where fd_username='" + inc.username + "'";

                        inc.db_control.SQLExecuteReader();

                        fn_global.log_operateion(0, "Login", 1);

                        inc.log_off = false;
                        inc.auto_log_off = inc.USER_TIME_OUT;

                        if (this.MyEnvent != null)    //为了防止异常 最好加上这么个判断
                        {
                            this.MyEnvent();
                        }
                        Close();
                        return;
                    }
                }
            }
            MessageBox.Show("User Name Error");
        }
        private void button1_Click(object sender, EventArgs e)
        {            
            confirm();            
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Application.Exit();
            }
            
        }

        private void Form_Loginout_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {           
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                confirm();
            }   
        }

        private void Form_Loginout_Load(object sender, EventArgs e)
        {
            
        }

        private void Form_Loginout_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (MessageBox.Show("Exit Application?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //{
            //    e.Cancel = false;
            //    Application.Exit();
            //}           
            //else
            //    e.Cancel = true;
        }
    }
}

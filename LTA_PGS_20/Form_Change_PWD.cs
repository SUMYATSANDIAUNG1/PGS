using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LTA_PGS_20
{
    public partial class Form_Change_PWD : Form
    {
        public bool first_login = false;
        public Form_Change_PWD()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string _pwd = Tools.UserMd5(textBox3.Text);
            if (Tools.UserMd5(textBox3.Text) != inc.currentpwd)
            {
                MessageBox.Show("Current Password is wrong.Please check");
                return;
            }
            if (textBox1.Text.Length < 8)
            {
                MessageBox.Show("At least 8 Charaters");
                return;
            }
            if (textBox1.Text != textBox2.Text)
            {
                MessageBox.Show("Passwords doesn't match.Please check.");
                return;
            }

            if (fn_global.checkpwd(inc.username, Tools.UserMd5(textBox1.Text)))
            {
                inc.db_control.SQL = "update tb_users set fd_password='" + Tools.UserMd5(textBox1.Text)
                                + "' where fd_username='" + inc.username + "'";
                if (inc.db_control.SQLExecuteReader() != null)
                {
                    fn_global.udpate_users_password_time(inc.currentuser);

                    inc.currentpwd = Tools.UserMd5(textBox1.Text);

                    MessageBox.Show("Change Password Successful");

                    first_login = false;
                    Close();
                }
                else
                    MessageBox.Show("Change Password Failed");
            }
            else
                MessageBox.Show("Password used more than 3 times,please change a new password!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_Change_PWD_Load(object sender, EventArgs e)
        {
            textBox3.Focus();
            if (first_login)
                button2.Enabled = false;
        }

        private void Form_Change_PWD_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (first_login)
                e.Cancel = true;
        }
    }
}

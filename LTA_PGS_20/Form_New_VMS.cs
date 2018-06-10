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
    public delegate void CallBack_Confirm();
    public partial class Form_New_VMS : Form
    {
        public event CallBack_Confirm MyEnvent;
        public Form_New_VMS()
        {
            InitializeComponent();
        }
        
        private bool checkdata()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please key in ID");
                return false;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Please key in name");
                return false;
            }
            if (comboBox_area.SelectedIndex<0 )
            {
                MessageBox.Show("Please select area");
                return false;
            }
            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {            
            if (!checkdata())
                return;
                  
            inc.db_control.SQL = "insert into tb_gdm (fd_id,fd_name,fd_location,fd_x,fd_y,fd_map,fd_xout,fd_yout,fd_dim,fd_flash,fd_timer,fd_ip,fd_udp,fd_memo,fd_disablepixel,fd_delete,fd_area) values ('" 
                + textBox1.Text + "','"
                + textBox2.Text + "','','0','0','0','0','0','0','0','2','','0','','5','0'," + (comboBox_area.SelectedIndex+1).ToString() + ")";
            try
            {
                if (inc.db_control.SQLExecuteReader() != null)
                {
                    fn_global.log_operateion((int)inc.LOGMSGCODE.VMI01, textBox1.Text, 1);
                    MessageBox.Show("add new VMS Successful");
                }
                else
                    MessageBox.Show("add new VMS Failed");
            }
            catch {
                MessageBox.Show("add new VMS Failed");
            }            

            if (this.MyEnvent != null)    //为了防止异常 最好加上这么个判断
            {
                this.MyEnvent();
            }

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_New_VMS_Load(object sender, EventArgs e)
        {
            comboBox_area.Items.AddRange(inc.Area);
        }
    }
}

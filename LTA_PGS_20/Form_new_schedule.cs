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
    public delegate void CallBack_New_Schedule();
    public partial class Form_new_schedule : Form
    {
        public event CallBack_New_Schedule MyEnvent;
        public Form_new_schedule()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                inc.db_control.SQL = inc.db_control.insert_table("tb_schedule", "", textBox_user_name.Text, "0", "0", inc.currentuser.ToString(), ""
                    , "", "1800-01-01 00:00:00", "1800-01-01 00:00:00", "0", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (inc.db_control.SQLExecuteReader() != null)
                {
                    //MessageBox.Show("Add New Schedule Successful");
                    fn_global.log_operateion((int)inc.LOGMSGCODE.SCI01, textBox_user_name.Text, 1);
                    if (this.MyEnvent != null)    //为了防止异常 最好加上这么个判断
                    {
                        this.MyEnvent();
                    }
                }
                else
                    MessageBox.Show("Add New Schedule Failed");
            }
            catch { MessageBox.Show("Add New Schedule Failed"); }             
            Close();
        }

        private void Form_new_schedule_Load(object sender, EventArgs e)
        {

        }
    }
}

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
    public partial class Form_Time_Config : Form
    {
        public int _index_adhox = -1;

        public bool isadhoc = true;
        public DateTime Start_time = new DateTime();
        public DateTime End_time = new DateTime();
        public bool useendtime = false;

        public Form_Time_Config()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isadhoc)
            {
                if (checkBox1.Checked)
                    inc.Adhoc_Group[_index_adhox].useendtime = 1;
                else
                    inc.Adhoc_Group[_index_adhox].useendtime = 0;
                inc.Adhoc_Group[_index_adhox].starttime = dateTimePicker1.Value;
                inc.Adhoc_Group[_index_adhox].endtime = dateTimePicker2.Value;
            }
            else
            {
                if (checkBox1.Checked)
                    inc.tb_schedule[_index_adhox].useendtime = 1;
                else
                    inc.tb_schedule[_index_adhox].useendtime = 0;
                inc.tb_schedule[_index_adhox].starttime = dateTimePicker1.Value;
                inc.tb_schedule[_index_adhox].endtime = dateTimePicker2.Value;
            }
            Close();
        }

        private void Form_Time_Config_Load(object sender, EventArgs e)
        {
            TimeSpan ts = new TimeSpan(365,0,0,0,0);
            checkBox1.Checked = useendtime;
            try
            {
                if(Start_time<(DateTime.Now-ts))
                    dateTimePicker1.Value = DateTime.Now;
                else
                    dateTimePicker1.Value = Start_time;
            }
            catch { dateTimePicker1.Value = DateTime.Now; }
            try
            {
                if (End_time < (DateTime.Now - ts))
                    dateTimePicker2.Value = DateTime.Now;
                else
                    dateTimePicker2.Value = End_time;
            }
            catch { dateTimePicker2.Value = DateTime.Now; }
        }
    }
}

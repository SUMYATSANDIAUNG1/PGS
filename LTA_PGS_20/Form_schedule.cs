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
    public partial class Form_schedule : Form
    {
        public Form_schedule()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int i = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Message" + i);            

            Rows_Data rd = new Rows_Data();
            rd.Name = "msg" + i;
            tabControl1.TabPages[i].Controls.Add(rd);

            tabControl1.SelectedIndex = i;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabControl1.TabPages[tabControl1.SelectedIndex]);
        }

        private void Form_schedule_Load(object sender, EventArgs e)
        {

        }
    }
}

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
    public delegate void CallBack_Detail();
    public partial class Form_VMS_detail : Form
    {
        public event CallBack_Detail MyEnvent;
        public Form_VMS_detail()
        {
            InitializeComponent();

            comboBox_name_color.Items.AddRange(inc.VMS_Name_color);
            comboBox_name_color.DropDownStyle = ComboBoxStyle.DropDownList;
            
            comboBox_lots_color.Items.AddRange(inc.VMS_Lots_color);
            comboBox_lots_color.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox_mode.Items.AddRange(inc.VMS_Color_mode);
            comboBox_mode.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button45_Click(object sender, EventArgs e)
        {
            try
            {
                inc.VMS_Detail.green = int.Parse(textBox_green.Text);
            }
            catch
            {
                inc.VMS_Detail.green = 100;
            }
            try{
            inc.VMS_Detail.red = int.Parse(textBox_red.Text);
            }
            catch { inc.VMS_Detail.red = 0; }
            inc.VMS_Detail.mode = comboBox_mode.SelectedIndex+1;
            inc.VMS_Detail.lots_color = comboBox_lots_color.SelectedIndex+1;
            inc.VMS_Detail.name_color = comboBox_name_color.SelectedIndex+1;
            inc.VMS_Detail.update = true;
            if (this.MyEnvent != null)    //为了防止异常 最好加上这么个判断
            {
                this.MyEnvent();
            }
            Close();
        }

        private void button46_Click(object sender, EventArgs e)
        {
            inc.VMS_Detail.update = false;
            Close();
        }

        private void Form_VMS_detail_Load(object sender, EventArgs e)
        {            
            try { comboBox_mode.SelectedIndex = inc.VMS_Detail.mode-1; }
            catch { }
            try
            {
                comboBox_lots_color.SelectedIndex = inc.VMS_Detail.lots_color-1;
            }
            catch { }
            try
            {
                comboBox_name_color.SelectedIndex = inc.VMS_Detail.name_color-1;
            }
            catch { }

            textBox_green.Text = inc.VMS_Detail.green.ToString();
            textBox_red.Text = inc.VMS_Detail.red.ToString();
            
        }
    }
}

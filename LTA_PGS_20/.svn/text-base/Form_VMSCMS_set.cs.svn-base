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
    public delegate void CallBack_Station();
    public partial class Form_VMSCMS_set : Form
    {
        public event CallBack_Station MyEnvent;
        public int main_page = -1;
        public Point New_Location = new Point();

        public Form_VMSCMS_set()
        {
            InitializeComponent();

            for (int i = 0; i < inc.tb_area.Length; i++)
            {
                comboBox1.Items.Add(inc.tb_area[i].name);
            }
            for (int i = 0; i < inc._vms.Length; i++)
            {
                comboBox1.Items.Add(inc._vms[i].name);
            }
            for (int i = 0; i < inc.Carpark.Length; i++)
            {
                comboBox1.Items.Add(inc.Carpark[i].name);
            }
            
        }

        private void Form_VMSCMS_set_Load(object sender, EventArgs e)
        {
             fn_global.load_vms_cms_treeview(treeView1, main_page);           
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            fn_global.show_treeview_location(treeView1, ((ComboBox)sender).Left, ((ComboBox)sender).Top + ((ComboBox)sender).Height);         
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (this.treeView1.SelectedNode.Nodes.Count == 0)
            {
                comboBox1.Text = this.treeView1.SelectedNode.Text;
                if (main_page==-1)
                {
                    int i = fn_global.fn_area_name2index(treeView1.SelectedNode.Text);
                    if (i >= 0)
                    {
                        textBox1.Text = "MainPage";
                        textBox2.Text = inc.tb_area[i].name;                        
                        textBox3.Text = inc.tb_area[i].x.ToString();
                        textBox4.Text = inc.tb_area[i].y.ToString();
                    }
                }
                else
                {
                    int i = fn_global.fn_area_name2index(treeView1.SelectedNode.Parent.Text);
                    if (i >= 0)
                    {
                        textBox1.Text = inc.tb_area[i].name;
                    }
                    i = fn_global.fn_vms_name2index(treeView1.SelectedNode.Text);
                    if (i >= 0)
                    {
                        textBox2.Text = inc._vms[i].name;
                        textBox3.Text = inc._vms[i].x.ToString();
                        textBox4.Text = inc._vms[i].y.ToString();
                    }
                    else
                    {
                        i = fn_global.fn_cms_name2index(treeView1.SelectedNode.Text);
                        if (i >= 0)
                        {
                            textBox2.Text = inc.Carpark[i].name;
                            textBox3.Text = inc.Carpark[i].map_point.X.ToString();
                            textBox4.Text = inc.Carpark[i].map_point.Y.ToString();
                        }
                    }
                }
                textBox6.Text = New_Location.X.ToString();
                textBox5.Text = New_Location.Y.ToString();
                treeView1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
                return;
            Point _po = new Point(int.Parse(textBox6.Text), int.Parse(textBox5.Text));            

            if (main_page==-1)
            {
                int i = fn_global.fn_area_name2index(comboBox1.Text);
                inc.tb_area[i].x = _po.X;
                inc.tb_area[i].y = _po.Y;

                _po = fn_global.fn_currentto1024(_po);
                inc.db_control.SQL = "update tb_area set fd_x = '"
                    + _po.X.ToString() + "',fd_y='" + _po.Y.ToString() + "' where fd_areaid='" + inc.tb_area[i].areaid.ToString() + "'";
                inc.db_control.SQLExecuteReader();                
            }
            else
            {
                int i = fn_global.fn_vms_name2index(comboBox1.Text);
                if (i >= 0)
                {
                    inc._vms[i].x = _po.X;
                    inc._vms[i].y = _po.Y;

                    _po = fn_global.fn_currentto1024(_po);
                    inc.db_control.SQL = "update tb_gdm set fd_x = '"
                    + _po.X.ToString() + "',fd_y='" + _po.Y.ToString() + "' where fd_id='" + inc._vms[i]._id.ToString() + "'";
                    inc.db_control.SQLExecuteReader();
                }
                else
                {
                    i = fn_global.fn_cms_name2index(comboBox1.Text);
                    if (i >= 0)
                    {
                        inc.Carpark[i].map_point.X = _po.X;
                        inc.Carpark[i].map_point.Y = _po.Y;

                        _po = fn_global.fn_currentto1024(_po);
                        inc.db_control.SQL = "update tb_carpark set fd_x = '"
                        + _po.X.ToString() + "',fd_y='" + _po.Y.ToString() + "' where fd_id='" + inc.Carpark[i].id.ToString() + "'";
                        inc.db_control.SQLExecuteReader();
                    }
                }
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
    }
}

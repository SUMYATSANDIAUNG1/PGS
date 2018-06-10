using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LTA_PGS_20
{
    public partial class Form_Dimming : Form
    {
        public int VMSid;
        public int fn_type;
        public string str;
        public Form_Dimming()
        {
            InitializeComponent();
        }

        private void Form_Dimming_Load(object sender, EventArgs e)
        {
            groupBox1.Text = inc._vms[fn_global.fn_vms_id2index(VMSid)].name + " Dimming Configuation";
            comboBox1.Items.AddRange(inc.Dimming_mode);
            if ((fn_type == 0)||(fn_type == 1))
            {
                button1.Visible = false;

                string _workdir = fn_global.get_workdir("gdm" + VMSid.ToString());
                string f_name = "";
                if (fn_type == 1)
                    f_name = _workdir + "\\dimupdate.txt";
                else
                    f_name = _workdir + "\\dimget.txt";

                MyFile f1 = new MyFile(f_name);

                label20.Text = Tools.get_str_value(f1.read_file_line());
                string _temp0 = f1.read_file_line();
                string _temp = Tools.get_str_value(_temp0);
                if (_temp == "Disconnect")
                {
                    comboBox1.Visible = false;
                    for (int i = 1; i < 9; i++)
                        ((TextBox)this.Controls.Find("textBox" + i, true)[0]).Visible = false;

                    for (int i = 2; i <= 19; i++)
                        ((Label)this.Controls.Find("label" + i, true)[0]).Visible = false;
                    textBox9.Visible = false;
                    label1.Text = _temp0;
                }
                else
                {
                    comboBox1.Enabled = false;
                    for (int i = 1; i <= 9; i++)
                        ((TextBox)this.Controls.Find("textBox" + i, true)[0]).ReadOnly = true;
                    if (fn_type == 1)
                    {
                        comboBox1.Text = _temp;
                        for (int i = 1; i < 9; i++)
                            ((TextBox)this.Controls.Find("textBox" + i, true)[0]).Text = Tools.get_str_value(f1.read_file_line());
                        textBox9.Text = Tools.get_str_value(f1.read_file_line());   
                    }
                    else
                    {                        
                        ((TextBox)this.Controls.Find("textBox" + 1, true)[0]).Text = _temp;
                        for (int i = 2; i < 9; i++)
                            ((TextBox)this.Controls.Find("textBox" + i, true)[0]).Text = Tools.get_str_value(f1.read_file_line());
                        comboBox1.Text = Tools.get_str_value(f1.read_file_line());
                        textBox9.Text = Tools.get_str_value(f1.read_file_line());
                    }
                    
                }
                f1.close();
            }
            else 
            {
                button1.Visible = true;

                string _workdir = fn_global.get_workdir("gdm" + VMSid.ToString());
                string f_name="";
                
                    f_name = _workdir + "\\dimming.txt";

                    MyFile f1 = new MyFile(f_name);

                    comboBox1.Text = Tools.get_str_value(f1.read_file_line());
                    for (int i = 1; i < 9; i++)
                        ((TextBox)this.Controls.Find("textBox" + i, true)[0]).Text
                            = Tools.get_str_value(f1.read_file_line());
                    textBox9.Text = Tools.get_str_value(f1.read_file_line());

                    f1.close();

                
            }              
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string _workdir = fn_global.get_workdir("gdm" + VMSid.ToString());
                string f_name = _workdir + "\\dimming.txt";
                //if (File.Exists(f_name))
                //    File.Delete(f_name);
                if (fn_global.fn_file_exist(_workdir, "dimming.txt"))
                    fn_global.fn_file_delete(_workdir, "dimming.txt");

                MyFile f1 = new MyFile(f_name);
                f1.write_to_file("dim_mode=" + comboBox1.Text);
                f1.write_to_file("dim_level1=" + textBox1.Text);
                f1.write_to_file("dim_level2=" + textBox2.Text);
                f1.write_to_file("dim_level3=" + textBox3.Text);
                f1.write_to_file("dim_level4=" + textBox4.Text);
                f1.write_to_file("dim_level5=" + textBox5.Text);
                f1.write_to_file("dim_level6=" + textBox6.Text);
                f1.write_to_file("dim_level7=" + textBox7.Text);
                f1.write_to_file("dim_level8=" + textBox8.Text);
                f1.write_to_file("file_type=" + textBox9.Text);
                f1.close();

                MessageBox.Show("Save Dimming Successful");
            }
            catch 
            {
                MessageBox.Show("Save Dimming Failed");
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

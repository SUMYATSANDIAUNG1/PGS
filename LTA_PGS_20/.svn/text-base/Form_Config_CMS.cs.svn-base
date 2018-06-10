using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace LTA_PGS_20
{
    public partial class Form_Config_CMS : Form
    {
        public int CMS_id;
        public Form_Config_CMS()
        {
            InitializeComponent();
        }
        private bool check_cms_in_vms(int vms_index)
        {
            for (int i = 0; i < 6; i++)
            {
                string _name = "gdm." + inc._vms[vms_index]._id.ToString() + "." + (i + 1).ToString() + ".txt";
                string _path = "gdm" + inc._vms[vms_index]._id.ToString();
                string _workdir = fn_global.get_workdir(_path);
                string file_name = _workdir + "\\" + _name;

                if (fn_global.fn_file_exist(_path, _name))
                {
                    if (fn_global.fn_get_file(_path, _name, _name))
                    {
                        MyFile f = new MyFile();

                        f.FileName = file_name;
                        for (int j = 1; j < 4; j++)
                        {
                            string name = "cp" + j.ToString() + "_cms_id";
                            string temp = "";
                            temp = f.search_file_line(name);

                            if (temp == CMS_id.ToString())
                                return true;
                        }
                        f.close();
                    }
                }
            }
            return false;
        }

        private void add_timezone_data()
        {
            string _week="";
            //dataGridView3.Rows.Clear();
            if (comboBox1.SelectedIndex != 6)
            {
                _week = "." + (comboBox1.SelectedIndex + 1).ToString();
            }
            string _path = "cms" + CMS_id.ToString();
            string _name = "cms." + CMS_id.ToString() + _week + ".txt";

            for (int r = 0; r < dataGridView3.Rows.Count; r++)
            {
                for (int j = 1; j <= 16; j++)
                {
                    dataGridView3.Rows[r].Cells[j].Value = "";
                }
                dataGridView3.EndEdit();
            }

            if (!fn_global.fn_get_file(_path, _name, _name))
            {                               
                return;
            }

            MyFile ftimeoffset = new MyFile();
            try
            {                
                string _workdir = fn_global.get_workdir(_path);
                ftimeoffset.FileName = _workdir + "\\" + _name;
                
                for (int r = 0; r < dataGridView3.Rows.Count; r++)
                {
                    int i = fn_global.fn_vms_name2index(dataGridView3.Rows[r].Cells[0].Value.ToString());
                    if(i>=0)
                    {
                        string name_head = "gdm" + inc._vms[i]._id.ToString() + "_";
                        for (int j = 1; j <= 16; j++)
                        {
                            string name = name_head + "zone" + j.ToString();
                            dataGridView3.Rows[r].Cells[j].Value = ftimeoffset.search_file_line(name);
                        }
                        dataGridView3.EndEdit();
                    }                    
                }
  
                ftimeoffset.close();  
            }
            catch(Exception e) 
            {
                MessageBox.Show(e.Message);
                ftimeoffset.close();
                //ftimeoffset = new MyFile("temp");
            }            
        }

        private void Form_Config_CMS_Load(object sender, EventArgs e)
        {
            textBox8.Text = inc.Carpark[fn_global.fn_cms_id2index(CMS_id)].name;
            comboBox1.Items.AddRange(inc.WeekDays);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            if (!fn_global.fn_get_file("setting", "cms_timezone.txt", "cms_timezone.txt"))
                return;

            string _workdir = fn_global.get_workdir("setting");
            MyFile ftimezone = new MyFile( _workdir + "\\cms_timezone.txt");
            string _temp = "";
            int j = 0;
            int _len = 1;
            string time_string = "";
            dataGridView3.Columns.Add("VMS", "VMS");
            while ((_temp = ftimezone.read_file_line()) != null)
            {
                //int _index = 32;
                try
                {
                    bool _addj = false;
                    if (j >= 10)
                        _len = 2;

                    string _day = _temp.Substring(6, _len);
                    int i = int.Parse(_day);
                    
                    if (_temp.IndexOf("start") > 0)
                    {
                        time_string = Tools.get_str_value(_temp) + "-";
                        //_addj = true;
                    }
                    else if (_temp.IndexOf("end") > 0)
                    {
                        time_string += Tools.get_str_value(_temp);
                        _addj = true;
                        
                    }
                    if (_addj)
                    {
                        dataGridView3.Columns.Add(time_string, time_string);
                        time_string = "";
                        j++;
                    }
                }
                catch { }
            }
            ftimezone.close();
            //ftimezone = new MyFile("temp");

            dataGridView3.Columns["VMS"].ReadOnly = true;
            dataGridView3.Columns["VMS"].Width = 180;

            for (int i = 0; i < inc._vms.Length; i++)
            {
                if (!check_cms_in_vms(i))
                    continue;

                dataGridView3.Rows.Add();
                dataGridView3.Rows[dataGridView3.Rows.Count - 1].Cells["VMS"].Value = inc._vms[i].name;
            }

            //comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            add_timezone_data();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {           
            string _week = "";
            int index = fn_global.fn_cms_name2index(textBox8.Text);
            if (index >= 0)
            {
                if (comboBox1.SelectedIndex != 6)
                {
                    _week = "." + (comboBox1.SelectedIndex + 1).ToString();
                }
                string _name = "cms." + inc.Carpark[index].id + _week + ".txt";
                //string file_name = fn_global.get_workdir("") + "\\cms" + inc.Carpark[index].id + "\\" + _name;

                string file_name = fn_global.get_workdir("\\cms" + inc.Carpark[index].id) + "\\" + _name;

                MyFile ftimezone = new MyFile();
                try
                {
                    ftimezone.FileName = file_name;
                    
                    //string[] valueofzone = new string[16];

                    for (int i = 0; i < dataGridView3.Rows.Count; i++)
                    {
                        string _vms_name = Tools.get_value(dataGridView3.Rows[i].Cells[0].Value);
                        int _index = fn_global.fn_vms_name2index(_vms_name);
                        string name_head = "gdm" + inc._vms[_index]._id.ToString() + "_";
                        for (int j = 1; j <= 16; j++)
                        {
                            string name = name_head + "zone" + j.ToString();
                            string _offset_value = name + "=" +Tools.get_value(dataGridView3.Rows[i].Cells[j].Value);
                            //Thread.Sleep(20);
                            ftimezone.write_to_file(_offset_value);                            
                        }
                    }
                    ftimezone.write_to_file("file_type=cms");
                    ftimezone.close();


                    if (fn_global.fn_put_file("cms" + inc.Carpark[index].id, _name))
                        MessageBox.Show("Update CMS Timezone Successaful");
                    else
                        MessageBox.Show("Update CMS Timezone Failed");
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                    ftimezone.close();
                }

                //ftimezone = null;
                //ftimezone = new MyFile("temp");
            }            
        }

    }
}

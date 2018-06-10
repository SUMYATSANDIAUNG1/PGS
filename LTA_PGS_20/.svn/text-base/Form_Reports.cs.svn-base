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
    public partial class Form_Reports : Form
    {
        #region
        public int Page_num = 0;
        OdbcDataReader MyReader = null;

        struct VMS_Breakdown_rep
        {
            public TimeSpan COMM_DOWN;
            public TimeSpan SCANBOARD_FAILURE;
            public TimeSpan other;
            public TimeSpan total;
        }

        #endregion

        #region Main Control
        public Form_Reports()
        {
            InitializeComponent();
            init_adhoc_report();
            init_alarm_report();
            init_log_report();
            init_cms_report();
            init_occupany_report();
            init_breakdown_report();
        }

        private void Form_Reports_Load(object sender, EventArgs e)
        {
            tabControl_management.SelectedIndex = Page_num;
            for (int i = 0; i < 6; i++)
                init_report_page(i);

            //tabControl_management.SelectedIndex = Page_num;            
            int _i = tabControl_management.TabPages.Count - 1;
            while (_i >= 0)
            {
                bool _valid = false;
                for (int j = 0; j < inc.pagelist.Count; j++)
                {
                    if (inc.pagelist[j] == tabControl_management.TabPages[_i].Name)
                        _valid = true;
                    ;
                }
                if (!_valid)
                    tabControl_management.TabPages.Remove(tabControl_management.TabPages[_i]);

                _i -= 1;
            }

            //tabControl_management.TabPages[inc.tabpagename].Select();
            for (int a = 0; a < tabControl_management.TabPages.Count; a++)
            {
                string _name = tabControl_management.TabPages[a].Name;
                if (inc.tabpagename == _name)
                {
                    tabControl_management.SelectedIndex = a;
                    break;
                }
            }

            init_combine();
        }

        private void init_report_page(int A_pagenum)
        {
            int _num = A_pagenum + 1;
            try
            {
                ((TextBox)this.Controls.Find("textBox" + _num, true)[0]).Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                ((TextBox)this.Controls.Find("textBox" + _num, true)[0]).ReadOnly = true;
                ((DataGridView)this.Controls.Find("dataGridView" + _num, true)[0]).ReadOnly = true;
                ((DataGridView)this.Controls.Find("dataGridView" + _num, true)[0]).AllowUserToAddRows = false;
                ((DataGridView)this.Controls.Find("dataGridView" + _num, true)[0]).SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch { }
        }

        private void get_report_data()
        {
            MyReader = null;
            string _time_field_name = "";
            string db_name = "";
            int _num = tabControl_management.SelectedIndex + 1;

            string f_time = "";
            string t_time = "";

            f_time = ((DateTimePicker)this.Controls.Find("dateTimePicker" + (_num * 2 - 1), true)[0]).Value.ToString("yyyy-MM-dd") + " 00:00:00";
            t_time = ((DateTimePicker)this.Controls.Find("dateTimePicker" + (_num * 2), true)[0]).Value.ToString("yyyy-MM-dd") + " 23:59:59";

            switch (tabControl_management.SelectedIndex)
            {
                case 0:
                    {
                        db_name = "tb_adhoc";
                        _time_field_name = "fd_addtime";
                        break;
                    }
                case 1:
                    {
                        db_name = "tb_alarmlog";
                        _time_field_name = "fd_time";
                        break;
                    }
                case 2:
                    {
                        db_name = "tb_log";
                        _time_field_name = "fd_time";
                        break;
                    }
                case 3:
                    {
                        break;
                    }
                case 4: { break; }
                case 5: { break; }
                case 6:
                    {
                        db_name = "tb_alarm";
                        _time_field_name = "fd_time";
                        break;
                    }
                default: { break; }
            }

            inc.db_control.SQL = "select * from "
                            + db_name + " where " + _time_field_name + ">='" + f_time + "' and " + _time_field_name + "<='" + t_time
                            + "' order by " + _time_field_name;

            if ((tabControl_management.SelectedIndex == 2) && (comboBox_users.Text != ""))
            {
                inc.db_control.SQL = "select * from "
                            + db_name + " where " + _time_field_name + ">='" + f_time + "' and " + _time_field_name + "<='" + t_time
                            + "' and fd_userid='" + inc.TB_user[fn_global.fn_user_name2index(comboBox_users.Text)].id
                            + "' order by " + _time_field_name;
            }

            MyReader = inc.db_control.SQLExecuteReader();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Private functions
        private string gdm_id2name(int id)
        {
            string name = "";

            int _index = fn_global.fn_vms_id2index(id);
            if (_index >= 0)
                name = inc._vms[_index].name;
            else
                name = id.ToString();

            return name;
        }

        private string mode_index2name(int index)
        {
            string name = "";

            try
            {
                name = inc.VMS_Flash_mode[index].ToString();
            }
            catch
            {
                name = index.ToString();
            }

            return name;
        }

        private string color_index2name(int index)
        {
            index = index >= 128 ? (index - 128) : index;

            string name = "";

            try
            {
                name = inc.VMS_Name_color[index].ToString();
            }
            catch
            {
                name = index.ToString();
            }

            return name;
        }

        private string get_name_by_userid(int id)
        {
            string name = "";
            int index = fn_global.fn_user_id2index(id);
            if (index < 0)
                name = id.ToString();
            else
                name = inc.TB_user[index].name;
            return name;
        }

        private string get_name_by_id(int id)
        {
            string name = "";
            if (id > 10000)
            {
                int _index = fn_global.fn_cms_id2index(id - 10000);
                name = inc.Carpark[_index].name;
            }
            else
            {
                int _index = fn_global.fn_vms_id2index(id);
                if (_index >= 0)
                    name = inc._vms[_index].name;
                else
                    name = id == 99 ? "EA INTERFACE" : "CCS SVR";
            }
            return name;
        }

        private string cms_id2name(int id)
        {
            string name = "";

            int _index = fn_global.fn_cms_id2index(id);
            if (_index >= 0)
                name = inc.Carpark[_index].name;
            else
                name = id.ToString();


            return name;
        }

        #endregion

        #region Adhoc Report
        private void init_adhoc_report()
        {
            dataGridView1.Columns.Add("id", "ID");
            dataGridView1.Columns.Add("gdmid", "VMS");//
            dataGridView1.Columns.Add("name", "Message Name");//
            dataGridView1.Columns.Add("mode", "Mode");//
            dataGridView1.Columns.Add("timer", "Timer(seconds)");//
            dataGridView1.Columns.Add("color", "Color");//
            dataGridView1.Columns.Add("stime", "Start Date & Time");//

            //dataGridView1.Columns.Add("useendtime", "ID");
            //dataGridView1.Columns.Add("endtime", "ID");
            dataGridView1.Columns.Add("user", "User");//
            dataGridView1.Columns.Add("delete", "Activity");//

            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["stime"].Width = 120;
            dataGridView1.Columns["name"].Width = 140;
            dataGridView1.Columns["mode"].Width = 80;
            dataGridView1.Columns["timer"].Width = 80;
            dataGridView1.Columns["color"].Width = 80;
        }
        private void set_enable_printbuttons(bool enablevalues)
        {
            button9.Enabled = enablevalues;
            button10.Enabled = enablevalues;
            button12.Enabled = enablevalues;
            button13.Enabled = enablevalues;
            button14.Enabled = enablevalues;
            button15.Enabled = enablevalues;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            set_enable_printbuttons(false);
            get_report_data();
            dataGridView1.Rows.Clear();
            if (MyReader != null)
            {
                int i = 0;
                while (MyReader.Read())
                {
                    dataGridView1.Rows.Add();

                    dataGridView1.Rows[i].Cells["id"].Value = ((Int64)MyReader["fd_id"]).ToString();
                    dataGridView1.Rows[i].Cells["gdmid"].Value = gdm_id2name(((Int32)MyReader["fd_gdm"]));
                    dataGridView1.Rows[i].Cells["name"].Value = (String)MyReader["fd_name"];
                    dataGridView1.Rows[i].Cells["mode"].Value = mode_index2name(((Int32)MyReader["fd_mode"]));
                    dataGridView1.Rows[i].Cells["timer"].Value = ((Int32)MyReader["fd_timer"]).ToString();
                    dataGridView1.Rows[i].Cells["color"].Value = color_index2name(((Int32)MyReader["fd_color"]));
                    dataGridView1.Rows[i].Cells["stime"].Value = ((DateTime)MyReader["fd_starttime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    dataGridView1.Rows[i].Cells["user"].Value = get_name_by_userid(((Int32)MyReader["fd_user"]));

                    if ((Int32)MyReader["fd_delete"] == 0)
                        dataGridView1.Rows[i].Cells["delete"].Value = "Activity";
                    else
                        dataGridView1.Rows[i].Cells["delete"].Value = "Delete";

                    i++;
                    label26.Text = "Total: " + i.ToString();
                    Application.DoEvents();
                }
            }
            set_enable_printbuttons(true);
        }
        #endregion

        #region alarm report
        private void init_alarm_report()
        {
            dataGridView2.Columns.Add("id", "ID");
            dataGridView2.Columns.Add("gdmid", "Station");//
            dataGridView2.Columns.Add("alarm", "Alarm");//
            dataGridView2.Columns.Add("alarmtime", "Alarm Time");//
            dataGridView2.Columns.Add("uptime", "Up Time");//
            dataGridView2.Columns.Add("ackuser", "Acked User");//
            dataGridView2.Columns.Add("acktime", "Acked Time");//
            dataGridView2.Columns.Add("dowmtime", "Down Duration(Hours)");//


            dataGridView2.Columns["id"].Visible = false;
            dataGridView2.Columns["uptime"].Width = 115;
            dataGridView2.Columns["alarmtime"].Width = 115;
            dataGridView2.Columns["acktime"].Width = 115;
            dataGridView2.Columns["gdmid"].Width = 160;
            dataGridView2.Columns["alarm"].Width = 160;
            dataGridView2.Columns["ackuser"].Width = 90;
            dataGridView2.Columns["gdmid"].Width = 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            set_enable_printbuttons(false);
            get_report_data();
            dataGridView2.Rows.Clear();
            if (MyReader != null)
            {
                int i = 0;
                while (MyReader.Read())
                {
                    dataGridView2.Rows.Add();

                    DateTime alarmtime = DateTime.Now;
                    DateTime uptime = DateTime.Now;
                    int equipid = 0;

                    try { alarmtime = ((DateTime)MyReader["fd_time"]); }
                    catch { }
                    try { uptime = ((DateTime)MyReader["fd_time_ok"]); }
                    catch { }

                    try { dataGridView2.Rows[i].Cells["id"].Value = ((Int32)MyReader["fd_id"]).ToString(); }
                    catch { }
                    try
                    {
                        equipid = ((Int32)MyReader["fd_gdmcms"]);
                        dataGridView2.Rows[i].Cells["gdmid"].Value = get_name_by_id(equipid);
                    }
                    catch { }
                    try
                    {
                        dataGridView2.Rows[i].Cells["alarmtime"].Value = alarmtime.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    catch { }
                    try
                    {
                        int msgid = ((Int32)MyReader["fd_msgid"]);
                        dataGridView2.Rows[i].Cells["alarm"].Value = fn_global.fn_getmsgbyid(msgid, equipid);
                    }
                    catch { }
                    try
                    {
                        if (alarmtime.Year > 2000)
                            dataGridView2.Rows[i].Cells["uptime"].Value = uptime.ToString("yyyy-MM-dd HH:mm:ss");
                        else
                            dataGridView2.Rows[i].Cells["uptime"].Value = "";
                    }
                    catch { }

                    string _ack = get_name_by_userid((int)((Int64)MyReader["fd_userid"]));
                    try { dataGridView2.Rows[i].Cells["ackuser"].Value = _ack == "0" ? "" : _ack; }
                    catch { }

                    try { dataGridView2.Rows[i].Cells["dowmtime"].Value = ((uptime - alarmtime).Days * 24 + (uptime - alarmtime).Hours).ToString(); }
                    catch { }
                    try
                    {
                        System.DateTime _at = (System.DateTime)MyReader["fd_time_ack"];
                        if (_at.Year > 2000)
                            dataGridView2.Rows[i].Cells["acktime"].Value = _at.ToString("yyyy-MM-dd HH:mm:ss");
                        else
                            dataGridView2.Rows[i].Cells["acktime"].Value = "";
                    }
                    catch { dataGridView2.Rows[i].Cells["acktime"].Value = ""; }

                    i++;

                    label27.Text = "Total: " + i.ToString();
                    Application.DoEvents();
                }
            }
            set_enable_printbuttons(true);
        }
        #endregion

        #region log report
        private void init_log_report()
        {
            dataGridView3.Columns.Add("id", "ID");
            dataGridView3.Columns.Add("time", "Time");//
            dataGridView3.Columns.Add("user", "User");//
            dataGridView3.Columns.Add("activity", "Activity");//


            dataGridView3.Columns["id"].Visible = false;
            dataGridView3.Columns["time"].Width = 140;
            dataGridView3.Columns["user"].Width = 120;
            dataGridView3.Columns["activity"].Width = 220;

            for (int i = 0; i < inc.TB_user.Length; i++)
            {
                comboBox_users.Items.Add(inc.TB_user[i].name);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            set_enable_printbuttons(false);
            dataGridView3.Rows.Clear();
            get_report_data();

            if (MyReader != null)
            {
                int i = 0;

                while (MyReader.Read())
                {
                    dataGridView3.Rows.Add();

                    dataGridView3.Rows[i].Cells["id"].Value = ((Int32)MyReader["fd_id"]).ToString();
                    dataGridView3.Rows[i].Cells["time"].Value = ((DateTime)MyReader["fd_time"]).ToString("yyyy-MM-dd HH:mm:ss");
                    dataGridView3.Rows[i].Cells["activity"].Value = ((String)MyReader["fd_msg"]).ToString();
                    dataGridView3.Rows[i].Cells["user"].Value = get_name_by_userid(((Int32)MyReader["fd_userid"]));

                    i++;

                    label28.Text = "Total: " + i.ToString();
                    Application.DoEvents();
                }
            }
            set_enable_printbuttons(true);
        }
        #endregion

        #region cms report
        private void init_cms_report()
        {
            dataGridView4.Columns.Add("time", "Time");//
            dataGridView4.Columns.Add("name", "CMS Name");//
            dataGridView4.Columns.Add("lots", "Available Lots");//
            dataGridView4.Columns.Add("limit", "Lots Limit");//

            dataGridView4.Columns["time"].Width = 220;
            dataGridView4.Columns["name"].Width = 160;
            dataGridView4.Columns["lots"].Width = 200;
            dataGridView4.Columns["limit"].Width = 200;

            comboBox1.Items.AddRange(inc.carparkobject);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            set_enable_printbuttons(false);
            dataGridView4.Rows.Clear();
            if (comboBox1.SelectedIndex >= 0)
            {
                string _path = "cms" + inc.Carpark[comboBox1.SelectedIndex].id.ToString();
                string _name = dateTimePicker8.Value.ToString("yyyyMMdd") + ".trans";
                string _cms_name = cms_id2name(inc.Carpark[comboBox1.SelectedIndex].id);
                string _data = "";

                dataGridView4.Rows.Clear();

                if (fn_global.fn_get_file(_path, _name, _name))
                {
                    MyFile f1 = new MyFile(fn_global.get_workdir(_path) + "\\" + _name);
                    int i = 0;
                    bool doevent = false;
                    while ((_data = f1.read_file_line()) != null)
                    {
                        string[] s_data = _data.Split('|');
                        dataGridView4.Rows.Add();

                        dataGridView4.Rows[i].Cells["time"].Value = s_data[1];
                        dataGridView4.Rows[i].Cells["name"].Value = _cms_name;
                        dataGridView4.Rows[i].Cells["lots"].Value = s_data[2];
                        dataGridView4.Rows[i].Cells["limit"].Value = s_data[3];
                        i++;

                        label25.Text = "Total: " + i.ToString();
                        Application.DoEvents();

                    }
                    f1.close();
                }
            }
            set_enable_printbuttons(true);
        }
        #endregion

        #region Occupancy report
        private void init_occupany_report()
        {
            dataGridView5.Columns.Add("time", "Time");//
            dataGridView5.Columns.Add("user", "CMS");//
            dataGridView5.Columns.Add("occupancy", "Occupancy");//
            dataGridView5.Columns.Add("lots", "Available Lots");//
            dataGridView5.Columns.Add("limit", "Lots Limit");//

            dataGridView5.Columns["time"].Width = 200;
            dataGridView5.Columns["user"].Width = 160;
            dataGridView5.Columns["occupancy"].Width = 160;
            dataGridView5.Columns["lots"].Width = 160;
            dataGridView5.Columns["limit"].Width = 160;

            comboBox2.Items.AddRange(inc.carparkobject);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            set_enable_printbuttons(false);
            dataGridView5.Rows.Clear();

            if (comboBox2.SelectedIndex >= 0)
            {
                string _path = "cms" + inc.Carpark[comboBox2.SelectedIndex].id.ToString();
                string _name = dateTimePicker9.Value.ToString("yyyyMMdd") + ".trans";
                string _cms_name = cms_id2name(inc.Carpark[comboBox2.SelectedIndex].id);
                string _data = "";

                dataGridView5.Rows.Clear();

                if (fn_global.fn_get_file(_path, _name, _name))
                {
                    MyFile f1 = new MyFile(fn_global.get_workdir(_path) + "\\" + _name);
                    int i = 0;

                    while ((_data = f1.read_file_line()) != null)
                    {
                        string[] s_data = _data.Split('|');
                        dataGridView5.Rows.Add();

                        dataGridView5.Rows[i].Cells["time"].Value = s_data[1];
                        dataGridView5.Rows[i].Cells["user"].Value = _cms_name;
                        dataGridView5.Rows[i].Cells["occupancy"].Value = (int)(double.Parse(s_data[4]) / 10) + "%";
                        dataGridView5.Rows[i].Cells["lots"].Value = s_data[2];
                        dataGridView5.Rows[i].Cells["limit"].Value = s_data[3];
                        i++;

                        label29.Text = "Total: " + i.ToString();
                        Application.DoEvents();
                    }
                    f1.close();
                }
            }
            set_enable_printbuttons(true);
        }
        #endregion

        #region Monthly Breakdown Duration report
        private void init_breakdown_report()
        {
            dataGridView6.Columns.Add("gdmid", "VMS");
            dataGridView6.Columns.Add("commdown", "COMM DOWN (hh:mm)");//
            dataGridView6.Columns.Add("scanboard", "SCANBOARD FAILURE (hh:mm)");//
            dataGridView6.Columns.Add("other", "OTHERS DOWNTIME (hh:mm)");//
            dataGridView6.Columns.Add("total", "SUBTOTAL (Hours)");//


            dataGridView6.Columns["gdmid"].Width = 120;
            dataGridView6.Columns["commdown"].Width = 150;
            dataGridView6.Columns["scanboard"].Width = 190;
            dataGridView6.Columns["other"].Width = 190;
            dataGridView6.Columns["total"].Width = 150;

            dataGridView6.ReadOnly = true;
            dataGridView6.AllowUserToAddRows = false;
            dataGridView6.AllowUserToDeleteRows = false;

            dataGridView7.Columns.Add("cmsid", "CMS");
            dataGridView7.Columns.Add("commdown", "CMS DOWN (hh:mm)");//
            dataGridView7.Columns.Add("ldcdown", "LDC DOWN (hh:mm)");//
            dataGridView7.Columns.Add("total", "SUBTOTAL (Hours)");//

            dataGridView7.Columns["cmsid"].Width = 120;
            dataGridView7.Columns["commdown"].Width = 160;
            dataGridView7.Columns["ldcdown"].Width = 160;
            dataGridView7.Columns["total"].Width = 160;

            dataGridView7.ReadOnly = true;
            dataGridView7.AllowUserToAddRows = false;
            dataGridView7.AllowUserToDeleteRows = false;
            dataGridView7.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private DateTime getLastDate(DateTime dtTmp)//返回指定日期的当月最后一天  
        {
            if (dtTmp.Month == 12) //12月份按后面处理会出错，所以单独处理  
            {
                DateTime dtReturn1 = new System.DateTime(dtTmp.Year, 12, 31, 0, 0, 0, 0);
                return dtReturn1;
            }
            DateTime dtReturn = new System.DateTime(dtTmp.Year, dtTmp.Month + 1, 1, 0, 0, 0, 0);
            dtReturn = dtReturn.AddDays(-1);
            return dtReturn;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            set_enable_printbuttons(false);
            string _month = dateTimePicker11.Value.ToString("yyyy-MM");
            dataGridView6.Rows.Clear();
            dataGridView7.Rows.Clear();

            VMS_Breakdown_rep[] _vms_breakdown_rep = new VMS_Breakdown_rep[inc._vms.Length];
            for (int i = 0; i < inc._vms.Length; i++)
            {
                _vms_breakdown_rep[i].SCANBOARD_FAILURE = new TimeSpan(0);
                _vms_breakdown_rep[i].COMM_DOWN = new TimeSpan(0);
                _vms_breakdown_rep[i].other = new TimeSpan(0);
                _vms_breakdown_rep[i].total = new TimeSpan(0);

                inc.db_control.SQL = "select * from tb_alarm where fd_delete<>1 and fd_gdmcms='" + inc._vms[i]._id + "' and fd_time like '" + _month + "%' order by fd_msgid";
                OdbcDataReader MyReader1 = inc.db_control.SQLExecuteReader();
                if (MyReader1 != null)
                {
                    while (MyReader1.Read())
                    {
                        int msgid = (Int32)MyReader1["fd_msgid"];
                        DateTime t1 = (DateTime)MyReader1["fd_time"];
                        DateTime t2 = getLastDate(dateTimePicker11.Value);
                        try
                        {
                            t2 = (DateTime)MyReader1["fd_time_ok"];
                        }
                        catch { }
                        TimeSpan _timespan = t2 - t1;

                        _vms_breakdown_rep[i].total += _timespan;

                        if (msgid == 5)
                        {
                            _vms_breakdown_rep[i].SCANBOARD_FAILURE += _timespan;
                        }
                        else if (msgid == 2)
                        {
                            _vms_breakdown_rep[i].COMM_DOWN += _timespan;
                        }
                        else
                        {
                            _vms_breakdown_rep[i].other += _timespan;
                        }
                    }
                }

                dataGridView6.Rows.Add();
                dataGridView6.Rows[i].Cells["gdmid"].Value = inc._vms[i].name;
                int _m = _vms_breakdown_rep[i].COMM_DOWN.Minutes;
                int _h = _vms_breakdown_rep[i].COMM_DOWN.Days * 24 + _vms_breakdown_rep[i].COMM_DOWN.Hours;

                dataGridView6.Rows[i].Cells["commdown"].Value = _h.ToString() + ":" + _m.ToString();
                _m = _vms_breakdown_rep[i].SCANBOARD_FAILURE.Minutes;
                _h = _vms_breakdown_rep[i].SCANBOARD_FAILURE.Days * 24 + _vms_breakdown_rep[i].SCANBOARD_FAILURE.Hours;
                dataGridView6.Rows[i].Cells["scanboard"].Value = _h.ToString() + ":" + _m.ToString();
                _m = _vms_breakdown_rep[i].other.Minutes;
                _h = _vms_breakdown_rep[i].other.Days * 24 + _vms_breakdown_rep[i].other.Hours;
                dataGridView6.Rows[i].Cells["other"].Value = _h.ToString() + ":" + _m.ToString();
                _m = _vms_breakdown_rep[i].total.Minutes;
                _h = _vms_breakdown_rep[i].total.Days * 24 + _vms_breakdown_rep[i].total.Hours;
                dataGridView6.Rows[i].Cells["total"].Value = _h.ToString() + ":" + _m.ToString();

                label31.Text = "Total: " + (i + 1).ToString();
                Application.DoEvents();

            }

            VMS_Breakdown_rep[] _cms_breakdown_rep = new VMS_Breakdown_rep[inc.Carpark.Length];
            for (int i = 0; i < inc.Carpark.Length; i++)
            {
                _cms_breakdown_rep[i].SCANBOARD_FAILURE = new TimeSpan(0);
                _cms_breakdown_rep[i].COMM_DOWN = new TimeSpan(0);
                _cms_breakdown_rep[i].total = new TimeSpan(0);

                inc.db_control.SQL = "select * from tb_alarm where fd_delete<>1 and fd_gdmcms='" + (inc.Carpark[i].id + 10000).ToString() + "' and fd_time like '" + _month + "%' order by fd_msgid";
                OdbcDataReader MyReader1 = inc.db_control.SQLExecuteReader();
                if (MyReader1 != null)
                {
                    while (MyReader1.Read())
                    {
                        int msgid = (Int32)MyReader1["fd_msgid"];
                        DateTime t1 = (DateTime)MyReader1["fd_time"];
                        DateTime t2 = getLastDate(dateTimePicker11.Value);
                        try
                        {
                            t2 = (DateTime)MyReader1["fd_time_ok"];
                        }
                        catch { }
                        TimeSpan _timespan = t2 - t1;

                        _cms_breakdown_rep[i].total += _timespan;

                        if (msgid == 11)
                        {
                            _cms_breakdown_rep[i].SCANBOARD_FAILURE += _timespan;//LDC
                        }
                        else if (msgid == 10)
                        {
                            _cms_breakdown_rep[i].COMM_DOWN += _timespan;
                        }
                        else
                        {
                            _cms_breakdown_rep[i].other += _timespan;
                        }
                    }
                }

                dataGridView7.Rows.Add();
                dataGridView7.Rows[i].Cells["cmsid"].Value = inc.Carpark[i].name;
                int _m = _cms_breakdown_rep[i].COMM_DOWN.Minutes;
                int _h = _cms_breakdown_rep[i].COMM_DOWN.Days * 24 + _cms_breakdown_rep[i].COMM_DOWN.Hours;
                dataGridView7.Rows[i].Cells["commdown"].Value = _h.ToString() + ":" + _m.ToString();
                _m = _cms_breakdown_rep[i].SCANBOARD_FAILURE.Minutes;
                _h = _cms_breakdown_rep[i].SCANBOARD_FAILURE.Days * 24 + _cms_breakdown_rep[i].SCANBOARD_FAILURE.Hours;
                dataGridView7.Rows[i].Cells["ldcdown"].Value = _h.ToString() + ":" + _m.ToString();
                _m = _cms_breakdown_rep[i].total.Minutes;
                _h = _cms_breakdown_rep[i].total.Days * 24 + _cms_breakdown_rep[i].total.Hours;
                dataGridView7.Rows[i].Cells["total"].Value = _h.ToString() + ":" + _m.ToString();

                label30.Text = "Total: " + (i + 1).ToString();
                Application.DoEvents();
            }
            set_enable_printbuttons(true);
        }
        #endregion

        #region Print and Save
        private string get_report_date()
        {
            string r = "";
            if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_adhoc_rep")
            {
                r = dateTimePicker1.Value.ToString("dd/MM/yyyy") + " to " + dateTimePicker2.Value.ToString("dd/MM/yyyy");
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_alarm_rep")
            {
                r = dateTimePicker3.Value.ToString("dd/MM/yyyy") + " to " + dateTimePicker4.Value.ToString("dd/MM/yyyy");
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_log_rep")
            {
                r = dateTimePicker5.Value.ToString("dd/MM/yyyy") + " to " + dateTimePicker6.Value.ToString("dd/MM/yyyy");
                r += "\r\n" + "User: " + (comboBox_users.Text == "" ? "All" : comboBox_users.Text);
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_cms_daily_rep")
            {
                r = dateTimePicker8.Value.ToString("dd/MM/yyyy");
                r += "\r\n" + "CMS: " + comboBox1.Text;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_cms_occ_rep")
            {
                r = dateTimePicker9.Value.ToString("dd/MM/yyyy");
                r += "\r\n" + "CMS: " + comboBox2.Text;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_combine")
            {
                r = dateTimePicker13.Value.ToString("dd/MM/yyyy");
                r += "\r\n" + "Area: " + comboBox_area.Text;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_vms_month_rep")
            {
                r = dateTimePicker11.Value.ToString("MM/yyyy");
            }
            return r;
        }

        private string get_report_dataviewname()
        {
            string r = null;
            if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_adhoc_rep")
            {
                r = dataGridView1.Name;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_alarm_rep")
            {
                r = dataGridView2.Name;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_log_rep")
            {
                r = dataGridView3.Name;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_cms_daily_rep")
            {
                r = dataGridView4.Name;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_cms_occ_rep")
            {
                r = dataGridView5.Name;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_combine")
            {
                r = dataGridView8.Name;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_vms_month_rep")
            {
                r = dataGridView7.Name;
            }
            return r;
        }

        private void save_report(string name)
        {
            MyFile mf = new MyFile(name);
            mf.addstr_to_file(tabControl_management.TabPages[tabControl_management.SelectedIndex].Text);
            mf.addstr_to_file("Report time: " + get_report_date());
            mf.addstr_to_file("Time: " + DateTime.Now.ToString("dd ") + inc.Month_Display[DateTime.Now.Month] + DateTime.Now.ToString(" yyyy"));
            string str = "";
            string _datagridname = get_report_dataviewname();

            for (int i = 0; i < ((DataGridView)this.Controls.Find(_datagridname, true)[0]).Columns.Count; i++)
            {
                if (((DataGridView)this.Controls.Find(_datagridname, true)[0]).Columns[i].Visible)
                    str += ((DataGridView)this.Controls.Find(_datagridname, true)[0]).Columns[i].HeaderText + ",";
            }
            mf.addstr_to_file(str);
            for (int i = 0; i < ((DataGridView)this.Controls.Find(_datagridname, true)[0]).Rows.Count; i++)
            {
                str = "";
                for (int j = 0; j < ((DataGridView)this.Controls.Find(_datagridname, true)[0]).Columns.Count; j++)
                {
                    if (((DataGridView)this.Controls.Find(_datagridname, true)[0]).Columns[j].Visible)
                        str += Tools.get_value(((DataGridView)this.Controls.Find(_datagridname, true)[0]).Rows[i].Cells[j].Value) + ",";
                }
                mf.addstr_to_file(str);
                Application.DoEvents();
            }
            mf.close();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfdSaveOnlineText = new SaveFileDialog();
                sfdSaveOnlineText.Filter = "CSV files (*.csv)|*.csv";
                sfdSaveOnlineText.FilterIndex = 0;
                sfdSaveOnlineText.RestoreDirectory = true;
                sfdSaveOnlineText.CreatePrompt = true;
                sfdSaveOnlineText.Title = "Save File To";

                sfdSaveOnlineText.FileName = tabControl_management.TabPages[tabControl_management.SelectedIndex].Text;

                if (sfdSaveOnlineText.ShowDialog() == DialogResult.OK)
                {
                    string name = sfdSaveOnlineText.FileName;
                    save_report(name);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Save File Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string str = "Report time: ";
            string r = "";
            if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_adhoc_rep")
            {
                r = dateTimePicker1.Value.ToString("dd/MM/yyyy") + " to " + dateTimePicker2.Value.ToString("dd/MM/yyyy");
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_alarm_rep")
            {
                r = dateTimePicker3.Value.ToString("dd/MM/yyyy") + " to " + dateTimePicker4.Value.ToString("dd/MM/yyyy");
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_log_rep")
            {
                r = dateTimePicker5.Value.ToString("dd/MM/yyyy") + " to " + dateTimePicker6.Value.ToString("dd/MM/yyyy");
                r += "\r\n" + "User: " + (comboBox_users.Text == "" ? "All" : comboBox_users.Text);
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_cms_daily_rep")
            {
                r = dateTimePicker8.Value.ToString("dd/MM/yyyy");
                r += "\r\n" + "CMS: " + comboBox1.Text;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_cms_occ_rep")
            {
                r = dateTimePicker9.Value.ToString("dd/MM/yyyy");
                r += "\r\n" + "CMS: " + comboBox2.Text;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_combine")
            {
                r = dateTimePicker13.Value.ToString("dd/MM/yyyy");
                r += "\r\n" + "Area: " + comboBox_area.Text;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_vms_month_rep")
            {
                r = dateTimePicker11.Value.ToString("MM/yyyy");
            }
            str += r;
            string _datagridname = get_report_dataviewname();
            PrintDataGrid.Print_DataGridView((DataGridView)this.Controls.Find(_datagridname, true)[0], str
                , tabControl_management.TabPages[tabControl_management.SelectedIndex].Text);
        }
        private void button12_Click(object sender, EventArgs e)
        {
            string str = "Report time: ";
            string r = "";
            if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_adhoc_rep")
            {
                r = dateTimePicker1.Value.ToString("dd/MM/yyyy") + " to " + dateTimePicker2.Value.ToString("dd/MM/yyyy");
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_alarm_rep")
            {
                r = dateTimePicker3.Value.ToString("dd/MM/yyyy") + " to " + dateTimePicker4.Value.ToString("dd/MM/yyyy");
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_log_rep")
            {
                r = dateTimePicker5.Value.ToString("dd/MM/yyyy") + " to " + dateTimePicker6.Value.ToString("dd/MM/yyyy");
                r += "\r\n" + "User: " + (comboBox_users.Text == "" ? "All" : comboBox_users.Text);
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_cms_daily_rep")
            {
                r = dateTimePicker8.Value.ToString("dd/MM/yyyy");
                r += "\r\n" + "CMS: " + comboBox1.Text;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_cms_occ_rep")
            {
                r = dateTimePicker9.Value.ToString("dd/MM/yyyy");
                r += "\r\n" + "CMS: " + comboBox2.Text;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_combine")
            {
                r = dateTimePicker13.Value.ToString("dd/MM/yyyy");
                r += "\r\n" + "Area: " + comboBox_area.Text;
            }
            else if (tabControl_management.TabPages[tabControl_management.SelectedIndex].Name == "tabPage_vms_month_rep")
            {
                r = dateTimePicker11.Value.ToString("MM/yyyy");
            }
            str += r;
            string _datagridname = get_report_dataviewname();
            PrintDataGrid.Print_dataGridView_preview((DataGridView)this.Controls.Find(_datagridname, true)[0], str
                , tabControl_management.TabPages[tabControl_management.SelectedIndex].Text);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfdSaveOnlineText = new SaveFileDialog();
                sfdSaveOnlineText.Filter = "CSV files (*.csv)|*.csv";
                sfdSaveOnlineText.FilterIndex = 0;
                sfdSaveOnlineText.RestoreDirectory = true;
                sfdSaveOnlineText.CreatePrompt = true;
                sfdSaveOnlineText.Title = "Save File To";

                if (sfdSaveOnlineText.ShowDialog() == DialogResult.OK)
                {
                    string name = sfdSaveOnlineText.FileName;

                    MyFile mf = new MyFile(name);
                    mf.addstr_to_file(tabControl_management.TabPages[tabControl_management.SelectedIndex].Text);
                    mf.addstr_to_file("Report time: " + get_report_date());
                    mf.addstr_to_file("Time: " + DateTime.Now.ToString("dd ") + inc.Month_Display[DateTime.Now.Month] + DateTime.Now.ToString(" yyyy"));
                    string str = "";
                    string _datagridname = get_report_dataviewname();

                    mf.addstr_to_file("\r\n");
                    str = "";
                    _datagridname = dataGridView6.Name;
                    for (int i = 0; i < ((DataGridView)this.Controls.Find(_datagridname, true)[0]).Columns.Count; i++)
                    {
                        if (((DataGridView)this.Controls.Find(_datagridname, true)[0]).Columns[i].Visible)
                            str += ((DataGridView)this.Controls.Find(_datagridname, true)[0]).Columns[i].HeaderText + ",";
                    }
                    mf.addstr_to_file(str);
                    for (int i = 0; i < ((DataGridView)this.Controls.Find(_datagridname, true)[0]).Rows.Count; i++)
                    {
                        str = "";
                        for (int j = 0; j < ((DataGridView)this.Controls.Find(_datagridname, true)[0]).Columns.Count; j++)
                        {
                            if (((DataGridView)this.Controls.Find(_datagridname, true)[0]).Columns[j].Visible)
                                str += Tools.get_value(((DataGridView)this.Controls.Find(_datagridname, true)[0]).Rows[i].Cells[j].Value) + ",";
                        }
                        mf.addstr_to_file(str);
                        Application.DoEvents();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Save File Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string _datagridname = get_report_dataviewname();
            PrintDataGrid.Print_DataGridView(dataGridView6, ""
                , tabControl_management.TabPages[tabControl_management.SelectedIndex].Text);
        }
        private void button13_Click(object sender, EventArgs e)
        {
            string _datagridname = get_report_dataviewname();
            PrintDataGrid.Print_dataGridView_preview(dataGridView6, ""
                , tabControl_management.TabPages[tabControl_management.SelectedIndex].Text);
        }
        #endregion

        #region combine cms
        private void init_combine()
        {
            comboBox_area.Items.AddRange(inc.Area);
            comboBox_area.Items.Add("");
            dataGridView8.Rows.Clear();
            dataGridView8.Columns.Clear();

            dataGridView8.Columns.Add("time", "time");
            dataGridView8.AllowUserToAddRows = false;

            radioButton1.Checked = true;
            //dataGridView8.RowHeadersWidth = 160;

        }
        private DateTime get_time(int i)
        {
            TimeSpan tp = new TimeSpan(0, (5 * i), 0);

            return dateTimePicker13.Value.Date + tp;
        }
        private void add_columns(int i)
        {
            dataGridView8.Columns.Add(i.ToString(), inc.Carpark[i].name);
            dataGridView8.Columns[i.ToString()].Width = 120;
            set_value(i);
        }
        private void refrash_combine()
        {
            dataGridView8.Rows.Clear();
            dataGridView8.Columns.Clear();
            dataGridView8.Columns.Add("time", "time");
            //for (int i = dataGridView8.Columns.Count; i > 1;i-- )
            //    dataGridView8.Columns.RemoveAt(i - 1);

            for (int i = 0; i < 288; i++)
            {
                dataGridView8.Rows.Add();
                dataGridView8.Rows[i].Cells["time"].Value = (get_time(dataGridView8.Rows.Count - 1).ToString("yyyy-MM-dd HH:mm:ss"));

                Application.DoEvents();
            }

            int index = fn_global.fn_area_name2index(comboBox_area.Text);
            if (index >= 0)
            {
                int areaid = inc.tb_area[index].areaid;

                for (int i = 0; i < inc.Carpark.Length; i++)
                {
                    if (inc.Carpark[i].area == areaid)
                    {

                        add_columns(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < inc.Carpark.Length; i++)
                {
                    add_columns(i);
                }
            }
        }

        private void set_value(int index)
        {
            string _path = "cms" + inc.Carpark[index].id.ToString();
            string _name = dateTimePicker13.Value.ToString("yyyyMMdd") + ".trans";
            string _cms_name = inc.Carpark[index].name;
            string _data = "";

            if (fn_global.fn_get_file(_path, _name, _name))
            {
                MyFile f1 = new MyFile(fn_global.get_workdir(_path) + "\\" + _name);
                int i = 0;
                while ((_data = f1.read_file_line()) != null)
                {
                    string[] s_data = _data.Split('|');

                    DateTime line_time = Tools.str_to_datetime(s_data[1], "yyyy-MM-dd HH:mm:ss");
                    DateTime cur_line_time = get_time(dataGridView8.Rows.Count - 1);

                    int mmm = line_time.Hour * 12 + line_time.Minute / 5;

                    if (radioButton1.Checked)
                        dataGridView8.Rows[mmm].Cells[index.ToString()].Value = s_data[2];
                    else
                    {
                        //dataGridView5.Rows[i].Cells["occupancy"].Value = (int)(double.Parse(s_data[4]) / 10) + "%";
                        dataGridView8.Rows[mmm].Cells[index.ToString()].Value = (int)(double.Parse(s_data[4]) / 10) + "%";
                    }

                    i++;
                    Application.DoEvents();
                }
                f1.close();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            set_enable_printbuttons(false);
            textBox7.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            refrash_combine();
            set_enable_printbuttons(true);
        }
        #endregion





    }
}

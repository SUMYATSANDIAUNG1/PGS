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
    public partial class Form_Adhoc_condig : Form
    {
        public bool load_from_main = false;
        public int gdm_index = -1;
        TreeView treeView_vms = new TreeView();
        TreeView treeView_vms_all = new TreeView();
        TreeView treeView_msg = new TreeView();
        bool check_pages1_change = false;
        bool clear_msg = false;

        private DateTime adhoc_start = new DateTime();
        private DateTime adhoc_end = new DateTime();
        private bool use_time_c = false;

        //private bool add_new_vms2group = false;
        string current_combobox = "";

        //2009-7-20
        bool new_adhoc = false;
        public Form_Adhoc_condig()
        {
            InitializeComponent();            
        }

        private void send_cmd(string cmd, string path, string file, string formname, string title)
        {
            ;

        }
        private void update_site()
        {
            //inc.tcp_client.SendBuffer("<update_adhoc><gdm=" + id.ToString() + ">");
            string _name = "Update Adhoc Config ";
            string _title = "Update Adhoc Config";
            string _path = "";
            string _file = "update.txt";
            string _cmd = "";

            string _str = "";
            this.Cursor = Cursors.WaitCursor;

            if (System.Configuration.ConfigurationSettings.AppSettings["tcpdemo"] != "1")                
                fn_global.fn_file_delete(_path, _file);                
            _cmd = "<update_adhoc>";
            if (fn_global.fn_tcp_send(_cmd))
            {
                if (fn_global.fn_cmd_check_finish(_path, _file, ref _str))
                {                    
                    fn_global.fn_return_status(_str, _name, _title, 1);
                }

            }

            this.Cursor = Cursors.Default;
        }
        private void radioButton_i_cmd_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_i_cmd.Checked == true)
            {
                listBox_adhoc_group.Enabled = false;
                button10.Enabled = false;
                button9.Enabled = false;
                button48.Enabled = false;
                listBox_adhoc_group.SelectedIndex = -1;
                button8.Enabled = true;
                //comboBox_adhoc_vmss.Items.Count; SelectedIndex = 0;
            }
            else
            {
                listBox_adhoc_group.Enabled = true;
                button10.Enabled = true;
                button9.Enabled = true;
                button48.Enabled = true;
                comboBox_adhoc_vmss.SelectedIndex = 0;
                button8.Enabled = false;
                //listBox_adhoc_group.SelectedIndex = -1;
            }
        }
        private void double_click_vmstree(object sender, TreeNodeMouseClickEventArgs e)
        {
            string parent_text = "", node_text = "";
            fn_global.Auto_close_treeview((TreeView)sender, ref parent_text, ref node_text);
            comboBox_adhoc_vmss.Text = node_text;
        }

        private void double_click_msgtree(object sender, TreeNodeMouseClickEventArgs e)
        {
            string parent_text = "", node_text = "";
            fn_global.Auto_close_treeview((TreeView)sender, ref parent_text, ref node_text);
            _set_adhoc_msg(node_text, current_combobox);
        }

        private void double_click_vmsall(object sender, TreeNodeMouseClickEventArgs e)
        {
            string parent_text = "", node_text = "";
            fn_global.Auto_close_treeview((TreeView)sender, ref parent_text, ref node_text);
            comboBox_VMSs.Text = node_text;
        }
        
        private void click_msg_combobox(object sender, EventArgs e)
        {
            fn_global.show_treeview_location(treeView_msg, ((ComboBox)sender).Left, ((ComboBox)sender).Top + ((ComboBox)sender).Height);
            current_combobox = ((ComboBox)sender).Name;
        }

        private void click_color_combobox(object sender, EventArgs e)
        {
            int _index = int.Parse(((ComboBox)sender).Name.Substring(8)) - 18;

            if (((ComboBox)sender).SelectedIndex == 0)
                ((ComboBox)this.Controls.Find("comboBox" + _index, true)[0]).ForeColor = Color.Green;
            else if (((ComboBox)sender).SelectedIndex == 1)
                ((ComboBox)this.Controls.Find("comboBox" + _index, true)[0]).ForeColor = inc.Amber;
            else
                ((ComboBox)this.Controls.Find("comboBox" + _index, true)[0]).ForeColor = Color.Red;

        }

        private void check_changed_checkbox(object sender, EventArgs e)
        {
            if (check_pages1_change || clear_msg)
            {
                
            }
            else
            {
                int _index = int.Parse(((CheckBox)sender).Name.Substring(8)) + 6;
                ((CheckBox)this.Controls.Find("checkBox" + _index, true)[0]).Checked = ((CheckBox)sender).Checked;
                _index += 6;
                ((CheckBox)this.Controls.Find("checkBox" + _index, true)[0]).Checked = ((CheckBox)sender).Checked;
            }
        }

        private void mouse_leave(object sender, EventArgs e)
        {
            ((TreeView)sender).Visible = false;
        }
        private void Form_Adhoc_condig_Load(object sender, EventArgs e)
        {
            #region modify at 2009-07-20 adhoc group disign changed
            //radioButton_i_cmd.Checked = true;            
            #endregion
            comboBox_adhoc_vmss.Items.AddRange(inc.VMS);
            comboBox38.Items.AddRange(inc.VMS_Name_color);
            comboBox_adhoc_mode.Items.AddRange(inc.VMS_Flash_mode);
            comboBox_VMSs.Items.AddRange(inc.VMS);

            for (int i = 1; i <= 18; i++)
            {
                ((ComboBox)this.Controls.Find("comboBox" + i, true)[0]).DropDownHeight = 1;
                ((ComboBox)this.Controls.Find("comboBox" + i, true)[0]).DropDown += new EventHandler(click_msg_combobox);
            }

            for (int i = 19; i <= 36; i++)
            {
                ((ComboBox)this.Controls.Find("comboBox" + i, true)[0]).Items.AddRange(inc.VMS_Name_color);
                ((ComboBox)this.Controls.Find("comboBox" + i, true)[0]).SelectedIndexChanged += new EventHandler(click_color_combobox);
            }

            for (int i = 1; i <= 6; i++)
            {
                ((CheckBox)this.Controls.Find("checkBox" + i, true)[0]).CheckedChanged += new EventHandler(check_changed_checkbox);
            }
            
            clear_messge_groupbox();
            load_adhoc_group();

            if (load_from_main)
            {
                comboBox_VMSs.Text = inc._vms[gdm_index].name;

                bool find = false;
                try
                {
                    for (int i = 0; i < inc.Adhoc_Group.Length; i++)
                    {
                        for (int j = 0; j < inc.Adhoc_Group[i].gdm_list.Count; j++)
                        {
                            if (inc._vms[gdm_index]._id.ToString() == inc.Adhoc_Group[i].gdm_list[j])
                            {
                                comboBox_adhoc.Text = inc.Adhoc_Group[i].name;
                                find = true;
                                break;
                            }
                        }
                    }
                }
                catch { }
                
                fn_global.load_vms_treeview(treeView_vms, fn_global.fn_area_id2index(inc._vms[gdm_index].area));

                if(!find)
                {
                    if(gdm_index>=0)
                    {
                        string adhoc_name = inc._vms[gdm_index].name + DateTime.Now.ToString("yyyyMMddHHmmss");
                        comboBox_adhoc.Items.Add(adhoc_name);
                        new_adhoc = true;
                        comboBox_adhoc.SelectedIndex = comboBox_adhoc.Items.Count - 1; 
                        new_adhoc = false;

                        TreeNode rootNode = Get_Tree_mode(treeView1.Nodes, inc._vms[gdm_index].name);
                        try 
                        { 
                            treeView1.SelectedNode = rootNode;
                        }
                        catch { }

                        add_vms2group();
                    }
                }
            }
            else
                fn_global.load_vms_treeview(treeView_vms, 0);

            

            treeView_vms.Visible = false;
            treeView_vms.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(double_click_vmstree);
            treeView_vms.LostFocus += new EventHandler(mouse_leave);

            treeView_msg.Visible = false;
            treeView_msg.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(double_click_msgtree);
            treeView_msg.LostFocus += new EventHandler(mouse_leave);

            treeView_vms_all.Visible = false;
            treeView_vms_all.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(double_click_vmsall);
            treeView_vms_all.LostFocus += new EventHandler(mouse_leave);

            groupBox68.Controls.Add(treeView_vms);
            groupBox74.Controls.Add(treeView_msg);
            //groupBox78.Controls.Add(treeView_vms_all);
            this.Controls.Add(treeView_vms_all);
            comboBox_flash_mode.Items.AddRange(inc.VMS_Flash_mode);
                        
            fn_global.load_vms_treeview(treeView_vms_all, 0);

            treeView1.Visible = false;
        }

        private TreeNode Get_Tree_mode(TreeNodeCollection nodes, string a_key)
        {
            TreeNode _node = null;
            foreach (TreeNode node in nodes)
            {
                if (node.Text == a_key)
                {
                    _node = node;
                    return _node;
                }
                if (node.Nodes.Count > 0)
                {
                    _node = Get_Tree_mode(node.Nodes, a_key);
                    if (_node != null)
                        return _node;
                }
            }
            return _node;
        }

        private void load_adhoc_group()
        {
            comboBox_adhoc.Items.Clear();
            inc.Adhoc_Group = null;
            //            inc.db_control.SQL = "select fd_id,fd_gdm,fd_mode,fd_timer,fd_groupid,fd_rows,fd_color,fd_color2,fd_color3 from tb_adhoc ";
            //inc.db_control.SQL = "select * from tb_adhoc where fd_groupid>0 and fd_gdm<>0 and fd_delete<>1 order by fd_groupid";
            inc.db_control.SQL = "select t1.* from tb_adhoc as t1,tb_gdm as t2 where t1.fd_gdm=t2.fd_id and t1.fd_groupid>0 and t2.fd_delete=0 and t1.fd_delete=0 order by t1.fd_groupid";
            OdbcDataReader myreader = inc.db_control.SQLExecuteReader();
            if (myreader != null)
            {
                inc.adhoc_group[] _temp_group = new inc.adhoc_group[myreader.RecordsAffected];


                int i = -1;
                while (myreader.Read())
                {
                    if ((i == -1) || ((i >= 0) && (_temp_group[i].groupid != (int)(long)myreader["fd_groupid"])))
                    {
                        i++;
                        _temp_group[i].gdm_list = new List<string>();
                        _temp_group[i].id = new List<Int64>();

                        _temp_group[i].gdm_list.Add(((int)myreader["fd_gdm"]).ToString());
                        _temp_group[i].id.Add((Int64)myreader["fd_id"]);

                        _temp_group[i].mode = (int)myreader["fd_mode"];
                        _temp_group[i].timer = (int)myreader["fd_timer"];
                        _temp_group[i].rows = (string)myreader["fd_rows"];
                        _temp_group[i].useendtime = (int)myreader["fd_useendtime"];
                        _temp_group[i].groupid = (int)((long)myreader["fd_groupid"]);
                        _temp_group[i].color = (int)myreader["fd_color"];
                        _temp_group[i].starttime = (DateTime)myreader["fd_starttime"];
                        _temp_group[i].endtime = (DateTime)myreader["fd_endtime"];
                        _temp_group[i].name = (String)myreader["fd_name"];
                        if (myreader["fd_addtime"] != DBNull.Value)
                            _temp_group[i].addtime = (DateTime)myreader["fd_addtime"];
                    }
                    else
                    {
                        _temp_group[i].gdm_list.Add(((int)myreader["fd_gdm"]).ToString());
                        _temp_group[i].id.Add((Int64)myreader["fd_id"]);
                    }
                }
                if (i >= 0)
                {
                    inc.Adhoc_Group = new inc.adhoc_group[i + 1];
                    Array.Copy(_temp_group, inc.Adhoc_Group, i + 1);

                    inc.Group_id = (int)inc.Adhoc_Group[i].groupid;
                }
                //listBox_adhoc_group.Items.Clear();
                #region modify at 2009-07-20 design of adhoc changed
                if (inc.Adhoc_Group != null)
                {
                    for (i = 0; i < inc.Adhoc_Group.Length; i++)
                    {
                        comboBox_adhoc.Items.Add(inc.Adhoc_Group[i].name);
                    }
                }
                #endregion 
            }                                               
        }
        #region modify at 2009-07-20 design of adhoc changed
        private void b1_Click(object sender, EventArgs e)        
        { 
            Form a = ((Button)sender).FindForm();
            
            string _name = ((TextBox)a.Controls.Find("t1", true)[0]).Text;
            if(_name=="")
            {
                MessageBox.Show("Please Input Adhoc Name!");
                return;
            }
            try
            {
                for (int i = 0; i < inc.Adhoc_Group.Length; i++)
                {
                    if (inc.Adhoc_Group[i].name == _name)
                    {
                        MessageBox.Show("Adhoc Name Aleady Exist");
                        return;
                    }
                }
            }
            catch { }
            comboBox_adhoc.Items.Add(_name);
            new_adhoc = true;
            comboBox_adhoc.SelectedIndex = comboBox_adhoc.Items.Count - 1;
            new_adhoc = false;
            ((Button)sender).FindForm().Close();
        }

        private void b2_Click(object sender, EventArgs e)
        {
            ((Button)sender).FindForm().Close();
        }

        private void button48_Click(object sender, EventArgs e)
        {
            #region modify at 2009-07-20 design of adhoc changed
            //if (comboBox_adhoc_vmss.SelectedIndex >= 0)
            //{
            //    add_new_adhocgroup();
            //}
            Form f = new Form();
            f.Name = "add_new_group";
            f.Text = "New Adhoc Name";
            f.Size = new Size(400, 200);
            Label l1 = new Label();
            TextBox t1 = new TextBox();

            l1.Text = "Adhoc Name:";
            l1.Left = 60;
            l1.Top = 50;

            t1.Left = l1.Left + l1.Width + 10;
            t1.Top = l1.Top - 2;
            t1.Width = 150;
            t1.Name = "t1";

            f.Controls.Add(l1);
            f.Controls.Add(t1);

            Button b1 = new Button();
            b1.Text = "Confirm";
            b1.Top = l1.Top + 50;
            b1.Left = l1.Left + 20;
            b1.Click += new EventHandler(b1_Click);
            
            Button b2 = new Button();
            b2.Text = "Cancel";
            b2.Top = l1.Top + 50;
            b2.Left = b1.Left + b1.Width + 80;
            b2.Click += new EventHandler(b2_Click);

            f.Controls.Add(b1);
            f.Controls.Add(b2);

            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
            #endregion
        }
        #endregion

        /// <summary>
        /// check a gdm already in a adhoc group or not
        /// </summary>
        /// <returns>
        /// true - in a group
        /// false -  not in any group
        /// </returns>
        private bool check_adhoc_group()
        {
            for (int i = 0; i < listBox_adhoc_group.Items.Count; i++)
            {
                string _temp = listBox_adhoc_group.Items[i].ToString();
                if (_temp.IndexOf(comboBox_adhoc_vmss.Text) >= 0)
                {
                    MessageBox.Show("Already In Group");
                    return false;
                }
            }
            return true;
        }

        private bool add_new_adhocgroup()
        {

            if (!check_adhoc_group())
                return false;
            listBox_adhoc_group.Items.Add(comboBox_adhoc_vmss.Text);
            listBox_adhoc_group.SelectedIndex = listBox_adhoc_group.Items.Count - 1;
            return true;
        }
        /// <summary>
        /// add new vms to group
        /// </summary>
        /// <returns></returns>
        private bool add_vms2group()
        {            
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Nodes.Count == 0)
                {
                    listBox_adhoc_group.Items.Add(treeView1.SelectedNode.Text);
                    int _id = inc._vms[fn_global.fn_vms_name2index(treeView1.SelectedNode.Text)]._id;
                    treeView1.SelectedNode.Remove();
                    if (listBox_adhoc_group.Items.Count == 1)
                    {
                        int _group_id = inc.Group_id + 1;
                        inc.db_control.SQL = inc.db_control.insert_table("tb_adhoc", "", _id.ToString(), comboBox_adhoc.Text, "1", "2", "2", "1800-01-01 00:00:00"
                            , "0", "1800-01-01 00:00:00", inc.currentuser.ToString(), "", "0"
                            , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "1800-01-01 00:00:00", "0", _group_id.ToString(),"0");

                        if (inc.db_control.SQLExecuteReader() != null)
                        {
                            load_adhoc_group();
                            comboBox_adhoc.SelectedIndex = comboBox_adhoc.Items.Count - 1;

                            fn_global.log_operateion((int)inc.LOGMSGCODE.ADI01, comboBox_adhoc.Text, 1);
                        }
                        else
                        {
                            MessageBox.Show("Create Group Failed");
                        }

                    }

                    
                    return true;
                }
            }            
            return false; 
        }
        /// <summary>
        /// add a gdm to a exist group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            if(comboBox_adhoc.SelectedIndex>=0)
                treeView1.Visible = true;
            //add_vms2group();
            //set_send_button_enable();
        }

        private void remove_cur_vms(int i)
        {
            #region modify at 2009-07-20 design of adhoc changed
            /*if (comboBox_adhoc_vmss.Text != "")
            {
                for (int i = 0; i < listBox_adhoc_group.Items.Count; i++)
                {
                    string _temp = listBox_adhoc_group.Items[i].ToString();
                    int pos = _temp.IndexOf(comboBox_adhoc_vmss.Text);
                    if (pos >= 0)
                    {
                        if (_temp == comboBox_adhoc_vmss.Text)  
                        {
                            if (MessageBox.Show("Are you Sure delete the group,it will auto send to VMS", "Delete Group", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                for (int j = 0; j < inc.Adhoc_Group[i].id.Count; j++)
                                {
                                    delete_adhoc((int)(inc.Adhoc_Group[i].id[j]));
                                    update_site(int.Parse(inc.Adhoc_Group[i].gdm_list[j]));
                                }
                                listBox_adhoc_group.Items.Remove(listBox_adhoc_group.Items[i]);
                            }
                        }
                        else
                        {
                            if (pos == 0)
                                listBox_adhoc_group.Items[i] = _temp.Remove(pos, comboBox_adhoc_vmss.Text.Length + 1);
                            else
                                listBox_adhoc_group.Items[i] = _temp.Remove(pos - 1, comboBox_adhoc_vmss.Text.Length + 1);
                        }
                        listBox_adhoc_group.SelectedIndex = i - 1;
                        return;
                    }
                }
            }*/
            #endregion           

            if (i >= 0)
            {
                string vms_name = listBox_adhoc_group.Items[i].ToString();
                int index = fn_global.fn_vms_name2index(vms_name);
                if (index >= 0)
                {
                    int _area_index = fn_global.fn_area_id2index(inc._vms[index].area);
                    string nodes_name = inc.tb_area[_area_index].name;
                    bool find = false;
                    for (int j = 0; j < treeView1.Nodes[0].Nodes.Count; j++)
                    {
                        if (treeView1.Nodes[0].Nodes[j].Text == nodes_name)
                        {
                            treeView1.Nodes[0].Nodes[j].Nodes.Add(vms_name);
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        treeView1.Nodes[0].Nodes.Add(nodes_name);
                        for (int j = 0; j < treeView1.Nodes[0].Nodes.Count; j++)
                        {
                            if (treeView1.Nodes[0].Nodes[j].Text == nodes_name)
                            {
                                treeView1.Nodes[0].Nodes[j].Nodes.Add(vms_name);
                                find = true;
                                break;
                            }
                        }

                    }
                    listBox_adhoc_group.Items.Remove(listBox_adhoc_group.Items[i]);
                }
            }

            //set_send_button_enable();
        }
        /// <summary>
        /// delete a gdm grom a group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            int i = listBox_adhoc_group.SelectedIndex;
            remove_cur_vms(i);
        }

        private void comboBox_adhoc_vmss_Click(object sender, EventArgs e)
        {
            fn_global.show_treeview_location(treeView_vms, ((ComboBox)sender).Left, ((ComboBox)sender).Top + ((ComboBox)sender).Height);
        }

        private void listBox_adhoc_group_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region modify at 2009-7-20 adhos group disign changed
            int _index = listBox_adhoc_group.SelectedIndex;
            
            if (_index >= 0)
                fn_global.load_msg_treeview(treeView_msg, listBox_adhoc_group.Items[_index].ToString());
            //            if (add_new_vms2group)
//                return;

//            groupBox74.Enabled = true;

//            int _index = listBox_adhoc_group.SelectedIndex;
//            clear_msg = true;

//            clear_messge_groupbox();

//            if (_index >= 0)
//            {
//                //_index = get_vms_group2index(listBox_adhoc_group.Items[listBox_adhoc_group.SelectedIndex].ToString());
//                string _temp = listBox_adhoc_group.Items[_index].ToString();
//                if (_temp.IndexOf(comboBox_adhoc_vmss.Text) >= 0)
//                {
//                    if ((_temp.IndexOf(",") > 0) && (radioButton_i_cmd.Checked))
//                    {
//                        groupBox74.Enabled = false;
//                    }
//                }
//                fn_global.load_msg_treeview(treeView_msg, listBox_adhoc_group.Items[_index].ToString());

////                groupBox74.Enabled = true;
//                if (_index < inc.Adhoc_Group.Length)
//                {
//                    string groupid = inc.Adhoc_Group[_index].groupid.ToString();
//                    comboBox_adhoc_mode.SelectedIndex = inc.Adhoc_Group[_index].mode;
//                    textBox1.Text = inc.Adhoc_Group[_index].timer.ToString();
//                    int _color = inc.Adhoc_Group[_index].color;
//                    adhoc_start = inc.Adhoc_Group[_index].starttime;
//                    adhoc_end = inc.Adhoc_Group[_index].endtime;
//                    if (inc.Adhoc_Group[_index].useendtime == 1)
//                        use_time_c = true;
//                    else
//                        use_time_c = false;

//                    List<String> rows_list = fn_global.fn_str2strlist(inc.Adhoc_Group[_index].rows);
//                    for (int j = 1; j < 7; j++)
//                    {
//                        string s_key = "row" + j.ToString() + "_use";

//                        int i_use = 0;
//                        try { i_use = int.Parse(fn_global.fn_txt_getvalue(ref rows_list, s_key)); }
//                        catch { }

//                        for (int p = 0; p < 3; p++)
//                        {
//                            int asss = (int)System.Math.Pow(2, p + 1);

//                            int _num = j + p * 6;
//                            if (Tools.fn_check_bit(asss, i_use))
//                                ((CheckBox)this.Controls.Find("checkBox" + _num, true)[0]).Checked = true;
//                            else
//                                ((CheckBox)this.Controls.Find("checkBox" + _num, true)[0]).Checked = false;

//                            s_key = "row" + j.ToString() + "_line" + (p + 1).ToString();
//                            string str = fn_global.fn_txt_getvalue(ref rows_list, s_key);

//                            _num += 18;

//                            string _name_box = "comboBox" + _num.ToString();                            
//                            ((ComboBox)this.Controls.Find(_name_box, true)[0]).Text = inc.VMS_Name_color[_color].ToString();
                            
//                            for (int c = 0; c < 3; c++)
//                            {
//                                if (str.IndexOf(inc.VMS_color_code[c].ToString()) >= 0)
//                                {
//                                    ((ComboBox)this.Controls.Find(_name_box, true)[0]).Text = inc.VMS_Name_color[c].ToString();                                 
//                                    str = str.Remove(str.IndexOf(inc.VMS_color_code[c].ToString()), 3);
//                                    break;
//                                }                                
//                            }
                                                                                            
//                            _num -= 18;
//                            ((ComboBox)this.Controls.Find("comboBox" + _num, true)[0]).Text = str;                            
//                        }

//                    }
//                }
//            }
//            else
//            {
////                groupBox74.Enabled = false;
//            }
            //            clear_msg = false;
            #endregion
        }
        /// <summary>
        /// set adhoc message get grom premessage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _set_adhoc_msg(string pre_name, string combobox_name)
        {

            int _index = fn_global.fn_msg_name2index(pre_name);

            if (_index >= 0)
            {
                try
                {
                    int _line = (int.Parse(combobox_name.Substring(8)) - 1) / 6;
                    int _page = (int.Parse(combobox_name.Substring(8)) - 1) % 6;

                    string[] msgs = new string[6];
                    msgs[0] = inc.Pre_Msg[_index].line1;
                    msgs[1] = inc.Pre_Msg[_index].line2;
                    msgs[2] = inc.Pre_Msg[_index].line3;
                    msgs[3] = inc.Pre_Msg[_index].line4;
                    msgs[4] = inc.Pre_Msg[_index].line5;
                    msgs[5] = inc.Pre_Msg[_index].line6;

                    int len = 0;
                    for (int i = 5; i >= 0; i--)
                    {
                        if (msgs[i] != "")
                        {
                            len = i;
                            break;
                        }
                    }

                    for (int i = _page; i < 6; i++)
                    {
                        if ((i - _page) > len)
                            break;
                        ((ComboBox)this.Controls.Find("comboBox" + (_line * 6 + i + 1), true)[0]).Text = msgs[i - _page];
                    }
                }
                catch { }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void clear_messge_groupbox()
        {
            clear_msg = true;
            comboBox_adhoc_mode.SelectedIndex = 0;
            textBox1.Text = "2";
            for (int i = 1; i < 21; i++)
            {
                ((CheckBox)this.Controls.Find("checkBox" + i, true)[0]).Checked = false;
            }
            for (int i = 1; i < 36; i++)
            {
                ((ComboBox)this.Controls.Find("comboBox" + i, true)[0]).Text = "";
            }
            clear_msg = false;
        }
        /// <summary>
        /// set default value of adhoc group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button41_Click(object sender, EventArgs e)
        {
            clear_messge_groupbox();
        }

        /// <summary>
        /// delete adhoc from tb_adhoc
        /// </summary>
        /// <param name="id"></param>
        private void delete_adhoc(int id)
        {
            inc.db_control.SQL = "update tb_adhoc set fd_delete=1 ";
            inc.db_control.SQL += "where fd_id='" + id.ToString() + "'";
            if (inc.db_control.SQLExecuteReader() != null)
            {

                inc.db_control.SQL = "select fd_gdm from tb_adhoc ";
                inc.db_control.SQL += "where fd_id='" + id.ToString() + "'";
                OdbcDataReader myreader = inc.db_control.SQLExecuteReader();
                if (myreader != null)
                {
                    while (myreader.Read())
                    {
                        int index = fn_global.fn_vms_id2index((int)myreader["fd_gdm"]);
                        fn_global.log_operateion((int)inc.LOGMSGCODE.ADD01, inc._vms[index].name, 1);
                    }
                }
            }
        }

        private void save_adhoc()
        {
            #region modify at 2009-7-20 adhos group disign changed
            int i = comboBox_adhoc.SelectedIndex;
           
            if (i < inc.Adhoc_Group.Length)
            {
                for (int j = 0; j < inc.Adhoc_Group[i].gdm_list.Count; j++)
                {
                    int _gdmid = int.Parse(inc.Adhoc_Group[i].gdm_list[j].ToString());
                    bool ingroup = false;
                    foreach (object item in listBox_adhoc_group.Items)
                    {
                        string _vmsname = inc._vms[fn_global.fn_vms_id2index(_gdmid)].name;
                        if (_vmsname == item.ToString())
                            ingroup = true;
                    }

                    if (!ingroup)
                    {
                        delete_adhoc((int)inc.Adhoc_Group[i].id[j]);
                    }
                }

                foreach (object item in listBox_adhoc_group.Items)
                {
                    bool add = true;
                    for (int j = 0; j < inc.Adhoc_Group[i].gdm_list.Count; j++)
                    {
                        int _gdmid = int.Parse(inc.Adhoc_Group[i].gdm_list[j].ToString());
                        if (item.ToString() == inc._vms[fn_global.fn_vms_id2index(_gdmid)].name)
                        {
                            update_adhoc(i, j);
                            add = false;
                        }
                    }
                    if (add)
                    {
                        insert_adhoc2group(i, fn_global.fn_vms_name2index(item.ToString()));                       
                    }
                }                
            }
            #endregion
        }

        /// <summary>
        /// update adhoc 
        /// </summary>
        /// <param name="id"></param>
        private void update_adhoc(int group_index, int list_index)
        {
            int _timer = 2;
            _timer = int.Parse(textBox1.Text);
            int _mode = comboBox_adhoc_mode.SelectedIndex+1;
            string _fd_id = inc.Adhoc_Group[group_index].id[list_index].ToString();
            string _fd_gdm = inc.Adhoc_Group[group_index].gdm_list[list_index].ToString();

            string _fd_start_time = inc.Adhoc_Group[group_index].starttime.ToString("yyyy-MM-dd HH:mm:ss");
            string _fd_end_time = inc.Adhoc_Group[group_index].endtime.ToString("yyyy-MM-dd HH:mm:ss");
            string _fd_add_time = inc.Adhoc_Group[group_index].addtime.ToString("yyyy-MM-dd HH:mm:ss");
            int _fd_useendtime = (int)inc.Adhoc_Group[group_index].useendtime;

            string _fd_name = comboBox_adhoc.Text;

            int _color = comboBox38.SelectedIndex >= 0 ? comboBox38.SelectedIndex : 0;

            if (checkBox_pattern.Checked)
                _color += 128;

            string _rows = pack_rows();

            inc.db_control.SQL = inc.db_control.udpate_table("tb_adhoc", _fd_id, _fd_gdm, _fd_name, _mode.ToString()
                , _timer.ToString(), _color.ToString(), _fd_start_time, _fd_useendtime.ToString()
                , _fd_end_time, inc.currentuser.ToString(), _rows, "0", _fd_add_time, "1800-01-01 00:00:00", "0", inc.Adhoc_Group[group_index].groupid.ToString());

            inc.db_control.SQL += " where fd_id='" + _fd_id + "'";
            if (inc.db_control.SQLExecuteReader()!=null)
            {
                string _name = inc._vms[fn_global.fn_vms_id2index(int.Parse(_fd_gdm))].name;

                fn_global.log_operateion((int)inc.LOGMSGCODE.ADU01, _name, 1);
            }
        }
        private string pack_rows()
        {
            string s_3 = "";
            for (int i = 0; i < 6; i++)
            {
                string s_row = "row" + (i+1).ToString() + "_";//row1_
                string _name = s_row + "use";               //row1_use
                string linemsg = "";
                int lineused = 0;
                for (int j = 0; j < 3; j++)
                {           
                    int index =  i + 1 + j * 6;
                    if (((CheckBox)this.Controls.Find("checkBox" +index, true)[0]).Checked)
                    {                        
                        lineused += (int)System.Math.Pow(2, j + 1);
                    }
                    int index_color = ((ComboBox)this.Controls.Find("comboBox" + (index + 18), true)[0]).SelectedIndex;
                    string _color="";
                    if(index_color>=0)
                        _color = inc.VMS_color_code[index_color].ToString();

                    string _msg = ((ComboBox)this.Controls.Find("comboBox" + index, true)[0]).Text;
                    _msg = fn_global.fn_rawurlencode(_msg);
                    if(_msg!="")
                        linemsg += s_row + "line" + (j + 1).ToString() + "=" + _color + _msg + inc.split_sign;
                    else
                        linemsg += s_row + "line" + (j + 1).ToString() + "=" + inc.split_sign;

                }
                s_3 += _name + "=" + lineused.ToString() + inc.split_sign + linemsg;                
            }            
            return s_3;
        }
       
        /// <summary>
        /// update adhoc 
        /// </summary>
        /// <param name="id"></param>
        private void insert_adhoc2group(int group_index, int list_index)
        {
            int _timer = 2;
            _timer = int.Parse(textBox1.Text);
            int _mode = comboBox_adhoc_mode.SelectedIndex+1;            
            string _fd_gdm = inc._vms[list_index]._id.ToString();
            string _fd_start_time = adhoc_start.ToString("yyyy-MM-dd HH:mm:ss");
            string _fd_end_time = adhoc_end.ToString("yyyy-MM-dd HH:mm:ss");           
            int _fd_useendtime = 0;
            if(use_time_c)
                _fd_useendtime = 1;

            string _rows = pack_rows();

            int _color = comboBox38.SelectedIndex >= 0 ? comboBox38.SelectedIndex : 0;
            
            if(checkBox_pattern.Checked)
                _color += 128;

            int _group_id = inc.Group_id+1;
            if(group_index<inc.Adhoc_Group.Length)
                _group_id = (int)inc.Adhoc_Group[group_index].groupid;
            inc.db_control.SQL = inc.db_control.insert_table("tb_adhoc", "", _fd_gdm, comboBox_adhoc.Text, _mode.ToString()
                , _timer.ToString(), _color.ToString(), _fd_start_time, _fd_useendtime.ToString(), _fd_end_time, inc.currentuser.ToString()
                , _rows, "0", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "1800-01-01 00:00:00", "0", _group_id.ToString(),"0");

            if (inc.db_control.SQLExecuteReader() != null)
            {
                string _name = inc._vms[fn_global.fn_vms_id2index(int.Parse(_fd_gdm))].name;

                fn_global.log_operateion((int)inc.LOGMSGCODE.ADI01, _name, 1);
            }
        }

        private bool check_adhoc_before_send()
        {
            if (radioButton_i_cmd.Checked)
            {
                if (comboBox_adhoc_vmss.Text == "")
                {
                    MessageBox.Show("Please select a VMS");
                    return false;
                }                               
            }
            else
            {
                if (comboBox_adhoc.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select a group");
                    return false;
                }
            }
            return true;
        }
        private void button49_Click(object sender, EventArgs e)
        {            
            if (check_adhoc_before_send())
            {
                //return;
                int i=comboBox_adhoc.SelectedIndex;
                save_adhoc();
                #region modify at 2009-7-20 adhos group disign changed
                //if (radioButton_i_cmd.Checked)
                //{
                //    update_site(inc._vms[fn_global.fn_vms_name2index(comboBox_adhoc_vmss.Text)]._id);                    
                //}
                //else
                //{
                //    string[] _vmsgroup = listBox_adhoc_group.Items[listBox_adhoc_group.SelectedIndex].ToString().Split(',');
                //    for (int i = 0; i < _vmsgroup.Length; i++)
                //    {
                //        update_site(inc._vms[fn_global.fn_vms_name2index(_vmsgroup[i])]._id); 
                //    }
                //}
                #endregion


                update_site();
                
                load_adhoc_group();
                try
                {
                    comboBox_adhoc.SelectedIndex = i;
                }
                catch { }
            }
        }

        private void comboBox38_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 19; i <= 36; i++)
            {
                ((ComboBox)this.Controls.Find("comboBox" + i, true)[0]).SelectedIndex = ((ComboBox)sender).SelectedIndex;
            }
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            int index = int.Parse(((CheckBox)sender).Name.Substring(8)) - 18;
            if (index == 1)
                check_pages1_change = true;
            else
                check_pages1_change = false;

            for (int i = 0; i < 6; i++)
            {
                int j = (index - 1) * 6 + i + 1;
                ((CheckBox)this.Controls.Find("checkBox" + j, true)[0]).Checked = ((CheckBox)sender).Checked;
            }
            check_pages1_change = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox_adhoc.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a adhoc");
                return;
            }
            if (listBox_adhoc_group.Items.Count == 0)
            {
                MessageBox.Show("At least include one VMS");
                return;
            }
            Form_Time_Config f = new Form_Time_Config();
            f._index_adhox = comboBox_adhoc.SelectedIndex;
            f.isadhoc = true;
            f.Start_time = inc.Adhoc_Group[f._index_adhox].starttime;
            f.End_time = inc.Adhoc_Group[f._index_adhox].endtime;
            if (inc.Adhoc_Group[f._index_adhox].useendtime == 1)
                f.useendtime = true;
            else
                f.useendtime = false;
            f.Text = "Adhoc Config Advance";

            f.ShowDialog();
        }

        private void comboBox_adhoc_vmss_SelectedIndexChanged(object sender, EventArgs e)
        {
            fn_global.load_msg_treeview(treeView_msg, comboBox_adhoc_vmss.Text);

            for (int i = 0; i < listBox_adhoc_group.Items.Count; i++)
            {
                string _temp=listBox_adhoc_group.Items[i].ToString();
                if (_temp.IndexOf(comboBox_adhoc_vmss.Text) >= 0)
                {
                    listBox_adhoc_group.SelectedIndex = i;
                    return;
                }
            }
            listBox_adhoc_group.SelectedIndex = -1;           
        }

        private string get_vms_group_str(int i)
        {
            string _group_list = "";
            for (int j = 0; j < inc.Adhoc_Group[i].gdm_list.Count; j++)
            {
                int _gdmid = int.Parse(inc.Adhoc_Group[i].gdm_list[j]);
                int _index = fn_global.fn_vms_id2index(_gdmid);

                if (_index >= 0)
                {
                    _group_list += inc._vms[_index].name;
                    if (j < inc.Adhoc_Group[i].gdm_list.Count - 1)
                    {
                        _group_list += ",";
                    }
                }
            }
            return _group_list;
        }

        private int get_vms_group2index(string str)
        {
            int r = -1;
            for (int i = 0; i < inc.Adhoc_Group.Length; i++)
            {
                string _temp = get_vms_group_str(i);
                if (_temp == str)
                {
                    r = i;
                }                    
            }
            return r;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_VMSs.SelectedIndex >= 0)
                {
                    //comboBox_flash_mode.SelectedIndex = 0;
                    for (int l = 1; l < 7; l++)
                    {
                        ((RichTextBox)this.Controls.Find("richTextBox" + l, true)[0]).Text = "";
                    }
                    int _index = fn_global.fn_vms_name2index(comboBox_VMSs.Text);
                    if (_index >= 0)
                    {
                        string _cmd = "<led_status><gdm=" + inc._vms[_index]._id + ">";
                        listBox_status.Items.Add(_cmd);

                        string _str = "";
                        string _path = "gdm" + inc._vms[_index]._id.ToString();

                        if (System.Configuration.ConfigurationSettings.AppSettings["tcpdemo"] != "1")
                            fn_global.fn_file_delete(_path, "ledstatus.txt"); 

                        if (fn_global.fn_tcp_send(_cmd))
                        {                          
  
                            if (fn_global.fn_cmd_check_finish(_path, "ledstatus.txt", ref _str))
                            {
                                this.Cursor = Cursors.Default;
                                string[] str_s = _str.Split('\n');

                                if (_str.IndexOf("isconnect") >= 0)
                                {
                                    listBox_status.Items.Add(str_s[1].Replace("=", " "));
                                }
                                else if (str_s.Length <= 3)
                                {
                                    listBox_status.Items.Add("Can not get value from VMS");
                                }
                                else
                                {
                                    string _toggle = str_s[1].Substring(str_s[1].IndexOf("=") + 1);
                                    _toggle = _toggle.Remove(_toggle.Length - 1);
                                    comboBox_flash_mode.Text = _toggle;

                                    Color c_c = inc.VMS_Color[0];


                                    for (int l = 1; l < 7; l++)
                                    {
                                        string _msg = str_s[(l * 3)].Substring(str_s[(l * 3)].IndexOf("=") + 1);
                                        _msg = _msg.Remove(_msg.Length - 1);

                                        int[] _s_len = new int[10] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                                        Color[] _s_clolr = new Color[10];

                                        int _i_len = 0;

                                        for (int i = 0; i < _msg.Length; i++)
                                        {
                                            if (_msg[i] == '^')
                                            {
                                                if (i == 0)
                                                {
                                                    string s_c = _msg[i].ToString() + _msg[i + 1].ToString() + _msg[i + 2].ToString();
                                                    int index_c = fn_global.fn_VMS_color2index(s_c);
                                                    _s_clolr[0] = inc.VMS_Color[index_c];
                                                }
                                                else
                                                {
                                                    for (int s = 9; s >= 0; s--)
                                                    {
                                                        if (_s_len[s] >= 0)
                                                        {
                                                            _i_len = _s_len[s];
                                                            break;
                                                        }
                                                    }

                                                    string s_c = _msg[i].ToString() + _msg[i + 1].ToString() + _msg[i + 2].ToString();
                                                    int index_c = fn_global.fn_VMS_color2index(s_c);
                                                    c_c = inc.VMS_Color[index_c];

                                                    for (int s = 0; s < 10; s++)
                                                    {
                                                        if (_s_len[s] < 0)
                                                        {
                                                            _s_len[s] = i - (s * 3) > 0 ? i - (s * 3) : 0;
                                                            _s_clolr[s] = c_c;
                                                            break;
                                                        }
                                                    }
                                                }


                                                i += 2;
                                            }
                                            else
                                            {
                                                ((RichTextBox)this.Controls.Find("richTextBox" + l, true)[0]).Text += _msg[i];
                                            }
                                        }

                                        bool f = true;
                                        for (int s = 9; s >= 0; s--)
                                        {
                                            if (_s_len[s] < 0)
                                            {
                                                continue;
                                            }

                                            int _len1 = ((RichTextBox)this.Controls.Find("richTextBox" + l, true)[0]).Text.Length;
                                            int len = 0;
                                            if (f)
                                            {
                                                f = false;
                                                len = _s_len[s];

                                            }
                                            else
                                            {
                                                _len1 = _s_len[s + 1];
                                                len = _s_len[s];
                                            }
                                            ((RichTextBox)this.Controls.Find("richTextBox" + l, true)[0]).Select(len, _len1 - len);
                                            ((RichTextBox)this.Controls.Find("richTextBox" + l, true)[0]).SelectionColor = _s_clolr[s];
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void load_group_message()
        {
            groupBox74.Enabled = true;

            int _index = comboBox_adhoc.SelectedIndex;
            clear_msg = true;

            clear_messge_groupbox();

            if (_index >= 0)
            {
                string groupid = inc.Adhoc_Group[_index].groupid.ToString();
                comboBox_adhoc_mode.SelectedIndex = inc.Adhoc_Group[_index].mode-1;
                textBox1.Text = inc.Adhoc_Group[_index].timer.ToString();
                int _color = inc.Adhoc_Group[_index].color;
                if (_color > 10)
                {
                    checkBox_pattern.Checked = true;
                    _color -= 128;
                }
                adhoc_start = inc.Adhoc_Group[_index].starttime;
                adhoc_end = inc.Adhoc_Group[_index].endtime;
                if (inc.Adhoc_Group[_index].useendtime == 1)
                    use_time_c = true;
                else
                    use_time_c = false;

                List<String> rows_list = fn_global.fn_str2strlist(inc.Adhoc_Group[_index].rows);
                for (int j = 1; j < 7; j++)
                {
                    string s_key = "row" + j.ToString() + "_use";

                    int i_use = 0;
                    try { i_use = int.Parse(fn_global.fn_txt_getvalue(ref rows_list, s_key)); }
                    catch { }

                    for (int p = 0; p < 3; p++)
                    {
                        int asss = (int)System.Math.Pow(2, p + 1);

                        int _num = j + p * 6;
                        if (Tools.fn_check_bit(asss, i_use))
                            ((CheckBox)this.Controls.Find("checkBox" + _num, true)[0]).Checked = true;
                        else
                            ((CheckBox)this.Controls.Find("checkBox" + _num, true)[0]).Checked = false;

                        s_key = "row" + j.ToString() + "_line" + (p + 1).ToString();
                        string str = fn_global.fn_txt_getvalue(ref rows_list, s_key);

                        _num += 18;

                        string _name_box = "comboBox" + _num.ToString();

                        ((ComboBox)this.Controls.Find(_name_box, true)[0]).Text = inc.VMS_Name_color[_color].ToString();

                        for (int c = 0; c < 3; c++)
                        {
                            if (str.IndexOf(inc.VMS_color_code[c].ToString()) >= 0)
                            {
                                ((ComboBox)this.Controls.Find(_name_box, true)[0]).Text = inc.VMS_Name_color[c].ToString();
                                str = str.Remove(str.IndexOf(inc.VMS_color_code[c].ToString()), 3);
                                break;
                            }
                        }

                        _num -= 18;
                        ((ComboBox)this.Controls.Find("comboBox" + _num, true)[0]).Text = str;
                    }

                }
            }
            else
            {
                //                groupBox74.Enabled = false;
            }
            clear_msg = false;
        }
        #region modify at 2009-7-20 adhoc group design changed
        private void load_current_adhoc(int index)
        {
            listBox_adhoc_group.Items.Clear();
            string vms_group = "";

            if (!new_adhoc)
            {
                try
                {
                    if (index >= 0)
                    {
                        
                        vms_group = "";
                        for (int j = 0; j < inc.Adhoc_Group[index].gdm_list.Count; j++)
                        {
                            int _gdmid = int.Parse(inc.Adhoc_Group[index].gdm_list[j]);
                            int _index = fn_global.fn_vms_id2index(_gdmid);

                            if (_index >= 0)
                            {
                                listBox_adhoc_group.Items.Add(inc._vms[_index].name);
                                vms_group += inc.VMS[_index].ToString() + ",";
                            }
                        }

                        fn_global.load_msg_treeview(treeView_msg, vms_group);
                                               
                        load_group_message();
                    }       
                }
                catch { }
            }
            
            int _w = treeView1.Width;
            int _h = treeView1.Height;

            vms_group = "";
            try
            {
                for (int i = 0; i < inc.VMS.Length; i++)
                {
                    for (int m = 0; m < inc.Adhoc_Group.Length; m++)
                    {
                        foreach (string _vmsid in inc.Adhoc_Group[m].gdm_list)
                        {
                            vms_group += inc._vms[fn_global.fn_vms_id2index(int.Parse(_vmsid))].name + ",";
                        }
                    }
                }
            }
            catch { }
            fn_global.update_vms_treeview(treeView1, vms_group);
            
            treeView1.Height = _h;
            treeView1.Width = _w;

            //set_send_button_enable();
            if (load_from_main)
            {
                for (int i = 0; i < listBox_adhoc_group.Items.Count; i++)
                {
                    if (listBox_adhoc_group.Items[i].ToString() == inc._vms[gdm_index].name)
                    {
                        listBox_adhoc_group.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
        private void set_send_button_enable()
        {
            if (listBox_adhoc_group.Items.Count > 0)
                button49.Enabled = true;
            else
                button49.Enabled = false;
        }
        private void comboBox_adhoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear_messge_groupbox();
            load_current_adhoc(comboBox_adhoc.SelectedIndex);
        }
        #endregion

        private void button42_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_adhoc_mode.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select flashing mode");
                    return;
                }

                Form_priview_new f = new Form_priview_new();
                f.timer = 4;
                try
                {
                    f.timer = int.Parse(textBox1.Text);
                }
                catch { }


                f.mode = comboBox_adhoc_mode.SelectedIndex >= 0 ? comboBox_adhoc_mode.SelectedIndex : 0;
                f.IsPattern = checkBox_pattern.Checked;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        try
                        {
                            int index = i * 6 + j + 1;

                            int c = ((ComboBox)this.Controls.Find("comboBox" + (index + 18), true)[0]).SelectedIndex;
                            string _color = inc.VMS_color_code[c].ToString();

                            f.msg_pages[i, j].msg = _color + ((ComboBox)this.Controls.Find("comboBox" + index, true)[0]).Text;
                            f.msg_pages[i, j].used = ((CheckBox)this.Controls.Find("checkBox" + index, true)[0]).Checked;
                        }
                        catch { }
                    }
                }
                f.ShowDialog();
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you Sure delete the adhoc", "Delete Adhoc", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                while (listBox_adhoc_group.Items.Count > 0)
                {
                    listBox_adhoc_group.SelectedIndex = listBox_adhoc_group.Items.Count - 1;
                    if (listBox_adhoc_group.Items[listBox_adhoc_group.SelectedIndex].ToString() == "")
                        break;
                    remove_cur_vms(listBox_adhoc_group.SelectedIndex);
                }
                try
                {
                    for (int j = 0; j < inc.Adhoc_Group[comboBox_adhoc.SelectedIndex].gdm_list.Count; j++)
                    {
                        int _gdmid = int.Parse(inc.Adhoc_Group[comboBox_adhoc.SelectedIndex].gdm_list[j].ToString());
                        delete_adhoc((int)inc.Adhoc_Group[comboBox_adhoc.SelectedIndex].id[j]);
                    }
                }
                catch { }
                load_adhoc_group();
            }
           
        }

        private void comboBox_VMSs_DropDown(object sender, EventArgs e)
        {
            fn_global.show_treeview_location(treeView_vms_all, ((ComboBox)sender).Left, ((ComboBox)sender).Top + ((ComboBox)sender).Height);
        }

        private void Form_Adhoc_condig_FormClosed(object sender, FormClosedEventArgs e)
        {                        
            inc.db_control.SQL = "update tb_adhoc set fd_delete=1 where fd_rows=''";
            inc.db_control.SQLExecuteReader();
        }
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            add_vms2group();
            treeView1.Visible = false;
        }

        private void groupBox68_Enter(object sender, EventArgs e)
        {

        }
        private void b4_Click(object sender, EventArgs e)
        {
            ((Button)sender).FindForm().Close();
        }
        private void b3_Click(object sender, EventArgs e)
        {
            Form a = ((Button)sender).FindForm();

            string _name = ((TextBox)a.Controls.Find("t1", true)[0]).Text;
            if (_name == "")
            {
                MessageBox.Show("Please Input Adhoc Name!");
                return;
            }
            try
            {
                for (int i = 0; i < inc.Adhoc_Group.Length; i++)
                {
                    if (inc.Adhoc_Group[i].name == _name)
                    {
                        MessageBox.Show("Adhoc Name Aleady Exist");
                        return;
                    }
                }
            }
            catch { }
            inc.db_control.SQL = "update tb_adhoc set fd_name='" + _name + "' where fd_groupid=" + inc.Adhoc_Group[comboBox_adhoc.SelectedIndex].groupid + " and fd_delete=0";
            if (inc.db_control.SQLExecuteReader() != null)
            {

                int i_index = comboBox_adhoc.SelectedIndex;
                load_adhoc_group();

                comboBox_adhoc.SelectedIndex = i_index;
            }
            else
                MessageBox.Show("Update Adhoc name failed");
            ((Button)sender).FindForm().Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form f = new Form();
            f.Name = "Rename_Adhoc";
            f.Text = "Rename Adhoc";
            f.Size = new Size(400, 200);
            Label l1 = new Label();
            TextBox t1 = new TextBox();

            l1.Text = "Adhoc Name:";
            l1.Left = 60;
            l1.Top = 50;

            t1.Left = l1.Left + l1.Width + 10;
            t1.Top = l1.Top - 2;
            t1.Width = 150;
            t1.Name = "t1";

            f.Controls.Add(l1);
            f.Controls.Add(t1);

            Button b1 = new Button();
            b1.Text = "Confirm";
            b1.Top = l1.Top + 50;
            b1.Left = l1.Left + 20;
            b1.Click += new EventHandler(b3_Click);
            
            Button b2 = new Button();
            b2.Text = "Cancel";
            b2.Top = l1.Top + 50;
            b2.Left = b1.Left + b1.Width + 80;
            b2.Click += new EventHandler(b4_Click);

            f.Controls.Add(b1);
            f.Controls.Add(b2);

            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_VMSs.SelectedIndex >= 0)
                {
                    //comboBox_flash_mode.SelectedIndex = 0;
                    for (int l = 1; l < 7; l++)
                    {
                        ((RichTextBox)this.Controls.Find("richTextBox" + l, true)[0]).Text = "";
                    }
                    int _index = fn_global.fn_vms_name2index(comboBox_VMSs.Text);
                    if (_index >= 0)
                    {
                        string _cmd = "<led_status><gdm=" + inc._vms[_index]._id + ">";
                        listBox_status.Items.Add(_cmd);

                        string _str = "";
                        string _path = "gdm" + inc._vms[_index]._id.ToString();

                        if (System.Configuration.ConfigurationSettings.AppSettings["tcpdemo"] != "1")
                            fn_global.fn_file_delete(_path, "ledstatus.txt");

                        if (fn_global.fn_tcp_send(_cmd))
                        {

                            if (fn_global.fn_cmd_check_finish(_path, "ledstatus.txt", ref _str))
                            {
                                string[] str_s = _str.Split('\n');
                                if (_str.IndexOf("isconnect") >= 0)
                                {
                                    listBox_status.Items.Add(str_s[1].Replace("=", " "));
                                }
                                else if (str_s.Length <= 3)
                                {
                                    listBox_status.Items.Add("Can not get value from VMS");
                                }
                                else
                                {
                                    this.Cursor = Cursors.Default;
                                    Form_3Pages_Diplay f = new Form_3Pages_Diplay();
                                    f.Rec_Message = _str;
                                    f.ShowDialog();
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }
    }
}

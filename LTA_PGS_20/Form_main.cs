using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Windows.Forms;
using System.IO;

namespace LTA_PGS_20
{
    public partial class Form_main : Form
    {
        private int _new_form_tabindex = 0;
        PictureBox picture_location = new PictureBox();
        PictureBox picture_vms = new PictureBox();
        f_paintbox paint_box = new f_paintbox();

        Label lm;

        private Size Picture_Location_size = new Size(0,0);

        MenuStrip menuStrip2 = new MenuStrip();

        private void getareamaps()
        {
            tabControl1.TabPages.Add("0", "Main");
            for (int i = 0; i < inc.tb_area.Length; i++)
            {
                string file = inc.images_path + "\\" + inc.tb_area[i].map_name;
                if(!File.Exists(file))
                {
                    fn_global.fn_download_file(file, "images", inc.tb_area[i].map_name);
                }
                if(inc.tb_area[i].show==1)
                    tabControl1.TabPages.Add(inc.tb_area[i].fd_id.ToString(),inc.tb_area[i].name);    
            }
        }
        public Form_main()
        {
            InitializeComponent();

            try
            {
                string ip = System.Configuration.ConfigurationSettings.AppSettings["DataBase_IP"];
                string databasename = System.Configuration.ConfigurationSettings.AppSettings["DataBase_Name"];
                string password = System.Configuration.ConfigurationSettings.AppSettings["DataBase_PWD"];

                inc.db_control = new DB_Control();
                inc.db_control.DBAddress = ip;
                inc.db_control.DBName = databasename;
                inc.db_control.Password = password;
                inc.db_control.DB_Init();

                fn_global.load_area_table();

                if (System.Configuration.ConfigurationSettings.AppSettings["UseRemoteDisk"] == "1")
                {
                    inc.remote_disk = new RemoteFile();
                    inc.remote_disk.NetWorkConnection();
                    if (!inc.remote_disk.Connected)
                        MessageBox.Show("Create Map Disk Failed");
                }
                else if (System.Configuration.ConfigurationSettings.AppSettings["UseRemoteDisk"] == "2")
                {
                    inc.remote_disk = new RemoteFile();
                    inc.remote_disk.Connected = true;
                }

                inc.images_path = Environment.CurrentDirectory + "\\images";

                getareamaps();

                int _width = SystemInformation.PrimaryMonitorSize.Width;
                int _height = SystemInformation.PrimaryMonitorSize.Height;


                if (System.Configuration.ConfigurationSettings.AppSettings["use_format_screen"] == "1")
                {
                    _width = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["format_width"]);
                    _height = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["format_height"]);
                    this.Height = _height;
                    this.Width = _width;

                    this.StartPosition = FormStartPosition.WindowsDefaultLocation;
                    this.WindowState = FormWindowState.Normal;
                }

                panel1.Width = _width - panel2.Width - 20;
                tabControl1.Width = panel1.Width - 5;
                panel2.Left = panel1.Width + 5;
                panel1.Height = _height - 90;
                tabControl1.Height = panel1.Height - 10;

                //tabPage4.Width = tabControl1.Width;
                //TabPages_Harbfront.Width = tabControl1.Width;
                //TabPages_Orchard.Width = tabControl1.Width;
                //TabPages_Marina.Width = tabControl1.Width;


                //tabPage4.Height = tabControl1.Height;
                //TabPages_Harbfront.Height = tabControl1.Height;
                //TabPages_Orchard.Height = tabControl1.Height;
                //TabPages_Marina.Height = tabControl1.Height;            

                panel2.Height = panel1.Height;
                panel3.Top = panel2.Height - panel3.Height - 2;

                groupBox1.Height = panel2.Height - panel4.Height - panel3.Height - 30;
                groupBox1.Top = panel4.Height + 25;
                listBox1.Height = groupBox1.Height - 25;
                groupBox1.Visible = false;
                //listBox1.a = true;



                fn_global.make_fix_map_pic(tabControl1.Size);

                pictureBox1.Image = new Bitmap(Image.FromFile(inc.images_path + "\\gdm_green.gif"), pictureBox1.Size);
                pictureBox2.Image = new Bitmap(Image.FromFile(inc.images_path + "\\gdm_yellow.gif"), pictureBox1.Size);
                pictureBox3.Image = new Bitmap(Image.FromFile(inc.images_path + "\\gdm_red.gif"), pictureBox1.Size);
                pictureBox4.Image = new Bitmap(Image.FromFile(inc.images_path + "\\cms_green.gif"), pictureBox1.Size);
                pictureBox5.Image = new Bitmap(Image.FromFile(inc.images_path + "\\cms_red.gif"), pictureBox1.Size);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void load_Management_form()
        {
            Form_Management f = new Form_Management();
            f.Page_num = _new_form_tabindex;
            //f.Show();
            f.ShowDialog();            
        }

        private void load_Report_form()
        {
            Form_Reports f = new Form_Reports();
            f.Page_num = _new_form_tabindex;
            //f.Show();
            f.ShowDialog();
            //Application.DoEvents();
        }

        private void tempMenuj_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem mi = (ToolStripMenuItem)sender;
  
            string s = mi.Text;
            if ((s == "Login") && (inc.currentuser == -1))
            {
                Form_Loginout form = new Form_Loginout();
                form.MyEnvent += new CallBack_LoadMenu(load_menu);
                form.ShowDialog();
                return;
            }

            inc.db_control.SQL = "select * from tb_menu where fd_name='" + s + "' and fd_delete<>1 order by fd_new_id";
            OdbcDataReader MyReader = inc.db_control.SQLExecuteReader();
            

            if (MyReader != null)
            {
                if (MyReader.Read())
                {
                    string formName = MyReader["fd_formlink"].ToString();
                    
                    if (formName != "")
                    {
                        if (formName == "Form_Loginout")
                        {
                            if (MessageBox.Show("Log Out?", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                inc.currentuser = -1;
                                inc.username = "";
                                load_menu_login();
                            }
                            //Form_Loginout form = null;
                            //Type type = Type.GetType("LTA_PGS_20." + formName);
                            //if (type != null)
                            //{
                            //    form = (Form_Loginout)Activator.CreateInstance(type);
                            //    form.MyEnvent += new CallBack_LoadMenu(load_menu);
                            //    if (MyReader["fd_page"] != DBNull.Value)
                            //        inc.tabpagename = (string)MyReader["fd_page"];

                            //    fn_global.log_operateion(0, "Logout", 1);

                            //    form.ShowDialog();
                            //}                            
                        }
                        else if (formName == "Restart_CCS")
                        {
                            fn_global.log_operateion((int)inc.LOGMSGCODE.RSC01, "", 1);
                            fn_global.fn_reload_ccs();
                            inc.auto_log_off = inc.USER_TIME_OUT;
                        }
                        else if (formName.IndexOf(".")>=0)
                        {
                            System.Diagnostics.Process p1 = null;
                            try
                            {
                                p1 = System.Diagnostics.Process.Start(formName);                                
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(this, ex.Message, "Error");
                            }
                        }
                        else
                        {
                            inc.auto_log_off = inc.USER_TIME_OUT;

                            Form form = null;

                            Type type = Type.GetType("LTA_PGS_20." + formName);
                            if (type != null)
                            {
                                try
                                {
                                    form = (Form)Activator.CreateInstance(type);
                                    if (MyReader["fd_page"] != DBNull.Value)
                                        inc.tabpagename = (string)MyReader["fd_page"];
                                    form.ShowDialog();
                                }
                                catch (Exception ex)
                                {
 
                                }
                            }
                        }

                    }
                }
            }
        }

        private void load_menu()
        {            
            int _index = fn_global.fn_user_id2index(inc.currentuser);
            if (_index >= 0)
            {                    
                //menuStrip2 = new MenuStrip();
                menuStrip2.Items.Clear();

                inc.db_control.SQL = "select * from tb_menu where fd_new_id like '%0000' and fd_delete<>1 and fd_level <='" 
                    + inc.TB_user[_index].level.ToString() + "'order by fd_new_id";
                OdbcDataReader MyReader = inc.db_control.SQLExecuteReader();

                inc.pagelist = new List<string>();

                if (MyReader != null)
                {
                   // int i = 0;
                    while (MyReader.Read())
                    {
                        ToolStripMenuItem tempMenui = new ToolStripMenuItem((string)MyReader["fd_name"]);
                        string _type = ((Int64)MyReader["fd_new_id"]).ToString();

                        inc.db_control.SQL = "select * from tb_menu where fd_new_id like '"
                            + _type.Substring(0, 1) + "%' and fd_delete<>1 and fd_new_id<>'" + _type + "' and fd_level <='"
                    + inc.TB_user[_index].level.ToString() + "' order by fd_new_id";

                        OdbcDataReader MyReader1 = inc.db_control.SQLExecuteReader();
                        bool _valid = false;
                        if (MyReader1 != null)
                        {
                            while (MyReader1.Read())
                            {
                                ToolStripMenuItem tempMenuj = new ToolStripMenuItem((string)MyReader1["fd_name"]);
                                if (MyReader1["fd_page"] != DBNull.Value)
                                {
                                    if ((string)MyReader1["fd_page"] != "")
                                        inc.pagelist.Add((string)MyReader1["fd_page"]);
                                }
                                tempMenuj.Click += new EventHandler(tempMenuj_Click);   
                                tempMenui.DropDownItems.Add(tempMenuj);
                                _valid = true;
                            }
                        }
                        if (!_valid)
                            tempMenui.Click += new EventHandler(tempMenuj_Click);   
                        menuStrip2.Items.Add(tempMenui);
                    }                    
                }
                
                if (inc.TB_user[_index].level > 2)
                    checkBox_edit.Visible = true;
                else
                    checkBox_edit.Visible = false;
                
                check_pwd_timeout();
            }
        }

        private void load_menu_login()
        {
            menuStrip2.Items.Clear(); 
            
            ToolStripMenuItem tempMenui = new ToolStripMenuItem("Login");            
            tempMenui.Click += new EventHandler(tempMenuj_Click);

            menuStrip2.Items.Add(tempMenui);
                        
            checkBox_edit.Visible = false;
        }        

        private void Form_main_Load(object sender, EventArgs e)
        {
            this.Controls.Add(menuStrip2);

            this.Text = System.Configuration.ConfigurationSettings.AppSettings["Application_Name"];

            

            try { inc.USER_TIME_OUT = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["usertimeout"]); }
            catch { }

            label2.Text = "";
            

            fn_global.load_cmss_config();

            fn_global.load_vmss_config();

            fn_global.load_user_table();

            //fn_global.load_area_table();

            fn_global.load_premessage();

            fn_global.load_logmessage();

            inc.currentuser = -1;

            //Form_Loginout f = new Form_Loginout();
            //f.MyEnvent += new CallBack_LoadMenu(load_menu);
            //f.ShowDialog();

            load_menu_login();

            tabControl1.SelectedIndex = -1;
            tabControl1.SelectedIndex = 0;

            lm = new Label();
            lm.BackColor = System.Drawing.Color.LightBlue;
            lm.AutoSize = true;

            hScrollBar1.Value = 0;

            fn_global.load_user_table();

            

            inc.log_off = true;
            //update_alarm();

            if (System.Configuration.ConfigurationSettings.AppSettings["use_format_screen"] == "1")
            {
                this.WindowState = FormWindowState.Normal;
                this.Left = 0;
                this.Top = 0;
            }
            //this.Controls.Add(menuStrip2);
            inc.alarm_check_timer = 14;
        }        

        private void load_area_map(int area_id)
        {            
            fn_global.fn_load_map(area_id, paint_box);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex < 0)
                return;

            paint_box.Size = tabControl1.SelectedTab.Size;            
            //paint_box.mf_clearicon();
            groupBox1.Visible = true;
                        
            if (tabControl1.TabPages[tabControl1.SelectedIndex].Name == "0")
            {
                groupBox1.Visible = false;
                string _map_main = System.Configuration.ConfigurationSettings.AppSettings["Map_singapore"];
                try
                {
                    paint_box.FileName = inc.images_path + "\\temp\\" + _map_main;
                    fn_global.fn_refresh_mainmap(paint_box);
                }
                catch { }
            }
            else
            {                
                for (int i = 0; i < inc.tb_area.Length; i++)
                {
                    if (tabControl1.SelectedTab.Name == inc.tb_area[i].fd_id.ToString())
                    {                        
                        paint_box.FileName = inc.images_path + "\\temp\\" + inc.tb_area[i].map_name;
                        load_area_map(inc.tb_area[i].areaid);
                        udpate_cms_lots();
                        groupBox1.Text = inc.tb_area[i].name;
                        udpate_vmscms_list(inc.tb_area[i].areaid);
                    }
                }
                    /*if (tabControl1.TabPages[tabControl1.SelectedIndex].Name == "TabPages_Marina")
                    {
                        string _map_marina = System.Configuration.ConfigurationSettings.AppSettings["Map_marina"];
                        paint_box.FileName = inc.images_path + "\\temp\\" + _map_marina;
                    }
                    else if (tabControl1.TabPages[tabControl1.SelectedIndex].Name == "TabPages_Orchard")
                    {
                        string _map_orchard = System.Configuration.ConfigurationSettings.AppSettings["Map_orchard"];
                        paint_box.FileName = inc.images_path + "\\temp\\" + _map_orchard;
                    }
                    else if (tabControl1.TabPages[tabControl1.SelectedIndex].Name == "TabPages_Harbfront")
                    {
                        string _map_harbfront = System.Configuration.ConfigurationSettings.AppSettings["Map_Harbfront"];
                        paint_box.FileName = inc.images_path + "\\temp\\" + _map_harbfront;
                    }
                */
                
            }

            paint_box.Flash = true;
            timer2.Enabled = true;
            timer2.Interval = 1000;

            paint_box.form_main = this;
            tabControl1.SelectedTab.Controls.Add(paint_box);
            paint_box.Location = new Point(0, 0);

            picture_location.MouseMove += new MouseEventHandler(picture_move);
            picture_location.MouseDown += new MouseEventHandler(picture_MouseDown);
            picture_location.MouseUp += new MouseEventHandler(picture_MouseUp);
        }

        bool wselected = false;
        Point p = new Point();

        private void picture_MouseDown(object sender, MouseEventArgs e)
        {
            picture_location.Cursor = Cursors.Hand; //按下鼠标时，将鼠标形状改为手型
            wselected = true;
            p.X = e.X;
            p.Y = e.Y;
        }

        int driftX = 0, driftY = 0;
        int mx = 0, my = 0;

        private void picture_move(object sender, MouseEventArgs e)
        {
            if (wselected)
            {
                driftX = p.X - e.X;
                driftY = p.Y - e.Y;

                mx = mx - driftX;
                my = my - driftY;

                Bitmap bm = new Bitmap(this.picture_location.Image);

                Graphics g = picture_location.CreateGraphics();
                g.Clear(picture_location.BackColor);
                g.DrawImage(bm, mx, my);

                p.X = e.X;
                p.Y = e.Y;

                bm.Dispose();
                g.Dispose();
            }
        }

        private void picture_MouseUp(object sender, MouseEventArgs e)
        {
            paint_box.Cursor = Cursors.Default;
            //picture_location.Cursor = Cursors.Default; //松开鼠标时，形状恢复为箭头
            //wselected = false;
        }

        private void _picture_click(object sender, EventArgs e)
        {
            if(Picture_Location_size.Height==0)
                Picture_Location_size = new Size((int)(picture_location.Width * 1.2), (int)(picture_location.Height * 1.2));
            else
                Picture_Location_size = new Size((int)(Picture_Location_size.Width * 1.2), (int)(Picture_Location_size.Height * 1.2));

            Bitmap new_bitmap = new Bitmap(picture_location.Image, Picture_Location_size);

            Point mx = picture_location.PointToClient(Control.MousePosition);
            mx = new Point((int)(mx.X*1.2),(int)(mx.Y*1.2));
            if (mx.X < 400)
                mx.X = 400;
            else if (((800 * 1.2) - mx.X) < 400)
                mx.X = (int)(800 * 1.2 - 400);

            if (mx.Y < 300)
                mx.Y = 300;
            else if (((600 * 1.2) - mx.X) < 300)
                mx.Y = (int)(600 * 1.2 - 300);
            //MessageBox.Show("控件坐标：" + mx.X.ToString() + ":" + mx.Y.ToString());

            
            Bitmap bak = new Bitmap(GetPart(new_bitmap, 0, 0, 800, 600, mx.X - 400, mx.Y - 300));
            //new Bitmap(picture_location.Image, Picture_Location_size);
            picture_location.Image = bak;
        }

        private Bitmap GetPart(Image originalImg
            , int pPartStartPointX, int pPartStartPointY, int pPartWidth
            , int pPartHeight, int pOrigStartPointX, int pOrigStartPointY)
        {
            Bitmap partImg = new Bitmap(pPartWidth, pPartHeight);
            Graphics graphics = Graphics.FromImage(partImg);
            Rectangle destRect = new Rectangle(new Point(pPartStartPointX, pPartStartPointY), new Size(pPartWidth, pPartHeight));//目标位置
            Rectangle origRect = new Rectangle(new Point(pOrigStartPointX, pOrigStartPointY), new Size(pPartWidth, pPartHeight));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）

            graphics.DrawImage(originalImg, destRect, origRect, GraphicsUnit.Pixel);
            return partImg;
        }

        private void globalParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _new_form_tabindex = 6;
            load_Management_form();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            paint_box.Zoom = paint_box.Zoom * 0.90 > 0.5 ? paint_box.Zoom * 0.90: paint_box.Zoom;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            paint_box.Zoom = paint_box.Zoom * 0.90 < 2.0 ? paint_box.Zoom / 0.90 : paint_box.Zoom;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (paint_box != null)
            {
                paint_box.mf_flash();
            }
            label3.Text = "User: " + inc.username;
            if (label2.Text == "ALARM")
                label2.Visible = !label2.Visible;

            if (!inc.log_off)
            {
                if (inc.auto_log_off > 0)
                    inc.auto_log_off--;
                if (inc.auto_log_off <= 0)
                {
                    inc.log_off = true;

                    inc.currentuser = -1;
                    load_menu_login();                        
                }
            }
        }

        private void show_adhoc_message_form(int index)
        {
            Form_Adhoc_condig f = new Form_Adhoc_condig();
            f.load_from_main = true;
            f.gdm_index = index;
            f.ShowDialog();
        }

        public void close_current_map_info()
        {
            lm.Visible = false;
        }

        public void show_current_map_info(Point p)
        {
            try
            {
                int x = p.X;
                int y = p.Y;

                lm.Left = x;
                lm.Top = y + 10;
                lm.Font = new Font("simsun", 10);
                int _space = 30;

                if (tabControl1.SelectedIndex == 0)
                {
                    for (int i = 0; i < inc.tb_area.Length; i++)
                    {
                        if ((x > (inc.tb_area[i].x)) && (x < (inc.tb_area[i].x + inc.tb_area[i].map_size.Width)) && (y > (inc.tb_area[i].y)) && (y < inc.tb_area[i].y + inc.tb_area[i].map_size.Height))
                        {
                            tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Add(lm);

                            lm.Text = "AREA ID     :" + inc.tb_area[i].areaid + "\n";
                            lm.Text += "AREA        :" + inc.tb_area[i].name;
                            lm.Visible = true;
                            lm.BringToFront();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < inc._vms.Length; i++)
                    {
                        if ((x > (inc._vms[i].x)) && (x < (inc._vms[i].x + _space)) && (y > (inc._vms[i].y)) && (y < inc._vms[i].y + _space))
                        {
                            if (inc._vms[i].area == tabControl1.SelectedIndex)
                            {
                                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Add(lm);
                                lm.Text = "VMS ID    :" + inc._vms[i]._id + "\n";
                                lm.Text += "VMS       :" + inc._vms[i].name + "\n";
                                try { lm.Text += "Flash Mode:" + inc.VMS_Flash_mode[inc._vms[i].flash - 1].ToString() + "\n"; }
                                catch { }
                                lm.Text += "Timer     :" + inc._vms[i].timer.ToString() + "\n";
                                lm.Visible = true;

                                lm.Left -= ((lm.Left + lm.Width) > tabControl1.Width) ? ((lm.Left + lm.Width) - tabControl1.Width) : 0;
                                lm.Top -= ((lm.Top + lm.Height) > tabControl1.Height) ? ((lm.Top + lm.Height) - tabControl1.Height) : 0;

                                lm.BringToFront();

                            }
                        }
                    }

                    for (int i = 0; i < inc.Carpark.Length; i++)
                    {
                        if ((x > (inc.Carpark[i].map_point.X)) && (x < (inc.Carpark[i].map_point.X + _space)) && (y > (inc.Carpark[i].map_point.Y)) && (y < inc.Carpark[i].map_point.Y + _space))
                        {
                            if (inc.Carpark[i].area == tabControl1.SelectedIndex)
                            {
                                tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Add(lm);
                                lm.Text = "CMS       :" + inc.Carpark[i].id + "\n";
                                lm.Text += "CMS       :" + inc.Carpark[i].name + "\n";
                                lm.Text += "LOTS      :" + inc.Carpark[i].lots + "\n";
                                lm.Text += "LIMIT     :" + inc.Carpark[i].limit;
                                lm.Visible = true;

                                lm.Left -= ((lm.Left + lm.Width) > tabControl1.Width) ? ((lm.Left + lm.Width) - tabControl1.Width + 20) : 0;
                                lm.Top -= ((lm.Top + lm.Height) > tabControl1.Height) ? ((lm.Top + lm.Height) - tabControl1.Height + 20) : 0;

                                lm.BringToFront();
                            }
                        }
                    }

                }
            }
            catch { }
        }

        private void Confirm_station()
        {
            if (tabControl1.TabPages[tabControl1.SelectedIndex].Name == "Main")
            {
                fn_global.fn_refresh_mainmap(paint_box);
            }
            else
            {
                for (int i = 0; i < inc.tb_area.Length; i++)
                {
                    if (tabControl1.SelectedTab.Text == inc.tb_area[i].name)
                        fn_global.fn_load_map(inc.tb_area[i].areaid, paint_box);
                }
            }
        }

        public void load_new_page()
        {
            int x = paint_box.m_double_click.X;
            int y = paint_box.m_double_click.Y;

            int _space = 30;
            int _index = fn_global.fn_user_id2index(inc.currentuser);
            if (_index >= 0)
            {
                if ((inc.TB_user[_index].level > 2) && (checkBox_edit.Checked))
                {
                    Form_VMSCMS_set f = new Form_VMSCMS_set();
                    f.MyEnvent += new CallBack_Station(Confirm_station);
                    f.New_Location.X = x;
                    f.New_Location.Y = y;
                    if (tabControl1.SelectedIndex == 0)
                        f.main_page = -1;
                    else
                        f.main_page = tabControl1.SelectedIndex - 1;
                    f.ShowDialog();
                }
                else
                {
                    if (tabControl1.SelectedIndex == 0)
                    {
                        for (int i = 0; i < inc.tb_area.Length; i++)
                        {
                            if ((x > (inc.tb_area[i].x)) && (x < (inc.tb_area[i].x + inc.tb_area[i].map_size.Width)) && (y > (inc.tb_area[i].y)) && (y < inc.tb_area[i].y + inc.tb_area[i].map_size.Height))
                            {
                                tabControl1.SelectedIndex = i + 1;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < inc._vms.Length; i++)
                        {
                            if ((x > (inc._vms[i].x)) && (x < (inc._vms[i].x + _space)) && (y > (inc._vms[i].y)) && (y < inc._vms[i].y + _space))
                            {
                                if (inc._vms[i].area == tabControl1.SelectedIndex)
                                {
                                    show_adhoc_message_form(i);
                                }
                            }
                        }

                        for (int i = 0; i < inc.Carpark.Length; i++)
                        {
                            if ((x > (inc.Carpark[i].map_point.X)) && (x < (inc.Carpark[i].map_point.X + _space)) && (y > (inc.Carpark[i].map_point.Y)) && (y < inc.Carpark[i].map_point.Y + _space))
                            {
                                if (inc.Carpark[i].area == tabControl1.SelectedIndex)
                                {
                                    //   /show_cms_status(i);
                                    //MessageBox.Show("123213131213");
                                }
                            }
                        }

                    }
                }
            }
            else
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    for (int i = 0; i < inc.tb_area.Length; i++)
                    {
                        if ((x > (inc.tb_area[i].x)) && (x < (inc.tb_area[i].x + inc.tb_area[i].map_size.Width)) && (y > (inc.tb_area[i].y)) && (y < inc.tb_area[i].y + inc.tb_area[i].map_size.Height))
                        {
                            tabControl1.SelectedIndex = i + 1;
                        }
                    }
                }
            }
        }

        private void Form_main_Shown(object sender, EventArgs e)
        {
            
        }

        private void udpate_cms_lots()
        {            
            try
            {
                string file_name = "alarmstatus.txt";
                string _workdir = fn_global.get_workdir("");

                if (!fn_global.fn_get_file("", file_name, file_name))
                    return;

                string str = "";
                if (System.Configuration.ConfigurationSettings.AppSettings["UseRemoteDisk"] == "1")
                {
                    File.Copy((_workdir + "\\" + file_name), "C:\\alarmstatus.txt", true);
                    MyFile mf = new MyFile("C:\\alarmstatus.txt");
                    str = mf.read_file_all();
                    mf.close();
                }
                else
                {
                    MyFile mf = new MyFile(_workdir + "\\" + file_name);
                    str = mf.read_file_all();
                    mf.close();
                }
                string[] list_str = str.Split('\n');

                for (int i = 0; i < inc.Carpark.Length; i++)
                {
                    string s_key = "lots_cms" + inc.Carpark[i].id;

                    string _temp = fn_global.fn_txt_getvalue(list_str, s_key);
                    try { inc.Carpark[i].lots = int.Parse(_temp); }
                    catch { }
                    
                    s_key = "lotslimit_cms" + inc.Carpark[i].id;
                    _temp = fn_global.fn_txt_getvalue(list_str, s_key);
                    try { inc.Carpark[i].limit = int.Parse(_temp); }
                    catch { }
                }

            }
            catch { }
        }

        private void udpate_vmscms_list( int area_id)
        {
            try
            {
                listBox1.Items.Clear();
                for (int i = 0; i < inc.Carpark.Length; i++)
                {
                    if (inc.Carpark[i].area == area_id)
                    {
                        string _name = inc.Carpark[i].name.PadRight(16, ' ');

                        if (inc.Carpark[i].status == -1)
                        {                            
                            listBox1.Items.Add(_name + ":" + inc.Carpark[i].lots.ToString());
                        }
                        else
                            listBox1.Items.Add(_name.Remove(15) + ":Disconnect");
                    }
                }
            }
            catch { }
        }

        private void update_alarm()
        {
            inc.db_control.SQL = "select * from tb_alarm where fd_delete<>1 and fd_sub_msgid=1099";
            OdbcDataReader MyReader = inc.db_control.SQLExecuteReader();

            if (MyReader != null)
                inc.server_status_ok = !MyReader.Read();

            inc.db_control.SQL = "select * from tb_alarm where fd_delete<>1 order by fd_id";
            MyReader = inc.db_control.SQLExecuteReader();

            if (MyReader != null)
            {
                inc.databaseerror = true;
                if (MyReader.Read())
                {
                    label2.Text = "ALARM";
                    if (System.Configuration.ConfigurationSettings.AppSettings["alarm_sound"] == "1")
                        fn_global.fn_alarm_sound();
                    return;
                }
            }
            else
            {
                if (inc.databaseerror)                    
                {
                    inc.databaseerror = false;
                    //MessageBox.Show("DataBase Connect error,please restart the magagerment software");                    
                    if (MessageBox.Show("DataBase disconnected,close the pgs software.(please reactivate pgs software)", "DataBase disconnected", MessageBoxButtons.OK) == DialogResult.OK) 
                    {
                        Application.Exit();
                    }
                    //MessageBoxOptions.
                }

            }
            label2.Text = "";
        }

        private void timer_alarm_Tick(object sender, EventArgs e)
        {
            try
            {
                inc.alarm_check_timer++;
                if (inc.alarm_check_timer >= 15)
                {
                    inc.alarm_check_timer = 0;
                    try { fn_global.fn_load_map(inc.tb_area[tabControl1.SelectedIndex - 1].areaid, paint_box); }
                    catch { }
                    for (int i = 0; i < inc._vms.Length; i++)
                    {
                        inc._vms[i].status = fn_global.get_status(inc._vms[i]._id);
                    }
                    for (int i = 0; i < inc.Carpark.Length; i++)
                    {
                        inc.Carpark[i].status = fn_global.get_status(10000 + inc.Carpark[i].id);
                    }

                    update_alarm();
                    udpate_cms_lots();
                    try
                    {
                        udpate_vmscms_list(inc.tb_area[tabControl1.SelectedIndex - 1].areaid);
                    }
                    catch { }
                    if (tabControl1.SelectedIndex == 0)
                    {
                        fn_global.fn_refresh_mainmap(paint_box);
                    }

                    string _location_icon_ea_green = System.Configuration.ConfigurationSettings.AppSettings["Icon_ea"];
                    string _location_icon_ea_red = System.Configuration.ConfigurationSettings.AppSettings["Icon_ea_red"];

                    string file = inc.images_path + "\\";
                    file += (fn_global.check_ea_status() == true ? _location_icon_ea_green : _location_icon_ea_red);

                    pictureBox6.Image = new Bitmap(Image.FromFile(file), pictureBox6.Size);

                    string _location_icon_firewall_green2 = System.Configuration.ConfigurationSettings.AppSettings["Icon_firewall2"];
                    string _location_icon_firewall_red2 = System.Configuration.ConfigurationSettings.AppSettings["Icon_firewall_red2"];

                    string _location_icon_firewall_green1 = System.Configuration.ConfigurationSettings.AppSettings["Icon_firewall1"];
                    string _location_icon_firewall_red1 = System.Configuration.ConfigurationSettings.AppSettings["Icon_firewall_red1"];

                    file = inc.images_path + "\\";
                    file += (fn_global.check_firewall_status(1) == true ? _location_icon_firewall_green1 : _location_icon_firewall_red1);
                    pictureBox7.Image = new Bitmap(Image.FromFile(file), pictureBox6.Size);

                    file = inc.images_path + "\\";
                    file += (fn_global.check_firewall_status(2) == true ? _location_icon_firewall_green2 : _location_icon_firewall_red2);
                    pictureBox8.Image = new Bitmap(Image.FromFile(file), pictureBox6.Size);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Tools.write_log(ex.Message);
            }

        }

        private void f_Close(object sender, EventArgs e)
        {
            ((Button)sender).FindForm().Close();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            
        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            double _value = 0 - (double)hScrollBar1.Value;

            if ((double)hScrollBar1.Value == 0)
                paint_box.Zoom = 1.0;
            else
            {                
                paint_box.Zoom = Math.Pow(0.9, _value);
            }            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string _temp = "";
            _temp += "VMS Power supply failure\n";
            _temp += "VMS Scan board failure\n";
            _temp += "VMS Photo sensor failure\n";
            _temp += "VMS Pixel failure\n";
            _temp += "VMS LED time error";

            Form f = new Form();
            f.Size = new Size(300, 200);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.MaximizeBox = false;
            f.MinimizeBox = false;
            GroupBox g1 = new GroupBox();
            g1.Text = "List of VMS Faults";
            g1.Size = new Size(270, 150);
            g1.Left = 8;
            g1.Top = 3;
            f.Controls.Add(g1);

            Label l1 = new Label();
            l1.Text = _temp;
            l1.Left = 10;
            l1.Font = new Font(this.Font.FontFamily.Name, 10);
            l1.Top = 30;
            l1.AutoSize = true;
            g1.Controls.Add(l1);

            Button b1 = new Button();
            b1.Text = "OK";

            b1.Top = l1.Top + l1.Height + 10;
            b1.Left = l1.Left + 80;
            g1.Controls.Add(b1);
            b1.Click += new EventHandler(f_Close);

            f.ShowDialog();
        }

        private void check_pwd_timeout()
        {
            inc.db_control.SQL = "select fd_password_time,fd_lasttime from tb_users where fd_id='" + inc.currentuser.ToString() + "'";
            OdbcDataReader MyReader = inc.db_control.SQLExecuteReader();
            if (MyReader != null)
            {
                if (MyReader.Read())
                {             
                    DateTime t_pwd = (DateTime)MyReader["fd_password_time"];
                    int _days = (DateTime.Now - t_pwd).Days;
                    if (_days > 90)
                    {
                        MessageBox.Show("Password expired,please update");
                        return;
                    }
                    if (_days > 80)
                    {
                        string _temp = (90-_days).ToString();
                        MessageBox.Show("Password will be expired after " + _temp + " Days,please update");
                        return;
                    }                    
                }
            }            
        }
    }
}

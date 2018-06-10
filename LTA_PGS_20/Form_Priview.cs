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
    public partial class Form_Priview : Form
    {
        public struct show_msg_one_row
        {
            public string msg;
            public bool iscms; //true-cms false-msg
            public int color;//cms color            
            public bool used;
        }

        public show_msg_one_row[,] msg_pages = new show_msg_one_row[3, 6];

        public int[,] msg11 = new int[3, 6];

        public int mode;
        public int timer;

        private bool ispattern;
        public bool IsPattern
        {
            get { return ispattern; }
            set { ispattern = value; }
        }

        private int run_timer = 0;
        private int run_page = 0;

        private int last_page = 0;//only used for pattern 

        private int len_step = 1;
        public Form_Priview()
        {
            InitializeComponent();
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void write_image()
        {
            //Graphics g = Graphics.FromImage(bmp);

            //System.Drawing.Drawing2D.LinearGradientBrush brush
            //    = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, bmp.Width, bmp.Height), Color.Black, Color.Black, 1.0f, true);


            //StringFormat sf = new StringFormat();
            //sf.Alignment = StringAlignment.Center;
            //sf.LineAlignment = StringAlignment.Center;

            //g.DrawString(text, f, brush, bmp.Width / 2, bmp.Height / 2, sf);
            //g.Save();

            //return bmp;
        }

        private void get_cur_page()
        {
            bool pagevalue = false;
            int loop = 0;
            while (!pagevalue)
            {
                if (ispattern)
                {
                    if(run_page==2)
                        last_page = last_page == 0 ? 1 : 0;
                    run_page = run_page == 2 ? last_page : 2;
                }
                else
                    run_page++;

                if (run_page > 2)
                {
                    run_page = 0;
                }
                loop++;

                for (int i = 0; i < 6; i++)
                {
                    pagevalue = msg_pages[run_page, i].used;
                    if (pagevalue)
                        break;
                }
                if (pagevalue)
                    break;

                if (loop > 2)
                {
                    if (!pagevalue)
                    {
                        run_page = -1;
                        break;
                    }
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (run_timer <= 0)
            {
                get_cur_page();
                if (run_page > -1)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (msg_pages[run_page, i].used)
                        {
                            ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Text = msg_pages[run_page, i].msg;
                            ((Label)this.Controls.Find("label" + (i + 1), true)[0]).ForeColor = inc.VMS_Color[msg_pages[run_page, i].color];
                        }
                    }

                    run_timer = timer;
                }
            }
            else
                run_timer--;
        }

        private void Form_Priview_Load(object sender, EventArgs e)
        {
            run_timer = timer;
            run_page = -1;
            if (mode == 0)
                timer_toggle.Enabled = true;
            else if (mode == 2)
            {
                timer_toggle.Enabled = true;
                timer_flash.Enabled = true;
            }
            else if (mode == 1)
            {
                int _w = 0;
                for (int i = 6; i < 12; i++)
                {
                    ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Left = 0 - ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Width;
                    _w = ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Width;
                }
                for (int i = 0; i < 6; i++)
                {
                    ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Left = 0;
                }
                timer_scroll.Enabled = true;

                len_step = (_w / run_timer) / 10;

            }
            for (int i = 0; i < 6; i++)
            {
                ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Text = "";
            }

            bool valid = false;
            for (int j = 0; j < 3; j++)
            {
                if (valid)
                    break;
                for (int i = 0; i < 6; i++)
                {                
                    if (msg_pages[j, i].used)
                    {
                        ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Text = msg_pages[j, i].msg;
                        ((Label)this.Controls.Find("label" + (i + 1), true)[0]).ForeColor = inc.VMS_Color[msg_pages[j, i].color];
                        run_page = j;
                        valid = true;
                    }
                }
            }
        }

        private void timer_flash_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                string _temp=((Label)this.Controls.Find("label" + (i + 1), true)[0]).Text;
                if (_temp.IndexOf("          ") <0)
                {
                    string _space ="                                            ";
                    ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Text = _space + _temp;
                }
                else
                {
                    ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Text = _temp.Trim();
                }
            }
        }

        private void timer_scroll_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                int page2 = i + 6;

                if ((((Label)this.Controls.Find("label" + (i + 1), true)[0]).Left + ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Width) <= 0)
                {
                    if (i == 0)
                    {
                        get_cur_page();
                    }
                    int line = i;
                    if (msg_pages[run_page, line].used)
                    {
                        ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Text = msg_pages[run_page, line].msg;
                        ((Label)this.Controls.Find("label" + (i + 1), true)[0]).ForeColor = inc.VMS_Color[msg_pages[run_page, line].color];
                    } 
                    ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Left = ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Width;
                    
                }
                if ((((Label)this.Controls.Find("label" + (page2 + 1), true)[0]).Left + ((Label)this.Controls.Find("label" + (page2 + 1), true)[0]).Width) <= 0)
                {
                    if (i == 0)
                    {
                        get_cur_page();
                    }
                    int line = i;
                    if (msg_pages[run_page, line].used)
                    {
                        ((Label)this.Controls.Find("label" + (page2 + 1), true)[0]).Text = msg_pages[run_page, line].msg;
                        ((Label)this.Controls.Find("label" + (page2 + 1), true)[0]).ForeColor = inc.VMS_Color[msg_pages[run_page, line].color];
                    }
                    ((Label)this.Controls.Find("label" + (page2 + 1), true)[0]).Left = ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Width;
                }

                int a = ((Label)this.Controls.Find("label" + (page2 + 1), true)[0]).Left;
                int _w = ((Label)this.Controls.Find("label" + (page2 + 1), true)[0]).Width;
                if (a > 0)
                {
                    ((Label)this.Controls.Find("label" + (page2 + 1), true)[0]).Left = ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Left + _w - len_step;
                    ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Left -= len_step;                    
                }
                else
                {
                    ((Label)this.Controls.Find("label" + (i + 1), true)[0]).Left = ((Label)this.Controls.Find("label" + (page2 + 1), true)[0]).Left + _w - len_step;   
                    ((Label)this.Controls.Find("label" + (page2 + 1), true)[0]).Left -= len_step;                    
                }

                
            }
        }

        private void timer_page_scroll_Tick(object sender, EventArgs e)
        {
            
        }
    }
}

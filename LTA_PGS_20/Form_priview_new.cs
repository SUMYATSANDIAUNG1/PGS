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
    public partial class Form_priview_new : Form
    {
        public int timer;
        public int mode;

        private int run_page = 0;
        private int run_timer = 0;

        private int last_page = 0;//only used for pattern
        private bool ispattern;
        public bool IsPattern
        {
            get { return ispattern; }
            set { ispattern = value; }
        }
        

        public struct show_msg_one_row
        {
            public string msg;
            public int color;
            public bool used;
        }
        
        public show_msg_one_row[,] msg_pages = new show_msg_one_row[3, 6];

         

        public Form_priview_new()
        {
            InitializeComponent();
        }

        private void get_cur_page()
        {
            bool pagevalue = false;
            int loop = 0;
            while (!pagevalue)
            {
                if (ispattern)
                {
                    if (run_page == 2)
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
        private void show()
        {
            for (int l = 1; l < 7; l++)
            {
                ((RichTextBox)this.Controls.Find("richTextBox" + l, true)[0]).Text = "";
            }
            try
            {
                Color c_c = inc.VMS_Color[0];

                for (int l = 1; l < 7; l++)
                {                    
                    string _msg = "";
                    if (msg_pages[run_page, (l - 1)].used)
                        _msg = msg_pages[run_page, (l - 1)].msg;

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
            catch { }            
        }

        private void Form_priview_new_Load(object sender, EventArgs e)
        {
            button3.Focus();
            run_timer = timer;
            timer_toggle.Enabled = true;
            run_page = 2;
            get_cur_page();
            show();
            button3.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timer_toggle_Tick(object sender, EventArgs e)
        {
            if (run_timer <= 0)
            {
                get_cur_page();
                if (run_page > -1)
                {
                    show();
                    button3.Focus();
                    run_timer = timer;
                }
            }
            else
                run_timer--;
        }
    }
}

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
    public partial class Form_3Pages_Diplay : Form
    {
        public string Rec_Message;

        public Form_3Pages_Diplay()
        {
            InitializeComponent();
        }

        private void Form_3Pages_Diplay_Load(object sender, EventArgs e)
        {
            string[] str_s = Rec_Message.Split('\n');


            string _toggle = str_s[1].Substring(str_s[1].IndexOf("=") + 1);
            _toggle = _toggle.Remove(_toggle.Length - 1);
            comboBox_flash_mode.Text = _toggle;

            Color c_c = inc.VMS_Color[0];


            for (int l = 1; l < 7; l++)
            {
                string _msg = str_s[(l * 3)].Substring(str_s[(l * 3)].IndexOf("=") + 1);
                _msg = _msg.Remove(_msg.Length - 1);

                string[] _msgs = _msg.Split('|');
                for (int p = 0; p < _msgs.Length; p++)
                {
                    _msg = _msgs[p];


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
                            ((RichTextBox)this.Controls.Find("richTextBox" + ((p * 6) + l), true)[0]).Text += _msg[i];
                        }
                    }

                    bool f = true;
                    for (int s = 9; s >= 0; s--)
                    {
                        if (_s_len[s] < 0)
                        {
                            continue;
                        }

                        int _len1 = ((RichTextBox)this.Controls.Find("richTextBox" + ((p * 6) + l), true)[0]).Text.Length;
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
                        ((RichTextBox)this.Controls.Find("richTextBox" + ((p * 6) + l), true)[0]).Select(len, _len1 - len);
                        ((RichTextBox)this.Controls.Find("richTextBox" + ((p * 6) + l), true)[0]).SelectionColor = _s_clolr[s];
                    }
                }
            }
        }
    }
}

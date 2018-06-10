using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LTA_PGS_20
{
    public partial class f_paintbox : UserControl
    {

        private System.Drawing.Bitmap m_bitmap,m_bitmap_back;
        //private wl_icon m_icons=null;
        List<wl_icon> m_icons = new List<wl_icon>();        

        private double m_zoom = 1.0;
        private string m_filename = "";
        private Point m_point_down,m_scroll_point;
        private bool m_mouse_down = false;
        private bool m_flash = false;
        private bool m_flash_off = false;

        public Form_main form_main = null;
        public Point m_double_click;

        private Point Hover_point;
        private long lastMouseMoveTime = 0;

        public void mf_clearicon()
        {
            try
            {
                m_icons.Clear();
            }
            catch
            {
                
            }

        }

        public void mf_updateicon_text(string text,int id)
        {
            if (m_icons.Count > 0)
            {
                foreach (wl_icon i1 in m_icons)
                {
                    if (i1.id == id)
                    {
                        i1.m_text = text;
                        break;
                    }
                }
            }
        }

        public void mf_addicon(Int32 a_x, Int32 a_y, string a_filename,bool flash,int id,bool realsize)
        {
            wl_icon i1 = new wl_icon();
            i1.m_x = a_x;
            i1.m_y = a_y;
            try
            {
                if(realsize)
                    i1.m_bitmap = (Bitmap)Bitmap.FromFile(a_filename, false);                    
                else
                    i1.m_bitmap = new Bitmap(Image.FromFile(a_filename), new Size(35,30));
                i1.m_filename = a_filename;
                i1.m_bitmap.MakeTransparent(Color.White);
                i1.m_flash = flash;
                i1.id = id;
                //Graphics g = Graphics.FromImage(this.m_bitmap);
                //g.DrawImage(i1.m_bitmap, i1.m_x,i1.m_y);

                m_icons.Add(i1);
            }
            catch
            {
                i1.m_filename = "";
            }
              
        }
        public double Zoom
        {
            get { return m_zoom; }
            set
            {
                if(m_zoom!=value)
                {
                    m_zoom = value;
                    this.AutoScrollMinSize = new Size((int)(m_bitmap.Width * m_zoom), (int)(m_bitmap.Height * m_zoom));
                    Invalidate();
                }
            }
        }
        public bool Flash
        {
            get { return m_flash; }
            set
            {
                if (m_flash != value)
                {
                    m_flash = value;
                    m_flash_off = false;
                    Invalidate();
                }
            }
        }
        public string FileName
        {
            get { return m_filename; }
            set
            {
                if (m_filename != value)
                {
                    m_filename = value;
                    try
                    {
                    m_bitmap = (Bitmap)Bitmap.FromFile(m_filename, false);                    
				    this.AutoScroll = true;                    
				    this.AutoScrollMinSize = new Size ((int)(m_bitmap.Width * m_zoom), (int)(m_bitmap.Height * m_zoom));
                    m_scroll_point = new Point(0, 0);
                    m_bitmap_back = m_bitmap.Clone(new Rectangle(0, 0, m_bitmap.Width, m_bitmap.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    }
                    catch
                    {
                        m_bitmap = new Bitmap(2, 2);
                        m_filename = "";
                    }
                    Invalidate();
                }
            }
        }

        public f_paintbox()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                            ControlStyles.AllPaintingInWmPaint, true);
            m_bitmap = new Bitmap(2, 2);
            this.AutoScroll = true;
            m_scroll_point = new Point(0, 0);
            Hover_point = new Point(0,0);
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            base.OnPaint(pe);
            g.DrawImage(m_bitmap, new Rectangle(this.AutoScrollPosition.X, this.AutoScrollPosition.Y, (int)(m_bitmap.Width * m_zoom), (int)(m_bitmap.Height * m_zoom)));

            
        }

        public Point GetVirtualMouseLocation(MouseEventArgs e)
        {
            float f1=(float)m_zoom;
            Matrix mx = new Matrix(f1, 0, 0, f1, 0, 0);
            //pans it according to the scroll bars
            mx.Translate(this.AutoScrollPosition.X * (1.0f / (float)m_zoom), this.AutoScrollPosition.Y * (1.0f / (float)m_zoom));
            //inverts it
            mx.Invert();

            //uses it to transform the current mouse position
            Point[] pa = new Point[] { new Point(e.X, e.Y) };
            mx.TransformPoints(pa);

            return pa[0];
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            int locationx = AutoScrollPosition.X;
            int locationy = AutoScrollPosition.Y;
            int ex = (int)((e.X - locationx)/m_zoom);
            int ey = (int)((e.Y - locationy)/ m_zoom);

            //int ex = e.X;
            //int ey = e.Y;
            m_double_click = new Point(ex, ey);
            form_main.load_new_page();
        }


        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (m_mouse_down == false)
            {
                this.Cursor = Cursors.Hand;
                m_point_down = new Point(e.X, e.Y);
                m_mouse_down = true;
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (m_mouse_down == true)
            {
                this.Cursor = Cursors.Default;
                m_point_down = new Point(0,0);
                m_scroll_point = new Point(-this.AutoScrollPosition.X, -this.AutoScrollPosition.Y);
                m_mouse_down = false;
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (m_mouse_down == true)
            {

                Point p1 = new Point( -e.X + m_point_down.X,  -e.Y + m_point_down.Y);

                this.AutoScrollPosition = new Point(p1.X + m_scroll_point.X, p1.Y + m_scroll_point.Y);
//                string str;
  //              str = string.Format("{0},{1},{2},{3},{4},{5}", AutoScrollPosition.X, AutoScrollPosition.Y, m_scroll_point.X, m_scroll_point.Y, p1.X, p1.Y);
    //            Program.g_main.mf_setlabel(str);
                Invalidate();
            }
        }
        public void mf_flash()
        {
            if (m_flash)
            {
                //m_bitmap = m_bitmap_back;
                m_bitmap = m_bitmap_back.Clone(new Rectangle(0, 0, m_bitmap_back.Width, m_bitmap_back.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                if (m_flash_off)
                {
                    m_flash_off = false;
                    if (m_icons.Count > 0)
                    {
                        Graphics g = Graphics.FromImage(this.m_bitmap);

                        foreach (wl_icon i1 in m_icons)
                        {
                            //if (i1.m_flash)
                            //{
                                g.DrawImage(i1.m_bitmap, i1.m_x, i1.m_y);                                
                                if (i1.m_text != "")
                                {
                                    Font font1 = new Font("Arial", 12, FontStyle.Bold);
                                    StringFormat sf = new StringFormat();
                                    System.Drawing.Drawing2D.LinearGradientBrush brush =
                               new System.Drawing.Drawing2D.LinearGradientBrush(
                                   new Rectangle(0, 0, i1.m_bitmap.Width, i1.m_bitmap.Height),
                                   Color.Black, Color.Red, 2.5f, true);                                    
                                    g.DrawString(i1.m_text, font1, brush, i1.m_x, i1.m_y + i1.m_bitmap.Height, sf);
                                }
                            //}
                        }

                    }
                }
                else
                {
                    if (m_icons.Count > 0)
                    {
                        Graphics g = Graphics.FromImage(this.m_bitmap);

                        foreach (wl_icon i1 in m_icons)
                        {
                            if (!i1.m_flash)
                            {                                
                                g.DrawImage(i1.m_bitmap, i1.m_x, i1.m_y);
                                if (i1.m_text != "")
                                {
                                    Font font1 = new Font("Arial", 12, FontStyle.Bold);
                                    StringFormat sf = new StringFormat();
                                    System.Drawing.Drawing2D.LinearGradientBrush brush =
                               new System.Drawing.Drawing2D.LinearGradientBrush(
                                   new Rectangle(0, 0, i1.m_bitmap.Width, i1.m_bitmap.Height),
                                   Color.Black, Color.Red, 2.5f, true);                                    
                                    g.DrawString(i1.m_text, font1, brush, i1.m_x, i1.m_y + i1.m_bitmap.Height, sf);
                                }
                            }
                        }

                    }
                    m_flash_off = true;
                }

                Invalidate();
            }
        }

        private void f_paintbox_MouseHover(object sender, EventArgs e)
        {
            System.Drawing.Point nowPoint = PointToClient(MousePosition);
            
            lastMouseMoveTime = DateTime.Now.Ticks;
            Hover_point = nowPoint;

            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            System.Drawing.Point nowPoint = PointToClient(MousePosition);
            long _now = DateTime.Now.Ticks;
            if ((Math.Abs(Hover_point.X - nowPoint.X) < 5) && (Math.Abs(Hover_point.Y - nowPoint.Y) < 5))
            {
                if (_now - lastMouseMoveTime >= 10000)
                {
                    form_main.show_current_map_info(nowPoint);
                    timer1.Enabled = false;
                }
            }            
        }

        private void f_paintbox_MouseMove(object sender, MouseEventArgs e)
        {
            System.Drawing.Point nowPoint = PointToClient(MousePosition);
            if ((Math.Abs(Hover_point.X - nowPoint.X) >= 5) || (Math.Abs(Hover_point.Y - nowPoint.Y) >= 5))
            {                
                form_main.close_current_map_info();
                

                lastMouseMoveTime = DateTime.Now.Ticks;
                Hover_point = nowPoint;

                timer1.Enabled = true;
            } 
        }

    }
    public class wl_icon
    {
        public string m_filename;
        public Int32 m_x, m_y;
        public Bitmap m_bitmap;
        public bool m_flash;
        public string m_text;
        public int id;
    }
}

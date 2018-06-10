using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Runtime.Serialization;
using System.Data;
using System.Security.Cryptography;
using System.Drawing.Printing;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Globalization;

namespace LTA_PGS_20
{
    class Tools
    {
        
        public static bool _open_log = false;
        public static StreamWriter _streamWriter;

        public static bool fn_check_bit(int check_num, int base_num)
        {
            if ((check_num & base_num) == check_num)
                return true;
            else
                return false;
        }
        public static string get_value(object a)
        {
            if (a == null)
                return "";
            else
                return a.ToString();
        }
        public static char[] str_to_array(string a_str)
        {
            char[] bt = new char[a_str.Length];
            int n = 0;
            for (int i = 0; i < a_str.Length; i += 1)
            {
                string s = a_str.Substring(i, 1);
                int a = (int)Convert.ToChar(s);
                byte bt1 = Byte.Parse(a.ToString(), System.Globalization.NumberStyles.Integer);
                bt[n] = (char)bt1;
                n++;
            }
            return bt;
        }

        public static string array_to_str(char[] a_char, int len)
        {
            string s = "";
            if (a_char == null)
                return s;
            for (int i = 0; i < len; i++)
            {
                s += a_char[i].ToString();
            }
            return s;
        }       

        public static string bytes_to_str(byte[] a_char, int len)
        {
            string s = "";
            char[] c_str = new char[len];

            for (int i = 0; i < len; i++)
            {
                c_str[i] = (char)a_char[i];
                s += c_str[i].ToString();
            }
            return s;
        }

        public static char[] bytes_to_char(byte[] a_char, int len)
        {
            char[] bt = new char[len];

            for (int i = 0; i < len; i++)
            {
                bt[i] = (char)a_char[i];
            }
            return bt;
        }
        public static bool check_num(string Check_String)
        {            
            Regex regex = new Regex("^[0-9]*[1-9][0-9]*$");            
            if (Check_String != null && regex.IsMatch(Check_String))
            {
                return true;
            }
            else
            {
                return false;
            }            
        }

        public static void write_log(string Msg)
        {
            try
            {
                string _filename = DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!_open_log)
                {
                    _streamWriter = new StreamWriter(System.Environment.CurrentDirectory + "\\log\\" + _filename, true);
                    _open_log = true;
                }


                _streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                _streamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + Msg);
                _streamWriter.Flush();
            }
            catch { }
        }
        public static string add_space(bool Start, int Len, string Str)
        {
            string _re_str = "";
            if (Start)
            {
                for (int i = 0; i < Len - Str.Length; i++)
                {
                    _re_str += " ";
                }
                _re_str += Str;
            }
            else
            {
                _re_str += Str;
                for (int i = 0; i < Len - Str.Length; i++)
                {
                    _re_str += " ";
                }
            }
            return _re_str;
        }
        /// <summary>
        /// string to hexstr
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string str_to_hexstr(string Str_Value)
        {
            string _return_string = "";

            for (int i = 0; i < Str_Value.Length; i++)
            {
                _return_string += String.Format("{0:X2}", (int)Convert.ToChar(Str_Value.Substring(i, 1)));
            }
            return _return_string;
        }
        /*
       * input int = 12
       * output string = 0012
       */
        public static string format_string_4(int a_num)
        {
            return String.Format("{0:D4}", a_num);
        }

        /*
         * input string = 12
         * output string = 3132
         */
        public static string str_to_ascstr(string a)
        {
            string temp;

            temp = "";
            for (int i = 0; i < a.Length; i++)
            {
                string t = a.Substring(i, 1);
                temp += String.Format("{0:X2}", (int)Convert.ToChar(t));
            }
            return temp;
        }

        public static char[] hexstr_to_array(string a_str)
        {
            char[] bt = new char[a_str.Length / 2];
            int n = 0;
            for (int i = 0; i < a_str.Length; i += 2)
            {
                string s = a_str.Substring(i, 2);
                byte bt1 = Byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
                bt[n] = (char)bt1;
                n++;
            }
            return bt;
        }
        public static string char_to_str(char[] a_char, int len)
        {
            string s;
            s = "";

            for (int i = 0; i < len; i++)
            {
                s += a_char[i].ToString();
            }
            return s;
        }
        
        public  static DateTime str_to_datetime(string Str,string Format)
        {
            //Format = "yyyy-MM-dd HH:mm:ss";
            try
            {
                DateTimeFormatInfo dtFI = new DateTimeFormatInfo();
                dtFI.ShortDatePattern = Format;
                return DateTime.Parse(Str, dtFI);
            }
            catch { return DateTime.Now; }
        }

        /// <summary>
        /// get Str SHA1 Value
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UserSHA1(string Str)
        {
            string cl = Str;
            string pwd = "";
            SHA1 sha = SHA1.Create();

            byte[] s = sha.ComputeHash(Encoding.UTF8.GetBytes(cl));

            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }

        public static string GetSHA512(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);
            SHA512Managed hashString = new SHA512Managed();
            string encodedData = Convert.ToBase64String(message);
            string hex = "";
            hashValue = hashString.ComputeHash(UE.GetBytes(encodedData));
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }


        /// <summary>
        /// get Str Md5 Value
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UserMd5_old(string Str)
        {
            string cl = Str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像            
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));

            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("x");
            }
            return pwd;
            //md5.
        }

        /// <summary>
        /// get Str Md5 Value
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UserMd5(string Str)
        {
            return GetSHA512(Str);

            string cl = Str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像            
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));

            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("x");
            }
            return pwd;
            //md5.
        }
        public static void make_folder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public static string get_str_value(string str)
        {
            string r = "";
            if (str != null)
            {
                try
                {
                    string temp = str.Substring(str.IndexOf("=") + 1);
                    r = temp.Trim();
                }
                catch { }
            }
            return r;
        }        

        public static bool copy_resize_image(Size new_size,string s_path,string t_path)
        {
            string folder = Path.GetDirectoryName(t_path);
            make_folder(folder);
     
            try
            {
                //Bitmap bm1 = (Bitmap)Image.FromFile(s_path);
                //Bitmap bm2 = new Bitmap(new_size.Width, new_size.Height);
                Bitmap bm2 = new Bitmap(Image.FromFile(s_path), new_size);
                bm2.Save(t_path, System.Drawing.Imaging.ImageFormat.Jpeg);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

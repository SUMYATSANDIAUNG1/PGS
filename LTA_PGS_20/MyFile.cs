using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LTA_PGS_20
{
    class MyFile
    {
        
        #region Paramiter
        /// <summary>
        /// File name
        /// </summary>
        private string strFileName;
        public string FileName
        {
            get
            {
                return strFileName;
            }
            set
            {
                strFileName = value;
            }
        }

        
        private StreamWriter swriter;
        private StreamReader sreader;
        #endregion

        #region constructor
        ///constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public MyFile()
        {
            sreader = null;
            sreader = null;
            strFileName = "";
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="filename"></param>
        public MyFile(string filename)
        {
            sreader = null;
            sreader = null;
            strFileName = filename;            
        }
        #endregion

        #region public function
        public void write_to_file(string str)
        {
            try
            {
                if (swriter==null)
                    swriter = new StreamWriter(FileName);
                swriter.WriteLine(str);
                swriter.Flush();
            }
            catch (Exception e)
            {                
                throw new IOException(e.Message);
            }
        }
        /// <summary>
        /// add string to file
        /// </summary>
        /// <param name="AddString"></param>
        public void addstr_to_file(string AddString)
        {
            try
            {
                if (swriter == null)
                    swriter = new StreamWriter(strFileName);
                swriter.BaseStream.Seek(0, SeekOrigin.End);
                swriter.WriteLine(AddString);
                swriter.Flush();
            }
            catch (Exception e)
            {
                close();
                string sss = e.Message;
            }
        }
        public string read_file_line()
        {
            try
            {
                if (sreader == null)
                    sreader = new StreamReader(FileName);
                return sreader.ReadLine();                
            }
            catch (Exception e)
            {
                string sss = e.Message;
                return null;
            }
        }

        public string search_file_line(string key)
        {
            try
            {
                if (sreader == null)
                    sreader = new StreamReader(FileName);

                //sreader = new StreamReader(FileName);
                string r="";
                while ((r = sreader.ReadLine()) != null)
                {
                    if (r.IndexOf(key) == 0)
                    {
                        return r.Remove(0,r.IndexOf("=")+1);
                    }
                }
                return "";
            }
            catch (Exception e)
            {
                string sss = e.Message;
                return null;
            }

        }
        public void close()
        {
            if (swriter != null)
            {
                swriter.Close();
                swriter.Dispose();
                swriter = null;
                strFileName = "";
            }

            if (sreader != null)
            {
                sreader.Close();
                sreader.Dispose();
                sreader = null;
                strFileName = "";
            }

        }
        public string read_file_all()
        {
            try
            {
                if (sreader == null)
                    sreader = new StreamReader(FileName);
                return sreader.ReadToEnd();
            }
            catch (Exception e)
            {
                string sss = e.Message;
                return null;
            }
        }
        #endregion
    }
}

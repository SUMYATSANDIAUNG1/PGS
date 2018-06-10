using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Windows.Forms;

namespace LTA_PGS_20
{
    class DB_Control
    {
        #region Private Variables

        private string _dbaAddress;
        private string _dbname;
        private string _password;
        private OdbcConnection _myconnecttion;
        private string _sql;
        #endregion

        #region Public Variables
        /// <summary>
        /// database ip      
        /// </summary>
        public string DBAddress
        {
            get { return _dbaAddress; }
            set { _dbaAddress = value; }
        }
        /// <summary>
        /// Database name
        /// </summary>
        public string DBName
        {
            get { return _dbname; }
            set { _dbname = value; }
        }
        /// <summary>
        /// Database access password
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public OdbcConnection MyConnecttion
        {
            get { return _myconnecttion; }
            set { _myconnecttion = value; }
        }

        public string SQL
        {
            get { return _sql; }
            set { _sql = value; }
        }
        #endregion
        /// <summary>
        /// init data base,connect to data base
        /// </summary>
        /// <returns></returns>
        public bool DB_Init()
        {                                  
            string MyConString = "DRIVER={MySQL ODBC 3.51 Driver};" +
                                 "SERVER=" + _dbaAddress + ";" +
                                 "DATABASE=" + _dbname + ";" +
                                 "UID=root;" +
                                 //"UID=SYSDBA;" +
                                 "PASSWORD=" + _password + ";" +
                                 "OPTION=3";
            _myconnecttion = new OdbcConnection(MyConString);
            try
            {
                _myconnecttion.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }


        public OdbcDataReader SQLExecuteReader()
        {
            OdbcCommand cmd = new OdbcCommand(_sql, _myconnecttion);

            OdbcDataReader MyReader = null;
            try
            {
                MyReader = cmd.ExecuteReader();
                return MyReader;
            }
            catch(Exception e)
            {
                Tools.write_log(_sql);
                Tools.write_log(e.Message);
                return null; 
            }            
        }

        public string udpate_table(string tbname, params string[] values)
        {
            string _sql = "select * from " + tbname;
            OdbcCommand cmd = new OdbcCommand(_sql, _myconnecttion);
            OdbcDataReader MyReader = cmd.ExecuteReader();
            string[] field_name = new string[MyReader.FieldCount];

            for (int j = 0; j < MyReader.FieldCount;j++ )
            {
                field_name[j] = MyReader.GetName(j);                
            }
            string ss = "update ";
            ss += tbname;
            ss += " set ";
            for (int i = 0; i < values.Length; i++)
            {
                ss += field_name[i] + "='" + values[i] + "'";
                if (i < values.Length - 1)
                    ss += ",";
            }
            return ss;
        }

        public string insert_table(string tbname, params string[] values)
        {
            string sss = "SHOW FIELDS FROM "+tbname;
            OdbcCommand cmd1 = new OdbcCommand(sss, _myconnecttion);
            OdbcDataReader MyReader1 = cmd1.ExecuteReader();

            string[] Extra = new string[MyReader1.RecordsAffected];
            if (MyReader1 != null)
            {
                int m=0;
                while (MyReader1.Read())
                {
                    Extra[m] = (string)MyReader1["Extra"];
                    m++;
                }                
            }
            string _sql = "select * from " + tbname;
            OdbcCommand cmd = new OdbcCommand(_sql, _myconnecttion);
            OdbcDataReader MyReader = cmd.ExecuteReader();            

            
            string ss = "insert into ";
            ss += tbname;
            ss += " (";
            for (int j = 0; j < MyReader.FieldCount;j++ )
            {
                if (Extra[j] != "auto_increment")
                {
                    ss += MyReader.GetName(j);
                    if (j < MyReader.FieldCount - 1)
                        ss += ",";
                }
            }
            ss += ") ";

            ss += "values ('";
            for (int i = 0; i < values.Length; i++)
            {
                if (Extra[i] != "auto_increment")
                {
                    ss += values[i];
                    if (i < values.Length - 1)
                        ss += "','";
                }
            }
            ss += "') ";
            return ss;
        }

        public string delete_table(string tbname)
        {
            string _sql = "delete from " + tbname;
            return _sql;
        }
    }
}
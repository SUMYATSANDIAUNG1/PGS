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
    public partial class Form_alarm : Form
    {
        public static Object[] VMS_Name_color = new Object[3] { "Green", "Amber", "Red" };
       
        
        public Form_alarm()
        {
            InitializeComponent();
            init_alarm();
        }

        #region Alarm List
        private void init_alarm()
        {
            //Time	Station	Alarm	Acked User	Function
            dataGridView7.Columns.Add("id", "ID");
            dataGridView7.Columns.Add("time", "Time");
            dataGridView7.Columns.Add("gdm", "Station");//
            dataGridView7.Columns.Add("alarm", "Alarm");//
            dataGridView7.Columns.Add("user", "Acked User");//
            //dataGridView7.Columns.Add("func", "Function");//
            DataGridViewButtonColumn _func = new DataGridViewButtonColumn();

            _func.HeaderText = "Function";
            _func.Name = "func";
            _func.FlatStyle = FlatStyle.Popup;

            _func.CellTemplate.Style.Font = new Font(this.Font, FontStyle.Underline);

            dataGridView7.Columns.Add(_func);
            dataGridView7.Columns["id"].Visible = false;
            dataGridView7.Columns["time"].Width = 160;
            dataGridView7.Columns["gdm"].Width = 120;
            dataGridView7.Columns["alarm"].Width = 180;
            dataGridView7.Columns["user"].Width = 120;
            dataGridView7.Columns["func"].Width = 90;

            dataGridView7.AllowUserToAddRows = false;
            dataGridView7.AllowUserToDeleteRows = false;
            dataGridView7.ReadOnly = true;
            dataGridView7.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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

        private string get_ack_by_id(int id)
        {
            if (id == 0)
                return "ACK";
            else
                return "NAK";
        }

        private void Alarm()
        {
            inc.db_control.SQL = "select * from tb_alarm order by fd_time";


            OdbcDataReader MyReader = inc.db_control.SQLExecuteReader();            
            
            if (MyReader != null)
            {                
                int i = 0;
                while (MyReader.Read())
                {
                    dataGridView7.Rows.Add();

                    dataGridView7.Rows[i].Cells["id"].Value = ((Int32)MyReader["fd_id"]).ToString();
                    dataGridView7.Rows[i].Cells["time"].Value = ((DateTime)MyReader["fd_time"]).ToString("yyyy-MM-dd HH:mm:ss");
                    
                    int equipid = ((Int32)MyReader["fd_gdmcms"]);
                    dataGridView7.Rows[i].Cells["gdm"].Value = get_name_by_id(equipid);

                    //dataGridView7.Rows[i].Cells["alarm"].Value = get_msg_by_id(((Int32)MyReader["fd_msgid"]));
                    int msgid = ((Int32)MyReader["fd_msgid"]);
                    dataGridView7.Rows[i].Cells["alarm"].Value = fn_global.fn_getmsgbyid(msgid, equipid);


                    int _ack = MyReader["fd_delete"] != DBNull.Value ? (int)(long)MyReader["fd_delete"] : 0;
                    dataGridView7.Rows[i].Cells["func"].Value = _ack == 0 ? "ACK" : "";
                    
                    dataGridView7.Rows[i].Cells["user"].Value = get_name_by_userid((int)((Int64)MyReader["fd_userid"]));

                    i++;
                }
            }
        }
        #endregion        

        private void Form_alarm_Load(object sender, EventArgs e)
        {
            textBox7.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Alarm();
        }

        private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn column = dataGridView7.Columns[e.ColumnIndex];
            if (column is DataGridViewButtonColumn)
            {
                if (dataGridView7.CurrentCell.Value != null)
                {
                    if (dataGridView7.CurrentCell.Value.ToString().Trim() != "")
                    {
                        int i = dataGridView7.CurrentRow.Index;
                        string _id = dataGridView7.Rows[i].Cells["id"].Value.ToString();
                        dataGridView7.Rows[i].Cells["user"].Value = get_name_by_userid(inc.currentuser);
                        inc.db_control.SQL = "update tb_alarm set fd_time_ack='"
                            + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            + "',fd_userid='" + inc.currentuser
                            + "',fd_delete=1"
                            + " where fd_id='" + _id + "'";
                        inc.db_control.SQLExecuteReader();
                        dataGridView7.CurrentCell.Value = "";
                    }
                }
            }
        }
        private void save_report(string name)
        {
            MyFile mf = new MyFile(name);
            mf.addstr_to_file("Alarm");
            mf.addstr_to_file("Time: " + DateTime.Now.ToString("dd ") + inc.Month_Display[DateTime.Now.Month] + DateTime.Now.ToString(" yyyy"));
            string str = "";

            for (int i = 0; i < dataGridView7.Columns.Count; i++)
            {
                if (dataGridView7.Columns[i].Visible)
                    str += dataGridView7.Columns[i].HeaderText + ",";
            }
            mf.addstr_to_file(str);
            for (int i = 0; i < dataGridView7.Rows.Count; i++)
            {
                str = "";
                for (int j = 0; j < dataGridView7.Columns.Count; j++)
                {
                    if (dataGridView7.Columns[j].Visible)
                        str += Tools.get_value(dataGridView7.Rows[i].Cells[j].Value) + ",";
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
                sfdSaveOnlineText.FileName = "Alarm";

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
            PrintDataGrid.Print_DataGridView(dataGridView7,"","Alarm List");                
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintDataGrid.Print_dataGridView_preview(dataGridView7, "", "Alarm List");                
        }
    }
}

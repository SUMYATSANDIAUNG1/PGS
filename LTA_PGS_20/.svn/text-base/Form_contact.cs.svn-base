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
    public partial class Form_contact : Form
    {
        public Form_contact()
        {
            InitializeComponent();

            dataGridView_list.Columns.Add("Id", "ID");            
            dataGridView_list.Columns.Add("name", "Name");
            dataGridView_list.Columns.Add("num", "Phone");
            dataGridView_list.Columns.Add("company", "Company");
            dataGridView_list.Columns.Add("remark", "Remark");
            
            dataGridView_list.Columns["name"].Width = 160;
            dataGridView_list.Columns["num"].Width = 100;
            dataGridView_list.Columns["company"].Width = 200;
            dataGridView_list.Columns["remark"].Width = 350;
            dataGridView_list.Columns["Id"].Visible = false;

        }

        private void load_contactlist()
        {
            inc.db_control.SQL = "select * from tb_address order by fd_id";
            OdbcDataReader MyReader = inc.db_control.SQLExecuteReader();

            dataGridView_list.Rows.Clear();
            int i = 0;
            while (MyReader.Read())
            {
                dataGridView_list.Rows.Add();
                dataGridView_list.Rows[i].Cells["Id"].Value = (Int64)MyReader["fd_id"];
                dataGridView_list.Rows[i].Cells["name"].Value = MyReader["fd_name"] != DBNull.Value ? (String)MyReader["fd_name"] : "";
                dataGridView_list.Rows[i].Cells["num"].Value = MyReader["fd_phone"] != DBNull.Value ? (String)MyReader["fd_phone"] : "";
                dataGridView_list.Rows[i].Cells["company"].Value = MyReader["fd_company"] != DBNull.Value ? (String)MyReader["fd_company"] : "";
                dataGridView_list.Rows[i].Cells["remark"].Value = MyReader["fd_memo"] != DBNull.Value ? (String)MyReader["fd_memo"] : "";

                i++;
            }
            dataGridView_list.EndEdit();

            try
            {
                textBox_name.Text = Tools.get_value(dataGridView_list.Rows[0].Cells["name"].Value);
                textBox_num.Text = Tools.get_value(dataGridView_list.Rows[0].Cells["num"].Value);
                textBox_company.Text = Tools.get_value(dataGridView_list.Rows[0].Cells["company"].Value);
                textBoxremark.Text = Tools.get_value(dataGridView_list.Rows[0].Cells["remark"].Value);
            }
            catch { }
        }
        private void button13_Click(object sender, EventArgs e)
        {            
            inc.db_control.SQL = inc.db_control.insert_table("tb_address", "", textBox_name.Text, textBox_num.Text, textBox_company.Text, textBoxremark.Text);
            if (inc.db_control.SQLExecuteReader() != null)
            {
                load_contactlist();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete the Contact?", "Contact List", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int i = dataGridView_list.CurrentRow.Index;
                string _id = Tools.get_value(dataGridView_list.Rows[i].Cells["Id"].Value);

                inc.db_control.SQL = inc.db_control.delete_table("tb_address");
                inc.db_control.SQL += " where fd_id='" + _id + "'";
                if (inc.db_control.SQLExecuteReader() != null)
                {
                    dataGridView_list.Rows.Remove(dataGridView_list.CurrentRow);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int i = dataGridView_list.CurrentRow.Index;
                string _id = Tools.get_value(dataGridView_list.Rows[i].Cells["Id"].Value);

                inc.db_control.SQL = inc.db_control.udpate_table("tb_address", _id, textBox_name.Text, textBox_num.Text, textBox_company.Text, textBoxremark.Text);
                inc.db_control.SQL += " where fd_id='" + _id + "'";
                if (inc.db_control.SQLExecuteReader() != null)
                {
                    MessageBox.Show("Update Contact List Successful");
                    load_contactlist();
                }
                else
                    MessageBox.Show("Update Contact List Failed");
            }
            catch (Exception e1)
            {
                MessageBox.Show("Update Contact List Failed");
            }
        }

        private void Form_contact_Load(object sender, EventArgs e)
        {
            load_contactlist();
        }

        private void dataGridView_list_SelectionChanged(object sender, EventArgs e)
        {
            int i = -1;
            if (dataGridView_list.CurrentRow != null)
            {
                i = dataGridView_list.CurrentRow.Index;
            }

            if (i < 0)
                return;
            
            //textBox_name.Text, textBox_num.Text, textBox_company.Text, textBoxremark.Text);

            textBox_name.Text = Tools.get_value(dataGridView_list.Rows[i].Cells["name"].Value);
            textBox_num.Text = Tools.get_value(dataGridView_list.Rows[i].Cells["num"].Value);
            textBox_company.Text = Tools.get_value(dataGridView_list.Rows[i].Cells["company"].Value);
            textBoxremark.Text = Tools.get_value(dataGridView_list.Rows[i].Cells["remark"].Value);
        }

    }
}

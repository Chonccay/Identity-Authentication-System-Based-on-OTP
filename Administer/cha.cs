using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace adm
{
    public partial class cha : Form
    {
        public cha()
        {
            InitializeComponent();
        }

        SqlConnection conn;
        SqlDataAdapter sda;
        DataTable dt;
        public static int a;

        public void getid(string id,int am)
        {
            label8.Text = id;
            a = am;
        }
        


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cha_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection("server=PC201801031527;database=db_15;uid=sa;pwd=ccc1519803907");
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select * from tb_people";
            sda = new SqlDataAdapter();
            sda.SelectCommand = cmd;
            dt = new DataTable();
            sda.Fill(dt);
            textBox2.Text = dt.Rows[a]["tb_pwd"].ToString();
            textBox3.Text = dt.Rows[a]["tb_remain"].ToString();
            textBox4.Text = dt.Rows[a]["tb_name"].ToString();
            textBox5.Text = dt.Rows[a]["tb_num"].ToString();
            textBox6.Text = dt.Rows[a]["tb_mail"].ToString();
            textBox7.Text = dt.Rows[a]["tb_tel"].ToString();
            textBox1.Text = dt.Rows[a]["tb_load"].ToString();
            textBox9.Text = dt.Rows[a]["tb_key"].ToString();
            conn.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            class1 DBC = new adm.class1();
            int num = Convert.ToInt32(textBox3.Text);
            string str1 = class1.Getcount1MD5(textBox2.Text.Trim().ToString() + textBox9.Text.Trim().ToString(), num);
            string strSql = "update tb_people set tb_pwd='" + str1 + "',tb_remain= '"
                + this.textBox3.Text.Trim().ToString() + "',tb_name='" + this.textBox4.Text.Trim().ToString() + "',tb_num='"
                + this.textBox5.Text.Trim().ToString() + "',tb_mail='" + this.textBox6.Text.Trim().ToString() + "',tb_tel='"
                + this.textBox7.Text.Trim().ToString() + "',tb_load='" + this.textBox1.Text.Trim().ToString() + "',tb_key='"
                + textBox9.Text.Trim().ToString() + "' where tb_ID='" + this.label8.Text.Trim().ToString() + "'";
            if (DBC.updateDB(strSql))
            {
                MessageBox.Show("修改成功");
            }
            else
            {
                MessageBox.Show("修改失败");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

    }
}

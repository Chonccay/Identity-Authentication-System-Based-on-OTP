using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.VisualBasic;
using System.Data.SqlClient;

namespace adm
{
    public partial class add : Form
    {
        public add()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool strqual = class1.Exists("select tb_ID from tb_people where tb_ID='" + textBox1.Text + "'");
            if (strqual)
            {
                MessageBox.Show("该用户账号已存在，勿重复添加！");
            }
            else
            {
                try
                {
                    if (this.textBox1.Text.Trim().ToString() != "" && this.textBox2.Text.Trim().ToString() != "" && this.textBox3.Text.Trim().ToString() != "")
                    {
                        class1 DBC = new class1();
                        int num = Convert.ToInt32(textBox3.Text);
                        KeyValuePair<string, string> keyPair = class1.CreateRSAKey();
                        string privateKey = keyPair.Value;
                        string publicKey = keyPair.Key;
                        string str1 = class1.Getcount1MD5(textBox2.Text.Trim().ToString() + publicKey, num);
                        string str2 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        string strSql = "insert into tb_people(tb_ID,tb_pwd,tb_remain,tb_name,tb_num,tb_mail,tb_tel,tb_load,tb_key) values('" + this.textBox1.Text.Trim().ToString() + "','" +
                        str1 + "','" + this.textBox3.Text.Trim().ToString() + "','" + this.textBox4.Text.Trim().ToString()
                        + "','" + this.textBox5.Text.Trim().ToString() + "','" + this.textBox6.Text.Trim().ToString() + "','"
                        + this.textBox7.Text.Trim().ToString() + "','" + str2 + "','" + publicKey + "')";
                        string strIn = Interaction.InputBox("请用户输入自己的八位口令短语", "字符串", "", 100, 100);
                        string AfterKey = class1.EncryptString(privateKey, strIn);
                        DBC.CreateXmlFile(textBox1.Text, AfterKey);
                        if (DBC.insertDB(strSql))
                        {
                            MessageBox.Show("添加成功");
                        }
                        else
                        {
                            MessageBox.Show("添加失败");
                        }
                    }
                    else
                    {
                        MessageBox.Show("请完善要添加的用户信息");
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                }
            }
        

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}

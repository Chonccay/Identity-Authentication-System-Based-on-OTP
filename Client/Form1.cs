using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Data.SqlClient;
using System.Xml;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        Socket socketClient = null;
        Thread threadClient = null;

        SqlConnection conn = new SqlConnection("server=PC201801031527;database=db_15;uid=sa;pwd=ccc1519803907");

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Class1 DBC = new Class1();
            string str3 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string strSql = "insert into tb_record(tb_ID,tb_way,tb_dataime) values('" + this.textBox1.Text.Trim().ToString() + "','" + this.comboBox1.Text.Trim().ToString() + "','" + str3 + "')";
            DBC.insertDB(strSql);
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint endpoint = new IPEndPoint(ipaddress, int.Parse("10"));
            socketClient.Connect(endpoint);
            threadClient = new Thread(RecMsg);
            threadClient.IsBackground = true;
            threadClient.Start();
            ClientSendMsg(comboBox1.Text + '$' + textBox1.Text);

        }


        private void RecMsg()
        {
            while (true)
            {
                try
                {
                    byte[] arrRecMsg = new byte[1024 * 1024];
                    int length = socketClient.Receive(arrRecMsg);
                    string strRecMsg = Encoding.UTF8.GetString(arrRecMsg, 0, length);
                    if (strRecMsg.Contains(","))
                    {
                        string[] strArray = strRecMsg.Split(',');
                        int countIC = Convert.ToInt32(strArray[0]);
                        string str1 = Class1.GetcountnMD5(textBox2.Text.Trim().ToString() + strArray[1], countIC);
                        ClientSendMsg(str1);
                    }
                    else if (strRecMsg.Contains("@"))
                    {
                        try
                        {
                            string[] strArray = strRecMsg.Split('@');
                            string str_path = "E://University//Project//xmlsaved//" + textBox1.Text + ".xml";
                            string PriKey;
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(str_path);
                            XmlNode node;
                            node = xmlDoc.SelectSingleNode("User/PriKey");
                            PriKey = node.InnerText;
                            string result = Class1.DecryptString(PriKey, textBox2.Text);
                            string signedKey = Class1.HashAndSignString(strArray[0], result);
                            ClientSendMsg(strArray[0] + '&' + signedKey + '&' + textBox1.Text);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("登录失败，请检查账号和密码是否正确！");

                        }
                    }
                    else
                    {
                        if (strRecMsg == "true" || strRecMsg == "True")
                        {
                            MessageBox.Show("登录成功！");
                        }
                        else
                        {
                            MessageBox.Show("登录失败，请检查账号和密码是否正确！");
                        }
                    }
                }
                catch
                {
                    socketClient.Shutdown(SocketShutdown.Both);
                    socketClient.Close();
                    break;
                }
            } 
        }

        private void ClientSendMsg(string sendMsg)
        {
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            socketClient.Send(arrClientSendMsg);
        }

        private DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox1.Items.Add("S/key认证");
            comboBox1.Items.Add("公钥认证");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}

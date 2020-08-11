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
using System.Threading;
using System.Net;
using System.Data.SqlClient;


namespace Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        Thread threadWatch = null; 
        Socket socketWatch = null;

        SqlConnection conn = new SqlConnection("server=PC201801031527;database=db_15;uid=sa;pwd=ccc1519803907");

        Socket socConnection = null;

        private void 启动登录认证ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint endpoint = new IPEndPoint(ipaddress, int.Parse("10"));
            socketWatch.Bind(endpoint);
            socketWatch.Listen(20);
            threadWatch = new Thread(WatchConnecting);
            threadWatch.IsBackground = true;
            threadWatch.Start();
            txtMsg.AppendText("开始监听" + "\r\n");
        }

        private void WatchConnecting()
        {
            while (true) 
            {
                socConnection = socketWatch.Accept();
                txtMsg.AppendText("客户端连接成功" + "\r\n");
                ParameterizedThreadStart pts = new ParameterizedThreadStart(ServerRecMsg);
                Thread thr = new Thread(pts);
                thr.IsBackground = true;
                thr.Start(socConnection);
            }
        }

        private void ServerSendMsg(string sendMsg)
        {
            byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            socConnection.Send(arrSendMsg);
            txtMsg.AppendText("本机:" + GetCurrentTime() + "\r\n" + sendMsg + "\r\n");
        }

        private void ServerRecMsg(object socketClientPara)
        {
            Socket socketServer = socketClientPara as Socket;
            while (true)
            {
                try
                {
                    byte[] arrServerRecMsg = new byte[1024 * 1024];
                    int length = socketServer.Receive(arrServerRecMsg);
                    string strSRecMsg = Encoding.UTF8.GetString(arrServerRecMsg, 0, length);
                    txtMsg.AppendText("客户端:" + GetCurrentTime() + "\r\n" + strSRecMsg + "\r\n");
                    if (strSRecMsg.Contains("$"))
                    {
                        string[] strArray = strSRecMsg.Split('$');
                        conn.Open();
                        SqlCommand com = new SqlCommand("select count(*) from tb_people where tb_ID='" + strArray[1] + "'", conn);
                        int count = Convert.ToInt32(com.ExecuteScalar());
                        if (count >= 1)
                        {
                            if (strArray[0] == "S/key认证")
                            {
                                SqlCommand cmd = new SqlCommand("select tb_key from tb_people where tb_ID='" + strArray[1] + "'", conn);
                                SqlDataReader sdr = cmd.ExecuteReader();
                                sdr.Close();
                                string strSeed = Convert.ToString(cmd.ExecuteScalar());
                                SqlCommand Scmd = new SqlCommand("select tb_remain from tb_people where tb_ID='" + strArray[1] + "'", conn);
                                SqlDataReader sdr2 = Scmd.ExecuteReader();
                                sdr2.Close();
                                string strCount = Convert.ToString(Scmd.ExecuteScalar());
                                ServerSendMsg(strCount + "," + strSeed);
                                conn.Close();
                            }
                            else
                            {
                                Class1 DBC = new Class1();
                                string str = DBC.StrRandom();
                                ServerSendMsg(str + "@");
                                conn.Close();
                            }
                        }
                        else
                        {
                            ServerSendMsg("wrong");
                            conn.Close();
                        }
                    }
                    else
                    {
                        if (strSRecMsg.Contains('&'))
                        {
                            string[] strArray = strSRecMsg.Split('&');
                            conn.Open();
                            SqlCommand sql = new SqlCommand();
                            sql.Connection = conn;
                            sql.CommandText = "select tb_key from tb_people where tb_ID='" + strArray[2] + "'";
                            string strPubKey = sql.ExecuteScalar().ToString();
                            bool verify = Class1.VerifySigned(strArray[0], strArray[1], strPubKey);
                            string strverify = verify.ToString();
                            ServerSendMsg(strverify);
                            conn.Close();
                        }
                        else
                        {
                            Class1 DBC = new Server.Class1();
                            conn.Open();
                            string str1 = "select max(ID) from tb_record ";
                            SqlCommand cmd = new SqlCommand(str1, conn);
                            int maxId = Convert.ToInt32(cmd.ExecuteScalar());
                            SqlCommand sqlcmd = new SqlCommand();
                            sqlcmd.Connection = conn;
                            sqlcmd.CommandText = "select tb_ID from tb_record where ID='" + Convert.ToString(maxId) + "'";
                            string val = sqlcmd.ExecuteScalar().ToString();
                            SqlCommand SqlCmd = new SqlCommand();
                            SqlCmd.Connection = conn;
                            SqlCmd.CommandText = "select tb_pwd from tb_people where tb_ID='" + val + "'";
                            string MD5pwd = SqlCmd.ExecuteScalar().ToString();
                            string MD5FromClient = Class1.GetMD5(strSRecMsg);
                            if (MD5pwd == MD5FromClient)
                            {
                                ServerSendMsg("true");
                                SqlCommand sql = new SqlCommand();
                                sql.Connection = conn;
                                sql.CommandText = "select tb_remain from tb_people where tb_ID='" + val + "'";
                                int count = Convert.ToInt32(sql.ExecuteScalar());
                                count--;
                                if (count == 0)
                                {
                                    string str3 = "";
                                    string str2 = "update tb_people set tb_pwd='" + str3 + "',tb_key='" + str3 + "'where tb_ID='" + val + "'";
                                    DBC.updateDB(str2);
                                }
                                else
                                {
                                    string str2 = "update tb_people set tb_pwd='" + strSRecMsg + "',tb_remain= '" + count.ToString() + "'where tb_ID='" + val + "'";
                                    DBC.updateDB(str2);
                                }
                            }
                            else
                            {
                                ServerSendMsg("wrong");
                            }
                            conn.Close();
                        }
                    }
                        }
                catch (Exception ex)
                {
                    break;
                }
            }
        }

        private DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }

        private void 审查日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Browse a = new Browse();
            a.Show();
        }

        private void txtMsg_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
       
    }
}

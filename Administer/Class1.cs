using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Xml;
using System.Globalization;

namespace adm
{
    class class1
    {
        private SqlConnection con = new SqlConnection("server=PC201801031527;uid=sa;pwd=ccc1519803907;database=db_15");

        public SqlConnection ConDB()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            return con;
        }

        public bool insertDB(String sql)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.CommandText = sql;
                    cmd.Connection = ConDB();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool updateDB(String sql)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                {
                    cmd.CommandText = sql;
                    cmd.Connection = ConDB();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public bool deleteDB(String sql)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.CommandText = sql;
                    cmd.Connection = ConDB();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        //MD5加密
        public static string EncryptByMD5(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.GetEncoding("utf-8").GetBytes(input);
            byte[] toData = md5.ComputeHash(fromData);
            String byteStr = "";
            for (int i = 0; i < toData.Length; i++)
            {
                String temp = toData[i].ToString("x");
                if (temp.Length == 1)
                {
                    temp = "0" + temp;
                }
                byteStr += temp;
            }
            return byteStr;
        }

        //MD5加密n+1次
        public static string Getcount1MD5(string str, int count)
        {
            string mystr = str;
            for (int i = 0; i <= count; i++)
            {
                mystr = EncryptByMD5(mystr);
            }
            return mystr;
        }

        //私钥和公钥生成
        public static KeyValuePair<string, string> CreateRSAKey()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            string privateKey = RSA.ToXmlString(true);
            string publicKey = RSA.ToXmlString(false);

            return new KeyValuePair<string, string>(publicKey, privateKey);
        }

        //DES加密
        public static string EncryptString(string input, string sKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = des.CreateEncryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return BitConverter.ToString(result);
            }
        }

        public void CreateNode(XmlDocument xmlDoc, XmlNode parentNode, string name, string value)
        {
            XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
            node.InnerText = value;
            parentNode.AppendChild(node);
        }

        public void CreateXmlFile(string User, string PK)
        {
            string str_path = "E://University//Project//xmlsaved//" + User + ".xml";
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(node);
            XmlNode root = xmlDoc.CreateElement("User");
            xmlDoc.AppendChild(root);
            CreateNode(xmlDoc, root, "username", User);
            CreateNode(xmlDoc, root, "PriKey", PK);
            try
            {
                xmlDoc.Save(str_path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection("server=PC201801031527;uid=sa;pwd=ccc1519803907;database=db_15"))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

    }
}

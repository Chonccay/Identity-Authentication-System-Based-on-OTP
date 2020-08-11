using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Globalization;

namespace Client
{
    class Class1
    {

        private SqlConnection con;
       
        public SqlConnection ConDB()
        {
            con = new SqlConnection("server=PC201801031527;uid=sa;pwd=ccc1519803907;database=db_15");
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

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.GetEncoding("utf-8").GetBytes(str);
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

        public static string GetcountnMD5(string str, int count)
        {
            string mystr = str;
            for (int i = 0; i < count; i++)
            {
                mystr = GetMD5(mystr);
            }
            return mystr;
        }

        //数字签名
        public static string HashAndSignString(string plaintext, string privateKey)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes(plaintext);

            using (RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider())
            {
                RSAalg.FromXmlString(privateKey);
                //使用SHA1进行摘要算法，生成签名
                byte[] encryptedData = RSAalg.SignData(dataToEncrypt, new SHA1CryptoServiceProvider());
                return Convert.ToBase64String(encryptedData);
            }
        }

        //DES解密
        public static string DecryptString(string input, string sKey)
        {
            string[] sInput = input.Split("-".ToCharArray());
            byte[] data = new byte[sInput.Length];
            for (int i = 0; i < sInput.Length; i++)
            {
                data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
            }
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = des.CreateDecryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
        }

    }
}

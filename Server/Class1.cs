using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;


namespace Server
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

        public static string Getcount1MD5(string str, int count)
        {
            string mystr = str;
            for (int i = 0; i <= count; i++)
            {
                mystr = GetMD5(mystr);
            }
            return mystr;
        }

        public string StrRandom()
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            Random randrom = new Random((int)DateTime.Now.Ticks);

            string str = "";
            for (int i = 0; i < 64; i++)
            {
                str += chars[randrom.Next(chars.Length)];
            }
            return str;
        }

        //签名认证
        public static bool VerifySigned(string plaintext, string SignedData, string publicKey)
        {
            using (RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider())
            {
                RSAalg.FromXmlString(publicKey);
                UnicodeEncoding ByteConverter = new UnicodeEncoding();
                byte[] dataToVerifyBytes = ByteConverter.GetBytes(plaintext);
                byte[] signedDataBytes = Convert.FromBase64String(SignedData);
                return RSAalg.VerifyData(dataToVerifyBytes, new SHA1CryptoServiceProvider(), signedDataBytes);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ResComm.Web.Lib.Util
{
    public class Security
    {
        public static string checkHMAC(string key, string message)
        {
            string result = "";
            if (key == null) return result;
            if (message == null) return result;


            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);

            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            result = ByteToString(hashmessage);
            return result;
        }
        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("x2"); // hex format
            }
            return (sbinary);
        }

        private const string _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890~!@$^*()-:,./";
        private static readonly Random _rng = new Random();
        public static string RandomString(int size)
        {
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }
        private const string _chars_alphanum = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public static string RandomAplhaNumString(int size)
        {
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars_alphanum[_rng.Next(_chars_alphanum.Length)];
            }
            return new string(buffer);
        }

    }
}

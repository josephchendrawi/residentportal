using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ResComm.Web.Payment
{
    public class Helper
    {
        public static string SignatureEncrypt(string Key)
        {
            var objSHA1 = new SHA1CryptoServiceProvider();
            objSHA1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Key));
            Byte[] buffer = objSHA1.Hash;
            string HashValue = System.Convert.ToBase64String(buffer);
            return HashValue;
        }
    }
}
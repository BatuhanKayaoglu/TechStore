using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Common.Infrastructure
{
    public class PasswordEncrypter
    {
        public static string Encrypt(string pasword)
        {
            using var md5=MD5.Create();

            byte[] inputBytes=Encoding.ASCII.GetBytes(pasword);
            byte[] hashBytes=md5.ComputeHash(inputBytes);   

            return Convert.ToHexString(hashBytes);  
        }
    }
}

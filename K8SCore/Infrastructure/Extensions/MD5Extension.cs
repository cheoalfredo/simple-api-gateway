using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace K8SCore.Infrastructure.Extensions
{
    public static class MD5Extension
    {
        public static string MD5Hash(this string string2Convert)
        {
            string encoded = string.Empty;          

            using (var md5 = MD5.Create())
            {
                encoded = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(string2Convert)));
            }                    

            return encoded;
        }
    }
}

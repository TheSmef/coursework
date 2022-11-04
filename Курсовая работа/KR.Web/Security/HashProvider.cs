using KR.Models;
using System.Security.Cryptography;
using System.Text;

namespace KR.Web.Security
{
    public static class HashProvider
    {
        public static string MakeHash(String target)
        {
            string hashTarget;
            using (SHA512 hash = new SHA512Managed())
            {
                byte[] data = Encoding.Default.GetBytes(target);
                hashTarget = BitConverter.ToString(hash.ComputeHash(data)).Replace("-", "").ToLower();
            }
            return hashTarget;
        }
    }
}

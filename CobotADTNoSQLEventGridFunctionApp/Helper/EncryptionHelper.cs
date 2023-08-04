using System.Text;
using System.Security.Cryptography;

namespace CobotADTNoSQLEventGridFunctionApp.Helper
{
    public class EncryptionHelper
    {
        public static string MD5Encryption(string encryptionText)
        {

            MD5 md5 = MD5.Create();
            byte[] array = Encoding.UTF8.GetBytes(encryptionText);
            array = md5.ComputeHash(array);
            StringBuilder sb = new StringBuilder();
            foreach (byte ba in array)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
    }
}

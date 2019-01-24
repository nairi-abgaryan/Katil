using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Katil.Common.Utilities
{
    public static class HashHelper
    {
        public static string GetHash(string value)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            var a = sb.ToString();
            return a;
        }

        public static string EncryptPassword(string password, string username)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(username, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                if (encryptor != null)
                {
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }

                        password = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            return password;
        }

        public static string DecryptPassword(string cipherPassword, string username)
        {
            cipherPassword = cipherPassword.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherPassword);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(username, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                if (encryptor != null)
                {
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }

                        cipherPassword = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }

            return cipherPassword;
        }

        public static string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder stringBuilder = new StringBuilder();

                foreach (byte item in data)
                {
                    stringBuilder.Append(item.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}

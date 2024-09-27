using Newtonsoft.Json;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Services
{
    public static class Utils
    {
        private const string keyString = "c55d79f3f4184e2f8f3c979b367821b1";
        private const string ClientKey = "-@!8A0P.!nm099(+";
        private const string ClientSalt = "i+!_Ay(1_9-*!71O";
        private static readonly Random random = new();

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string RemoveSpecialCharacters2(string str)
        {
            StringBuilder sb = new();
            foreach (char c in str)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static bool IsNumeric(this string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Encrypt string.
        /// /// </summary>
        /// <param name="text"></param>
        /// <returns name="result">Decrypted string</returns>
        public static string Encrypt(string text)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using var aesAlg = Aes.Create();
            using var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV);
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }

            var iv = aesAlg.IV;

            var decryptedContent = msEncrypt.ToArray();

            var result = new byte[iv.Length + decryptedContent.Length];

            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Decrypt string.
        /// /// </summary>
        /// <param name="cipherText"></param>
        /// <returns name="result">Encrypted string</returns>
        public static string Decrypt(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using var aesAlg = Aes.Create();
            using var decryptor = aesAlg.CreateDecryptor(key, iv);
            string result;
            using (var msDecrypt = new MemoryStream(cipher))
            {
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);
                result = srDecrypt.ReadToEnd();
            }

            return result;
        }

        public static string DecryptStringAES(string cipherText)
        {
            try
            {
                var secretkey = Encoding.UTF8.GetBytes(ClientKey);
                var ivKey = Encoding.UTF8.GetBytes(ClientSalt);

                var encrypted = Convert.FromBase64String(cipherText);
                var decriptedFromJavascript = DecryptStringFromBytes(encrypted, secretkey, ivKey);
                return decriptedFromJavascript;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string EncryptStringAES(string plainText)
        {
            try
            {
                var secretkey = Encoding.UTF8.GetBytes(ClientKey);
                var ivKey = Encoding.UTF8.GetBytes(ClientSalt);

                var plainBytes = Encoding.UTF8.GetBytes(plainText);
                // var encrypted = Convert.ToBase64String(plainBytes);
                var encryptedFromJavascript = EncryptStringToBytes(plainText, secretkey, ivKey);
                // _logger.LogWarning($" decriptedFromJavascript: {encryptedFromJavascript}");
                return encryptedFromJavascript;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {

            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = Aes.Create())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;
                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                // Create the streams used for encryption.
                using var msEncrypt = new MemoryStream();
                using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write(plainText);
                }
                encrypted = msEncrypt.ToArray();
            }
            //Convert.ToBase64String(encrypted);
            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = Aes.Create())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                // rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;
                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.
                    using var msDecrypt = new MemoryStream(cipherText);
                    using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                    using var srDecrypt = new StreamReader(csDecrypt);
                    // Read the decrypted bytes from the decrypting stream
                    // and place them in a string.
                    plaintext = srDecrypt.ReadToEnd();
                }
                catch (Exception)
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        public static async Task<IpInfo> GetUserLocationByIp(string ip)
        {
            _ = new IpInfo();
            IpInfo ipInfo;
            try
            {
                string info = await new HttpClient().GetStringAsync("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
            }
            catch (Exception)
            {
                ipInfo = null;
            }

            return ipInfo;
        }

        public static DateTime GetDateofFOW()
        {
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
            DayOfWeek today = DateTime.Now.DayOfWeek;

            return DateTime.Now.AddDays(-(today - fdow)).Date;
        }

        public static IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }

        public static (DateTime, DateTime) PreviousWeek()
        {
            DateTime dateFDW = GetDateofFOW();

            DateTime previousFDW = dateFDW.AddDays(-7);
            DateTime previousLDW = dateFDW.AddDays(-1);

            return (previousFDW, previousLDW);
        }

        public static string GenerateUniqueNumber()
        {
            var timestampPart = DateTime.UtcNow.ToString("HHmmss"); // 6 digits from the current time
            var randomPart = random.Next(0, 100).ToString("D2");    // 2 digits random number

            return timestampPart + randomPart;
        }
        
    }
}

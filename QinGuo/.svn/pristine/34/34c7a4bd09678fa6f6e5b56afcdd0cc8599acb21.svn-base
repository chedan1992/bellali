using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace QINGUO.Common
{
    public class AESEncryption
    {
        //默认密钥向量   
        private static byte[] _key1 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>  
        /// AES加密算法  
        /// </summary>  
        /// <param name="plainText">明文字符串</param>  
        /// <param name="strKey">密钥</param>  
        /// <returns>返回加密后的密文字节数组</returns>  
        public string AESEncrypt(string text, string key)
        {
            Byte[] _Key = Encoding.ASCII.GetBytes(key);
            Byte[] _Source = Encoding.UTF8.GetBytes(text);

            Aes aes = Aes.Create("AES");
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _Key;
            ICryptoTransform cTransform = aes.CreateEncryptor();
            Byte[] cryptData = cTransform.TransformFinalBlock(_Source, 0, _Source.Length);
            string HexCryptstring = Hex2To16(cryptData);
            Byte[] HexCryptData = Encoding.UTF8.GetBytes(HexCryptstring);
            string Cryptstring = Convert.ToBase64String(HexCryptData);
            return Cryptstring;  
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="cipherText">密文字节数组</param>  
        /// <param name="strKey">密钥</param>  
        /// <returns>返回解密后的字符串</returns>  
        public string AESDecrypt(string cipherText, string key)
        {
            Byte[] _Key = Encoding.ASCII.GetBytes(key);
            Aes aes = Aes.Create("AES");
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7 ;
            aes.Key = _Key;
            ICryptoTransform cTransform = aes.CreateDecryptor();

            Byte[] encryptedData = Convert.FromBase64String(cipherText);
            string encryptedstring = Encoding.UTF8.GetString(encryptedData);
            Byte[] _Source = Hex16To2(encryptedstring);
            Byte[] originalSrouceData = cTransform.TransformFinalBlock(_Source, 0, _Source.Length);
            string originalstring = Encoding.UTF8.GetString(originalSrouceData);
            return originalstring;  
        }

        /// <summary>  
        /// 2进制转16进制  
        /// </summary>  
        public static string Hex2To16(Byte[] bytes)
        {
            string hexstring = string.Empty;
            Int32 iLength = 65535;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                if (bytes.Length < iLength)
                {
                    iLength = bytes.Length;
                }

                for (int i = 0; i < iLength; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexstring = strB.ToString();
            }
            return hexstring;
        }

        /// <summary>  
        /// 16进制转2进制  
        /// </summary>  
        public static Byte[] Hex16To2(string hexstring)
        {
            if ((hexstring.Length % 2) != 0)
            {
                hexstring += " ";
            }
            Byte[] returnBytes = new Byte[hexstring.Length / 2];
            for (Int32 i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexstring.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }


        /// <summary>
        /// Encrpyts the sourceString, returns this result as an Aes encrpyted, BASE64 encoded string
        /// </summary>
        /// <param name="plainSourceStringToEncrypt">a plain, Framework string (ASCII, null terminated)</param>
        /// <param name="passPhrase">The pass phrase.</param>
        /// <returns>
        /// returns an Aes encrypted, BASE64 encoded string
        /// </returns>
        public string EncryptString(string plainSourceStringToEncrypt, string passPhrase)
        {
            //Set up the encryption objects
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(passPhrase)))
            {
                byte[] sourceBytes = Encoding.ASCII.GetBytes(plainSourceStringToEncrypt);
                ICryptoTransform ictE = acsp.CreateEncryptor();

                //Set up stream to contain the encryption
                MemoryStream msS = new MemoryStream();

                //Perform the encrpytion, storing output into the stream
                CryptoStream csS = new CryptoStream(msS, ictE, CryptoStreamMode.Write);
                csS.Write(sourceBytes, 0, sourceBytes.Length);
                csS.FlushFinalBlock();

                //sourceBytes are now encrypted as an array of secure bytes
                byte[] encryptedBytes = msS.ToArray(); //.ToArray() is important, don't mess with the buffer

                //return the encrypted bytes as a BASE64 encoded string
                return Convert.ToBase64String(encryptedBytes);
            }
        }


        /// <summary>
        /// Decrypts a BASE64 encoded string of encrypted data, returns a plain string
        /// </summary>
        /// <param name="base64StringToDecrypt">an Aes encrypted AND base64 encoded string</param>
        /// <param name="passphrase">The passphrase.</param>
        /// <returns>returns a plain string</returns>
        public string DecryptString(string base64StringToDecrypt, string passphrase)
        {
            //Set up the encryption objects
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(passphrase)))
            {
                byte[] RawBytes = Convert.FromBase64String(base64StringToDecrypt);
                ICryptoTransform ictD = acsp.CreateDecryptor();

                //RawBytes now contains original byte array, still in Encrypted state

                //Decrypt into stream
                MemoryStream msD = new MemoryStream(RawBytes, 0, RawBytes.Length);
                CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read);
                //csD now contains original byte array, fully decrypted

                //return the content of msD as a regular string
                return (new StreamReader(csD)).ReadToEnd();
            }
        }

        private static AesCryptoServiceProvider GetProvider(byte[] key)
        {
            AesCryptoServiceProvider result = new AesCryptoServiceProvider();
            result.BlockSize = 128;
            result.KeySize = 128;
            result.Mode = CipherMode.CBC;
            result.Padding = PaddingMode.PKCS7;

            result.GenerateIV();
            result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            byte[] RealKey = GetKey(key, result);
            result.Key = RealKey;
            // result.IV = RealKey;
            return result;
        }

        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            byte[] kRaw = suggestedKey;
            List<byte> kList = new List<byte>();

            for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                kList.Add(kRaw[(i / 8) % kRaw.Length]);
            }
            byte[] k = kList.ToArray();
            return k;
        }

    }
}

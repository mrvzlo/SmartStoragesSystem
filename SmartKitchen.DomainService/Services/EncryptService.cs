using System;
using System.Security.Cryptography;
using System.Text;

namespace SmartKitchen.DomainService.Services
{
    public static class EncryptService
    {
        private const int KeySizeInBytes = 4096;
        public static void GetKeys(out string publicKey, out string privateKey)
        {
            var csp = new RSACryptoServiceProvider(KeySizeInBytes);
            var privKey = csp.ExportParameters(true);
            var pubKey = csp.ExportParameters(false);
            {
                var sw = new System.IO.StringWriter();
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                xs.Serialize(sw, pubKey);
                publicKey = sw.ToString();
            }
            {
                var sw = new System.IO.StringWriter();
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                xs.Serialize(sw, privKey);
                privateKey = sw.ToString();
            }
        }

        public static string Decrypt(string text, string privateKey)
        {
            var bytesCypherText = Convert.FromBase64String(text);

            RSAParameters privKey;
            {
                var sr = new System.IO.StringReader(privateKey);
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                privKey = (RSAParameters)xs.Deserialize(sr);
            }

            var csp = new RSACryptoServiceProvider(KeySizeInBytes);
            csp.ImportParameters(privKey);

            var bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

            var plainTextData = Encoding.UTF8.GetString(bytesPlainTextData);
            return plainTextData;
        }
    }
}
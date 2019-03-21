using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace TestConsole
{
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    [SuppressMessage("ReSharper", "UnusedVariable")]
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:2740/";
            var id = 1;
            var publicKey = File.ReadAllText("C:\\Users\\vladimir\\source\\repos\\SmartKitchen\\TestConsole\\key.txt");
            var privateKey = File.ReadAllText("C:\\Users\\vladimir\\source\\repos\\SmartKitchen\\TestConsole\\keyPrivate.txt");
            string action;
            do
            {
                Console.Clear();
                Console.WriteLine("1 - Add cell\n2 - Update cell date\n3 - Update cell amount\n4 - Delete cell\n5 - Exit");
                switch (Console.ReadLine())
                {
                    case "1":
                        action = "AddCell"; break;
                    case "2":
                        action = "RemoveCell"; break;
                    case "3":
                        action = "UpdateCellAmount"; break;
                    case "4":
                        action = "UpdateCellBestBefore"; break;
                    case "5":
                        action = "exit"; break;
                    default:
                        action = "unknown"; break;
                }

                if (action == "exit" || action == "unknown") continue;
                Console.WriteLine("Please input object ID");
                var obj = Console.ReadLine();
                Console.WriteLine("Please input parameter value");
                var value = Console.ReadLine();

                var request = url + "?request=";
                request += Uri.EscapeDataString(id + ":"+Encrypt(action + ":" + obj + ":" + value, publicKey));
                Console.WriteLine(request);
                var webRequest = WebRequest.Create(request);
                StreamReader objReader;
                try
                {
                    var stream = webRequest.GetResponse().GetResponseStream();
                    objReader = new StreamReader(stream);
                }
                catch (Exception)
                {
                    continue;
                }

                string sLine = "";
                int i = 0;
                while (sLine != null)
                {
                    i++;
                    sLine = objReader.ReadLine();
                    if (sLine != null) Console.WriteLine("{0}:{1}", i, sLine);
                }
                Console.ReadLine();
            } while (action != "exit");
        }

        private static string Encrypt(string source, string key)
        {
            RSAParameters pubKey;
            {
                var sr = new StringReader(key);
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                pubKey = (RSAParameters)xs.Deserialize(sr);
            }

            var csp = new RSACryptoServiceProvider(512);
            csp.ImportParameters(pubKey);
            var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(source);
            var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);
            var cypherText = Convert.ToBase64String(bytesCypherText);

            return cypherText;
        }
        
    }
}

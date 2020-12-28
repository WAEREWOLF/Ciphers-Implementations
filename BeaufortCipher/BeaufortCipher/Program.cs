using System;
using System.IO;
using System.Linq;

namespace BeaufortCipher
{
    class Program
    {
        // used for encrypting and decrypting
        public static string BeaufortCipher(string plainText, string key)
        {
            string beaufortText = "";
            char[] keys = key.ToArray();
            int j = 0;
            foreach (char c in plainText)
            {
                int p = c - 65;
                int k = keys[j] - 65;
                if (k >= p)
                {
                    beaufortText += (char)((k - p) % 26 + 65);
                }
                else
                {
                    beaufortText += (char)((k - p + 26) % 26 + 65);
                }
                j = (j + 1) % key.Length;                
            }

            return beaufortText;
        }

        static void Main(string[] args)
        {
            // read the plain text from the EncryptBeaufort.txt file
            string fileEncrypt = File.ReadAllText(@"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\EncryptBeaufort.txt");

            // file path for the file where the encrypted message will be stored and later on will be decoded
            string fileDecryptPath = @"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\DecryptBeaufort.txt";
            
            // print initial plaintext
            Console.WriteLine("Initial message: " + fileEncrypt);

            // user input key for Beaufort encryption
            Console.WriteLine("Input the key to cypher EncryptBeaufort.txt: ");
            string input = Convert.ToString(Console.ReadLine()); // the user input key should be a string

            // call the Encrypt method using as parameters the file and for the key, the user input
            string encryptedMessage = BeaufortCipher(fileEncrypt.ToUpper(), input.ToUpper());

            // display the encrypted message
            Console.WriteLine("The file was encrypted: \n" + encryptedMessage + "\n");

            // write crypted message to file EncryptBeaufort.txt
            File.WriteAllText(fileDecryptPath, encryptedMessage);

            // read text for decryption in the file at a speciffic path
            string fileDecrypt = File.ReadAllText(fileDecryptPath);

            Console.WriteLine("Input the key to decrypt DecryptBeaufort.txt: ");
            string input2 = Convert.ToString(Console.ReadLine()); // the user input key should be a string

            // decrypt the message from fileDecrypt.txt
            string decryptMessage = BeaufortCipher(fileDecrypt.ToUpper(), input2.ToUpper());
            Console.WriteLine("The file was decrypted: \n" + decryptMessage + "\n");
        }

        
    }
}


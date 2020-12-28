using System;
using System.IO;

namespace AutoKeyCipher
{
    class Program
    {
        // alphabet with capital letters
        private static readonly char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        // used for encryption and decryption based on the boolean toEncrypt ( true = Encrypt)
        public static string AutoKeyCipher(string plain, string pass, bool toEncrypt)
        {
            char[] message = plain.ToCharArray();
            char[] keystream = (pass + plain).ToCharArray();

            for (int i = 0; i < message.Length; i++)
            {
                int keyidx = Array.IndexOf(alphabet, keystream[i]);
                int msgidx = Array.IndexOf(alphabet, message[i]);

                if (toEncrypt)
                {
                    // Encrypt
                    message[i] = alphabet[(alphabet.Length + keyidx + msgidx) % alphabet.Length];
                }
                else
                {
                    // Decrypt
                    message[i] = alphabet[(alphabet.Length + msgidx - keyidx) % alphabet.Length];
                    keystream[i + pass.Length] = message[i];
                }
            }
            return new string(message);
        }

        static void Main(string[] args)
        {           

            // read the plain text from the EncryptAutoKey.txt file
            string fileEncrypt = File.ReadAllText(@"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\EncryptAutoKey.txt");

            // file path for the file where the encrypted message will be stored and later on will be decoded
            string fileDecryptPath = @"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\DecryptAutoKey.txt";

            // print initial plaintext
            Console.WriteLine("Initial message: " + fileEncrypt);

            // user input key for Autokey encryption
            Console.WriteLine("Input the key to cypher EncryptAutokey.txt: ");
            string input = Convert.ToString(Console.ReadLine()); // the user input key should be a string

            // call the Encrypt method using as parameters the file and for the key, the user input
            string encryptedMessage = AutoKeyCipher(fileEncrypt.ToUpper(), input.ToUpper(), true);

            // display the encrypted message
            Console.WriteLine("The file was encrypted: \n" + encryptedMessage + "\n");

            // write crypted message to file DecryptAutokey.txt
            File.WriteAllText(fileDecryptPath, encryptedMessage);

            // read text for decryption in the file at a speciffic path
            string fileDecrypt = File.ReadAllText(fileDecryptPath);

            Console.WriteLine("Input the key to decrypt DecryptAutokey.txt: ");
            string input2 = Convert.ToString(Console.ReadLine()); // the user input key should be a string

            // decrypt the message from DecryptAutoKey.txt
            string decryptMessage = AutoKeyCipher(fileDecrypt.ToUpper(), input2.ToUpper(), false);
            Console.WriteLine("The file was decrypted: \n" + decryptMessage + "\n");
        }
    }
}

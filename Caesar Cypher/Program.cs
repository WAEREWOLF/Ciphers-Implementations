using System;
using System.IO;

namespace Caesar_Cypher
{
    class Program
    {
        // mehod that will shift characters based on the provided key
        private static char ShiftChar(char ch, int key)
        {   
            // check if the current char is Not a letter, so ignore it
            if (!char.IsLetter(ch))
                return ch;

            // check if the letter is uppercase or lower
            char offset = char.IsUpper(ch) ? 'A' : 'a';
            // apply (char + key) % 26 but we add in the problem the offset in order to detect the lower or uppercase
            return (char)((((ch + key) - offset) % 26) + offset);
        }

        // method that will iterate through the input file characters and will call the above method ShiftChar
        public static string Encrypt(string input, int key)
        {
            string output = string.Empty;

            foreach (char ch in input)
            {
                output += ShiftChar(ch, key);
            }

            return output;
        }

        // method that will decrypt the message providing a key and a crypted message
        public static string Decrypt(string input, int key)
        {            
            return Encrypt(input, 26 - key); // is enough to call the Encrypt method but with the key: (26 - key) because is used the cyclic property of the cipher under modulo
        }

        public static void Main(string[] args)
        {
            // read from file
            string fileEncrypt = File.ReadAllText(@"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\Encrypt.txt");
            string fileDecrypt = File.ReadAllText(@"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\Decrypt.txt");

            // user input for the key for Caesar encryption
            Console.WriteLine("Input the key to cypher Encrypt.txt: ");
            int input = Convert.ToInt32(Console.ReadLine()); // the user input should be an int

            // call the Encrypt method using as parameters the file and for the key, the user input
            string encryptedMessage = Encrypt(fileEncrypt, input);
            // disply the encrypted message
            Console.WriteLine("The file was encrypted: \n" + encryptedMessage + "\n");

            Console.WriteLine("Input the key to decrypt Decrypt.txt: ");
            int input2 = Convert.ToInt32(Console.ReadLine()); // the user input should be an int
            // Call the Decrypt method with the crypted message in order to decrypt it
            string decryptMessage = Decrypt(fileDecrypt, input2);
            Console.WriteLine("The file was decrypted: \n" + decryptMessage + "\n");
        }
    }
}

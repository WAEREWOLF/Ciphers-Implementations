using System;
using System.IO;

namespace TwoSquareCipher
{
    class Program
    {
        static readonly char[,] Merged_Arr = new char[5, 10]; // merged array 5*10 elements total
        static readonly char[,] First_Arr = new char[5, 5];    // first array with 5*5 elements   
        static readonly char[,] Second_Arr = new char[5, 5];   // second array with 5*5 elements

        // encryption function
        static string Encrypt(string plain_text, string key1, string key2)
        {
            string encrypted_msg = "";

            // fill the second array with the provided key2
            int n = -1;
            for (int i = 0; i < key2.Length; i++)
            {
                n += 1;
                n %= 5;
                int m = (int)(i / 5);
                Second_Arr[m, n] = key2[i];
            }

            // fill the first array with the provided key1
            n = -1;
            for (int i = 0; i < key1.Length; i++)
            {
                n += 1;
                n %= 5;
                int m = (int)(i / 5);
                First_Arr[m, n] = key1[i];
            }

            string sequence = "";

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (j > 4)
                    {
                        int k = j - 5;
                        sequence += Second_Arr[i, k];
                    }
                    else
                    {
                        sequence += First_Arr[i, j];
                    }
                }
            }
            
            int ctr = -1;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 10; j++)
                {
                    ctr++;
                    Merged_Arr[i, j] = sequence[ctr]; // fill the merged array                   
                }
            
            // iterate through the plain text and sepparate the text in pairs of 2 letters
            // for each bigram created find the first letter in the first array and the second letter in the second array respecting the rules            
            for (int i = 0; i < plain_text.Length; i += 2)
            {
                int row = 0, col = 0;
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (Second_Arr[j, k] == plain_text[i])
                        {
                            row = j;
                            break;
                        }
                        if (row != 0)
                            break;
                    }
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (First_Arr[j, k] == plain_text[i + 1])
                        {
                            col = k + 5;
                            break;
                        }
                        if (col != 0)
                            break;
                    }
                encrypted_msg += Merged_Arr[row, col].ToString();

                row = 0;
                col = 0;
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (First_Arr[j, k] == plain_text[i + 1])
                        {
                            row = j;
                            break;
                        }
                        if (row != 0)
                            break;
                    }
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (Second_Arr[j, k] == plain_text[i])
                        {
                            col = k+5;
                            break;
                        }
                        if (col != 0)
                            break;
                    }
                encrypted_msg += Merged_Arr[row, col].ToString();

            }
            return encrypted_msg;
        }

        //decryption function
        static string Decrypt(string encrypted_msg)
        {
            string decrypted_msg = "";
            for (int i = 0; i < encrypted_msg.Length; i += 2)
            {
                int row = 0, col = 0;
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (First_Arr[j, k] == encrypted_msg[i])
                        {
                            row = j;
                            break;
                        }
                        if (row != 0)
                            break;
                    }
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (Second_Arr[j, k] == encrypted_msg[i + 1])
                        {
                            col = k;
                            break;
                        }
                        if (col != 0)
                            break;
                    }
                decrypted_msg += Merged_Arr[row, col].ToString();
                row = 0;
                col = 0;
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (Second_Arr[j, k] == encrypted_msg[i + 1])
                        {
                            row = j;
                            break;
                        }
                        if (row != 0)
                            break;
                    }
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (First_Arr[j, k] == encrypted_msg[i])
                        {
                            col = k + 5;
                            break;
                        }
                        if (col != 0)
                            break;
                    }
                decrypted_msg += Merged_Arr[row, col].ToString();
            }
            return decrypted_msg;
        }

        //main function
        static void Main(string[] args)
        {
            // read the plain text from the EncryptTwoSquare.txt file
            string fileEncrypt = File.ReadAllText(@"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\EncryptTwoSquare.txt");

            // file path for the file where the encrypted message will be stored and later on will be decoded
            string fileDecryptPath = @"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\DecryptTwoSquare.txt";

            // print initial plaintext
            Console.WriteLine("Initial message: " + fileEncrypt + "\n");

            // user input key for the TwoSquare encryption
            Console.WriteLine("Input the first key: ");
            string input = Convert.ToString(Console.ReadLine()); // the user input key should be a string

            Console.WriteLine("Input the second key: ");
            string input2 = Convert.ToString(Console.ReadLine()); // the user input key should be a string

            // call the Encrypt method using as parameters the file and for the key, the user input
            string encryptedMessage = Encrypt(fileEncrypt.ToUpper(), input.ToUpper(), input2.ToUpper());

            // display the encrypted message
            Console.WriteLine("\nThe file EncryptTwoSquare.txt was encrypted: \n" + encryptedMessage + "\n");

            // write crypted message to file EncryptFourSquare.txt
            File.WriteAllText(fileDecryptPath, encryptedMessage);
            Console.WriteLine("Writing the encrypted message to DecryptTwoSquare.txt ...\n");

            // read text for decryption in the file at a speciffic path
            string fileDecrypt = File.ReadAllText(fileDecryptPath);

            // decrypt the message from fileDecrypt.txt
            string decryptMessage = Decrypt(fileDecrypt.ToUpper());
            Console.WriteLine("The file DecryptFourSquare.txt was decrypted: \n" + decryptMessage + "\n");
        }
    }
}

using System;
using System.IO;

namespace FourSquare
{
    class Program
    {
        static readonly char[,] aggregate = new char[10,10];        
        static readonly char[,] Top_Left = new char[5,5];
        static readonly char[,] Top_Right = new char[5,5];
        static readonly char[,] Bottom_Left = new char[5,5];
        static readonly char[,] Bottom_Right = new char[5,5];

        // encryption function
        static string Encrypt(string plain_text, string key1, string key2)
        {
            string encrypted_msg = "";
            int n = -1;
            for (int i = 0; i < key1.Length; i++)
            {
                n += 1;
                n %= 5;
                int m = (int)(i / 5);
                Top_Right[m,n] = key1[i];
            }
            n = -1;
            for (int i = 0; i < key2.Length; i++)
            {
                n += 1;
                n %= 5;
                int m = (int)(i / 5);
                Bottom_Left[m,n] = key2[i];
            }
            int num = 64;

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    num++;
                    if (num == 74)
                        num++;
                    Top_Left[i,j] = (char)num; //fill Top Left table with standard alphabet omitting letter 'J'
                    Bottom_Right[i,j] = (char)num; //fill Bottom Right table with standard alphabet omitting letter 'J'
                }
            string sequence = "";

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (j > 4)
                    {
                        int k = j - 5;
                        sequence += Top_Right[i,k];
                    }
                    else
                    {
                        sequence += Top_Left[i,j];
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (j > 4)
                    {
                        int k = j - 5;
                        sequence += Bottom_Right[i,k];
                    }
                    else
                    {
                        sequence += Bottom_Left[i,j];
                    }
                }
            }
            int ctr = -1;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    ctr++;
                    aggregate[i,j] = sequence[ctr];
                }

            for (int i = 0; i < plain_text.Length; i += 2)
            {
                int row = 0, col = 0;
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (Top_Left[j,k] == plain_text[i])
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
                        if (Bottom_Right[j,k] == plain_text[i + 1])
                        {
                            col = k + 5;
                            break;
                        }
                        if (col != 0)
                            break;
                    }
                encrypted_msg += aggregate[row,col].ToString();
                row = 0;
                col = 0;
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (Bottom_Right[j,k] == plain_text[i + 1])
                        {
                            row = j + 5;
                            break;
                        }
                        if (row != 0)
                            break;
                    }
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (Top_Left[j,k] == plain_text[i])
                        {
                            col = k;
                            break;
                        }
                        if (col != 0)
                            break;
                    }
                encrypted_msg += aggregate[row,col].ToString();
            }
            return encrypted_msg;
        }
        // decryption function 
        static string Decrypt(string encrypted_msg, string key1, string key2)
        {
            string decrypted_msg = "";
            for (int i = 0; i < encrypted_msg.Length; i += 2)
            {
                int row = 0, col = 0;
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (Top_Right[j,k] == encrypted_msg[i])
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
                        if (Bottom_Left[j,k] == encrypted_msg[i + 1])
                        {
                            col = k;
                            break;
                        }
                        if (col != 0)
                            break;
                    }
                decrypted_msg += aggregate[row,col].ToString();
                row = 0;
                col = 0;
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (Bottom_Left[j,k] == encrypted_msg[i + 1])
                        {
                            row = j + 5;
                            break;
                        }
                        if (row != 0)
                            break;
                    }
                for (int j = 0; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                    {
                        if (Top_Right[j,k] == encrypted_msg[i])
                        {
                            col = k + 5;
                            break;
                        }
                        if (col != 0)
                            break;
                    }
                decrypted_msg += aggregate[row,col].ToString();
            }
            return decrypted_msg;
        }
        // main function
        static void Main(string[] args)
        { 
            // read the plain text from the EncryptFourSquare.txt file
            string fileEncrypt = File.ReadAllText(@"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\EncryptFourSquare.txt");

            // file path for the file where the encrypted message will be stored and later on will be decoded
            string fileDecryptPath = @"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\DecryptFourSquare.txt";

            // print initial plaintext
            Console.WriteLine("Initial message: " + fileEncrypt + "\n");

            // user input key for the FourSquare encryption
            Console.WriteLine("Input the first key: ");
            string input = Convert.ToString(Console.ReadLine()); // the user input key should be a string

            Console.WriteLine("Input the second key: ");
            string input2 = Convert.ToString(Console.ReadLine()); // the user input key should be a string

            // call the Encrypt method using as parameters the file and for the key, the user input
            string encryptedMessage = Encrypt(fileEncrypt.ToUpper(), input.ToUpper(), input2.ToUpper());

            // display the encrypted message
            Console.WriteLine("\nThe file EncryptFourSquare.txt was encrypted: \n" + encryptedMessage + "\n");

            // write crypted message to file EncryptFourSquare.txt
            File.WriteAllText(fileDecryptPath, encryptedMessage);
            Console.WriteLine("Writing the encrypted message to DecryptFourSquare.txt ...\n");

            // read text for decryption in the file at a speciffic path
            string fileDecrypt = File.ReadAllText(fileDecryptPath);

            // decrypt the message from fileDecrypt.txt
            string decryptMessage = Decrypt(fileDecrypt.ToUpper(), input.ToUpper(), input2.ToUpper());
            Console.WriteLine("The file DecryptFourSquare.txt was decrypted: \n" + decryptMessage + "\n");
        }
    }
}

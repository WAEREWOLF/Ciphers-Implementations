using System;
using System.IO;

namespace Vigenere
{
    class Program
    {	
		// method that takes a string as input representing the message, the key and a bool that will decide if we want to cypher or decypher the message
		private static string VigenereCypher(string input, string key, bool encipher)
		{
			// scan through the key characters and if are other chars then letters, return null
			for (int i = 0; i < key.Length; ++i)
				if (!char.IsLetter(key[i]))
					return null; 

			string output = string.Empty; // output variable
			int nonLetters = 0, keyIndex; // count the occurence of non letters to skip them
			char offset;
			bool charIsUpper; 

			for (int i = 0; i < input.Length; ++i)
			{
				if (char.IsLetter(input[i])) // verrify if the input message chars are letters, then proceed
				{
					charIsUpper = char.IsUpper(input[i]); // check if a char is lower or uppercase
					
					// the offset for an uppercase letter will be a capital 'A', and for a lowercase letter will be 'a'
					if (charIsUpper)
					{
						offset = 'A';
					}
					else offset = 'a';

					keyIndex = (i - nonLetters) % key.Length;
					// if the message chars are uppercase then the key also must be processed as uppercase, the same for lowercase
					int k = (charIsUpper ? char.ToUpper(key[keyIndex]) : char.ToLower(key[keyIndex])) - offset;
					
					// The bool encipher selects if the message will be decrypted or encrypted, if is false then the message will be decrypted
					if (!encipher)
					{
						k = -k;
					}
					// shift the characrters based on the key and offset using also the Mod method defined below
					char ch = (char)((Mod(((input[i] + k) - offset), 26)) + offset);

					// load the output string with the shifted chars
					output += ch; 
				}
				else
				{
					// this case represents the non letters chars that added to the counter
					output += input[i];
					++nonLetters;
				}
			}

			return output;
		}

		// method used for decryption, the main difference is that the VigenereCypher function is called with the bool encipher being false
		public static string DecryptVigenere(string input, string key)
		{
			return VigenereCypher(input, key, false);
		}

		private static int Mod(int a, int b)
		{
			return (a % b + b) % b;
		}

		static void Main(string[] args)
        {	
			// put some text for encryption in the file at a speciffic path
			string fileEncrypt = File.ReadAllText(@"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\EncryptVigenere.txt");

			// put some text for decryption in the file at a speciffic path
			string fileDecrypt = File.ReadAllText(@"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\DecryptVigenere.txt");

			// user input key for Vigenere encryption
			Console.WriteLine("Input the key to cypher EncryptVigenere.txt: ");
			string input = Convert.ToString(Console.ReadLine()); // the user input key should be a string

			// call the Encrypt method using as parameters the file and for the key, the user input, and the encipher bool is true
			string encryptedMessage = VigenereCypher(fileEncrypt, input, true);

			// disply the encrypted message
			Console.WriteLine("The file was encrypted: \n" + encryptedMessage + "\n");

			Console.WriteLine("Input the key to decrypt DecryptVigenere.txt: ");
			string input2 = Convert.ToString(Console.ReadLine()); // the user input key should be a string

			// decrypt the message from fileDecrypt.txt
			string decryptMessage = DecryptVigenere(fileDecrypt, input2);
			Console.WriteLine("The file was decrypted: \n" + decryptMessage + "\n");
		}
    }
}

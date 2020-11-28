using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class Program
{
	// method that is retrieving the indexes of every char that occurs in the string input
	private static List<int> SearchCharInString(string input, char value)
	{
		List<int> indexes = new List<int>();

		int index = 0;
		while ((index = input.IndexOf(value, index)) != -1)
			indexes.Add(index++); // add the index to the indexes list

		return indexes; // return the list of indexes with the occurences of char 
	}

	// method that is removing from a string all the chars at the specified indexes
	private static string RemoveDuplicates(string input, List<int> indexes)
	{
		for (int i = indexes.Count - 1; i >= 1; i--) // iterate through the list of indexes
			input = input.Remove(indexes[i], 1); // remove one character starting from the current index in the string input

		return input;
	}

	// method that is generating the key matrix based on the provided key
	// removing the duplicates chars that already occur in the input key
	private static char[,] GenerateKeyMatrix(string key)
	{
		char[,] keySquare = new char[5, 5];
		string defaultKeyMatrix = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; // the default matrix key is represented by all the letters in the alphabet
		string tempKey = key.ToUpper(); // making all the letters from the provided key uppercase in order to match the defaultKeyMatrix format

		tempKey = tempKey.Replace("J", "");
		tempKey += defaultKeyMatrix;

		for (int i = 0; i < 25; ++i)
		{
			List<int> indexes = SearchCharInString(tempKey, defaultKeyMatrix[i]);
			tempKey = RemoveDuplicates(tempKey, indexes);
		}

		tempKey = tempKey.Substring(0, 25);

		for (int i = 0; i < 25; ++i)
			keySquare[(i / 5), (i % 5)] = tempKey[i];

		return keySquare;
	}

	// method that is returning the specific position at a row and col of a char
	// "ref" is a keyword that indicates a value was passed by reference
	private static void GetPosition(ref char[,] keySquare, char ch, ref int row, ref int col)
	{
		if (ch == 'J')
			GetPosition(ref keySquare, 'I', ref row, ref col);

		for (int i = 0; i < 5; ++i)
			for (int j = 0; j < 5; ++j)
				if (keySquare[i, j] == ch)
				{
					row = i;
					col = j;
				}
	}

	// Mod method between two int
	private static int Mod(int a, int b)
	{
		return (a % b + b) % b;
	}

	// The algorithm will split into pairs of two letters, so below are the possible cases when testing this pairs

	// the case when the both letters are on the same column => we take the letter below each one
	private static char[] SameColumn(ref char[,] keySquare, int col, int row1, int row2, int encipher)
	{
		return new char[] { keySquare[Mod((row1 + encipher), 5), col], keySquare[Mod((row2 + encipher), 5), col] };
	}

	// the case when both letters are on the same row => we take the letter to the right of each one
	private static char[] SameRow(ref char[,] keySquare, int row, int col1, int col2, int encipher)
	{
		return new char[] { keySquare[row, Mod((col1 + encipher), 5)], keySquare[row, Mod((col2 + encipher), 5)] };
	}

	// the case when both letters are on the same row and column
	private static char[] SameRowColumn(ref char[,] keySquare, int row, int col, int encipher)
	{
		return new char[] { keySquare[Mod((row + encipher), 5), Mod((col + encipher), 5)], keySquare[Mod((row + encipher), 5), Mod((col + encipher), 5)] };
	}

	// the case when both letters are on different row and col
	private static char[] DifferentRowColumn(ref char[,] keySquare, int row1, int col1, int row2, int col2)
	{
		return new char[] { keySquare[row1, col2], keySquare[row2, col1] };
	}

	// method that is removing other characters then letters in a given input string
	private static string RemoveOtherChars(string input)
	{
		string output = input;

		for (int i = 0; i < output.Length; ++i)
			if (!char.IsLetter(output[i]))
				output = output.Remove(i, 1);

		return output;
	}

	// method that is printing the result
	private static string Print(string input, string output)
	{
		StringBuilder result = new StringBuilder(output);

		for (int i = 0; i < input.Length; ++i)
		{
			if (!char.IsLetter(input[i]))
				result = result.Insert(i, input[i].ToString());

			if (char.IsLower(input[i]))
				result[i] = char.ToLower(result[i]);
		}

		return result.ToString();
	}

	// main method that is implementing Playfair Cipher using the other methods as helper functions
	private static string PlayfairCipher(string input, string key, bool toEncrypt)
	{
		string result = string.Empty;
		char[,] keySquare = GenerateKeyMatrix(key); // generates the key matrix
		string tempInput = RemoveOtherChars(input); // removes the other characters in the input string
		
		// testing the boolean toEncrypt to find out the method: Encryption or Decryption
		int method;
		if (toEncrypt)
		{
			method = 1; // we want ecryption
		}
		else method = -1; // we want  decryption

		if ((tempInput.Length % 2) != 0)
			tempInput += "X";

		for (int i = 0; i < tempInput.Length; i += 2)
		{
			// initializing the rows and cols with 0
			int row1 = 0;
			int col1 = 0;
			int row2 = 0;
			int col2 = 0;

			// getting the positions of the char on the row and column
			GetPosition(ref keySquare, char.ToUpper(tempInput[i]), ref row1, ref col1);
			GetPosition(ref keySquare, char.ToUpper(tempInput[i + 1]), ref row2, ref col2);

			// testing the possible cases
			if (row1 == row2 && col1 == col2)
			{
				result += new string(SameRowColumn(ref keySquare, row1, col1, method));
			}
			else if (row1 == row2)
			{
				result += new string(SameRow(ref keySquare, row1, col1, col2, method));
			}
			else if (col1 == col2)
			{
				result += new string(SameColumn(ref keySquare, col1, row1, row2, method));
			}
			else
			{
				result += new string(DifferentRowColumn(ref keySquare, row1, col1, row2, col2));
			}
		}

		result = Print(input, result); // getting the result string

		return result; // return the result string
	}

	// method that is encrypting an input string based on a specific key 
	public static string Encrypt(string input, string key)
	{
		return PlayfairCipher(input, key, true); // it's required to call the PlayfairCypher method with the toEncrypt bool as true
	}

	// method that is decrypting an input string based on a key 
	public static string Decrypt(string input, string key)
	{
		return PlayfairCipher(input, key, false); // it's required to call the PlayfairCypher method with the toEncrypt bool as false
	}

	public static void Main()
	{
		
		// put some text for encryption in the file at a speciffic path
		string fileEncrypt = File.ReadAllText(@"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\EncryptPlayfair.txt");

		// put some text for decryption in the file at a speciffic path
		string fileDecrypt = File.ReadAllText(@"D:\FACULTATE AUTOMATICA\ANI de studiu\ANUL 4\DS\DecryptPlayfair.txt");

		// user input key for Playfair encryption
		Console.WriteLine("Input the key to cypher EncryptPlayfair.txt: ");
		string input = Convert.ToString(Console.ReadLine()); // the user input key should be a string

		// call the Encrypt method using as parameters the file and for the key, the user input
		string encryptedMessage = Encrypt(fileEncrypt, input);

		// disply the encrypted message
		Console.WriteLine("The file was encrypted: \n" + encryptedMessage + "\n");

		Console.WriteLine("Input the key to decrypt DecryptPlayfair.txt: ");
		string input2 = Convert.ToString(Console.ReadLine()); // the user input key should be a string

		// decrypt the message from fileDecrypt.txt
		string decryptMessage = Decrypt(fileDecrypt, input2);
		Console.WriteLine("The file was decrypted: \n" + decryptMessage + "\n");

	}
}
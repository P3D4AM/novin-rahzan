using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Stimulsoft.Base;

public class StiEncryption
{
	private const int rand_m = 714025;

	private const int rand_a = 4096;

	private const int rand_c = 150889;

	public static byte[] Encrypt(byte[] src, byte[] key)
	{
		return EncryptAdv(src, key);
	}

	public static byte[] Encrypt(byte[] src, string password)
	{
		byte[] keyFromPassword = GetKeyFromPassword(password);
		return EncryptAdv(src, keyFromPassword);
	}

	public static string Encrypt(string src, string password)
	{
		if (src == null)
		{
			return null;
		}
		byte[] bytes = Encoding.UTF8.GetBytes(src);
		byte[] keyFromPassword = GetKeyFromPassword(password);
		byte[] inArray = EncryptAdv(bytes, keyFromPassword);
		return Convert.ToBase64String(inArray);
	}

	public static byte[] Decrypt(byte[] src, byte[] key)
	{
		return DecryptAdv(src, key);
	}

	public static byte[] Decrypt(byte[] src, string password)
	{
		byte[] keyFromPassword = GetKeyFromPassword(password);
		return DecryptAdv(src, keyFromPassword);
	}

	public static string Decrypt(string src, string password)
	{
		if (src == null)
		{
			return null;
		}
		byte[] src2 = Convert.FromBase64String(src);
		byte[] keyFromPassword = GetKeyFromPassword(password);
		byte[] bytes = DecryptAdv(src2, keyFromPassword);
		return Encoding.UTF8.GetString(bytes);
	}

	public static byte[] GenerateRandomKey()
	{
		byte[] array = new byte[32];
		RandomNumberGenerator.Create();
		Random random = new Random();
		for (int i = 0; i < 32; i++)
		{
			array[i] = (byte)random.Next(0, 255);
		}
		return array;
	}

	private static byte[] EncryptAdv(byte[] src, byte[] key)
	{
		if (src == null)
		{
			return null;
		}
		byte[] array = new byte[src.Length];
		array = CryptRandom(src, key, encrypt: true);
		array = CryptXor(array, key);
		return CryptShift(array, key, encrypt: true);
	}

	private static byte[] DecryptAdv(byte[] src, byte[] key)
	{
		if (src == null)
		{
			return null;
		}
		byte[] array = new byte[src.Length];
		array = CryptShift(src, key, encrypt: false);
		array = CryptXor(array, key);
		return CryptRandom(array, key, encrypt: false);
	}

	private static byte[] CryptXor(byte[] src, byte[] key)
	{
		byte[] array = new byte[src.Length];
		int num = 0;
		int num2 = 0;
		while (num < src.Length)
		{
			if (num2 >= key.Length)
			{
				num2 = 0;
			}
			array[num] = (byte)(src[num] ^ key[num2]);
			num++;
			num2++;
		}
		return array;
	}

	private static byte[] CryptShift(byte[] src, byte[] key, bool encrypt)
	{
		byte[] array = new byte[src.Length];
		int num = 0;
		int num2 = 0;
		while (num < src.Length)
		{
			if (num2 >= key.Length)
			{
				num2 = 0;
			}
			if (encrypt)
			{
				array[num] = ShiftLeft(src[num], key[num2]);
			}
			else
			{
				array[num] = ShiftRight(src[num], key[num2]);
			}
			num++;
			num2++;
		}
		return array;
	}

	private static byte ShiftLeft(byte value, byte count)
	{
		int num = value << (count & 7);
		num = (num & 0xFF) | ((num & 0xFF00) >> 8);
		return (byte)num;
	}

	private static byte ShiftRight(byte value, byte count)
	{
		int num = value << 8 - (count & 7);
		num = (num & 0xFF) | ((num & 0xFF00) >> 8);
		return (byte)num;
	}

	private static byte[] CryptRandom(byte[] src, byte[] key, bool encrypt)
	{
		byte[] array = new byte[src.Length];
		int i = 0;
		uint randomSeed = SetRandomSeed(key);
		int[] mixArray = GetMixArray(src.Length, randomSeed);
		for (; i < src.Length; i++)
		{
			if (encrypt)
			{
				array[i] = src[mixArray[i]];
			}
			else
			{
				array[mixArray[i]] = src[i];
			}
		}
		return array;
	}

	private static int[] GetMixArray(int count, uint randomSeed)
	{
		int[] array = new int[count];
		int[] array2 = new int[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = i;
		}
		for (int j = 0; j < count; j++)
		{
			int random = GetRandom(0, count - j - 1, ref randomSeed);
			array2[j] = array[random];
			array[random] = array[count - j - 1];
		}
		return array2;
	}

	private static uint SetRandomSeed(byte[] key)
	{
		uint num = (uint)(key[0] | (key[1] << 8) | (key[^2] << 16) | (key[^1] << 24));
		return num % 714025;
	}

	private static int GetRandom(int min, int max, ref uint randomSeed)
	{
		randomSeed = (randomSeed * 4096 + 150889) % 714025;
		return (int)(min + (max - min + 1) * randomSeed / 714025);
	}

	private static byte[] GetKeyFromPassword(string password)
	{
		MemoryStream memoryStream = new MemoryStream();
		byte[] array = StiMD5Helper.ComputeHash(Encoding.UTF8.GetBytes(password));
		memoryStream.Write(array, 0, array.Length);
		for (int i = array.Length; i < password.Length; i += array.Length)
		{
			array = StiMD5Helper.ComputeHash(Encoding.UTF8.GetBytes(password.Substring(0, i)));
			memoryStream.Write(array, 0, (i + array.Length < password.Length) ? array.Length : (password.Length - i));
		}
		byte[] result = memoryStream.ToArray();
		memoryStream.Close();
		memoryStream.Dispose();
		return result;
	}
}

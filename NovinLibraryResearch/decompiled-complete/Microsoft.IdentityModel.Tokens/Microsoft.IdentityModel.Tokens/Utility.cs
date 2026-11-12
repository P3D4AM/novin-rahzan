using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public static class Utility
{
	public const string Empty = "empty";

	public const string Null = "null";

	public static byte[] CloneByteArray(this byte[] src)
	{
		if (src == null)
		{
			throw LogHelper.LogArgumentNullException("src");
		}
		return (byte[])src.Clone();
	}

	internal static string SerializeAsSingleCommaDelimitedString(IEnumerable<string> strings)
	{
		if (strings == null)
		{
			return "null";
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		foreach (string @string in strings)
		{
			if (flag)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}", @string ?? "null");
				flag = false;
			}
			else
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, ", {0}", @string ?? "null");
			}
		}
		if (flag)
		{
			return "empty";
		}
		return stringBuilder.ToString();
	}

	public static bool IsHttps(string address)
	{
		if (string.IsNullOrEmpty(address))
		{
			return false;
		}
		try
		{
			new Uri(address);
			return IsHttps(new Uri(address));
		}
		catch (UriFormatException)
		{
			return false;
		}
	}

	public static bool IsHttps(Uri uri)
	{
		if (uri == null)
		{
			return false;
		}
		return uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);
	}

	[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
	public static bool AreEqual(byte[] a, byte[] b)
	{
		byte[] array = new byte[32]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
			20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
			30, 31
		};
		byte[] array2 = new byte[32]
		{
			31, 30, 29, 28, 27, 26, 25, 24, 23, 22,
			21, 20, 19, 18, 17, 16, 15, 14, 13, 12,
			11, 10, 9, 8, 7, 6, 5, 4, 3, 2,
			1, 0
		};
		int num = 0;
		byte[] array3;
		byte[] array4;
		if (a == null || b == null || a.Length != b.Length)
		{
			array3 = array;
			array4 = array2;
		}
		else
		{
			array3 = a;
			array4 = b;
		}
		for (int i = 0; i < array3.Length; i++)
		{
			num |= array3[i] ^ array4[i];
		}
		return num == 0;
	}

	[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
	internal static bool AreEqual(byte[] a, byte[] b, int length)
	{
		byte[] array = new byte[32]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
			20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
			30, 31
		};
		byte[] array2 = new byte[32]
		{
			31, 30, 29, 28, 27, 26, 25, 24, 23, 22,
			21, 20, 19, 18, 17, 16, 15, 14, 13, 12,
			11, 10, 9, 8, 7, 6, 5, 4, 3, 2,
			1, 0
		};
		int num = 0;
		int num2 = 0;
		byte[] array3;
		byte[] array4;
		if (a == null || b == null || a.Length < length || b.Length < length)
		{
			array3 = array;
			array4 = array2;
			num2 = array3.Length;
		}
		else
		{
			array3 = a;
			array4 = b;
			num2 = length;
		}
		for (int i = 0; i < num2; i++)
		{
			num |= array3[i] ^ array4[i];
		}
		return num == 0;
	}

	internal static byte[] ConvertToBigEndian(long i)
	{
		byte[] bytes = BitConverter.GetBytes(i);
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse((Array)bytes);
		}
		return bytes;
	}

	internal static byte[] Xor(byte[] a, byte[] b, int offset, bool inPlace)
	{
		if (inPlace)
		{
			for (int i = 0; i < a.Length; i++)
			{
				a[i] ^= b[offset + i];
			}
			return a;
		}
		byte[] array = new byte[a.Length];
		for (int j = 0; j < a.Length; j++)
		{
			array[j] = (byte)(a[j] ^ b[offset + j]);
		}
		return array;
	}

	internal static void Zero(byte[] byteArray)
	{
		for (int i = 0; i < byteArray.Length; i++)
		{
			byteArray[i] = 0;
		}
	}

	internal static byte[] GenerateSha256Hash(string input)
	{
		using SHA256 sHA = SHA256.Create();
		return sHA.ComputeHash(Encoding.UTF8.GetBytes(input));
	}
}

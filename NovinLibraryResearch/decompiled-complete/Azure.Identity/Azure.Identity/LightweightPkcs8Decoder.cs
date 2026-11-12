using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Azure.Identity;

internal class LightweightPkcs8Decoder
{
	private static readonly byte[] s_derIntegerZero = new byte[3] { 2, 1, 0 };

	private static readonly byte[] s_rsaAlgorithmId = new byte[15]
	{
		48, 13, 6, 9, 42, 134, 72, 134, 247, 13,
		1, 1, 1, 5, 0
	};

	private static int ReadLength(byte[] data, ref int offset)
	{
		byte b = data[offset++];
		if (b < 128)
		{
			return b;
		}
		int num = b & 0x7F;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			if (num2 > 65535)
			{
				throw new InvalidDataException("Invalid PKCS#8 Data");
			}
			num2 <<= 8;
			num2 |= data[offset++];
		}
		return num2;
	}

	private static byte[] ReadUnsignedInteger(byte[] data, ref int offset, int targetSize = 0)
	{
		if (data[offset++] != 2)
		{
			throw new InvalidDataException("Invalid PKCS#8 Data");
		}
		int num = ReadLength(data, ref offset);
		if (num < 1 || data[offset] >= 128)
		{
			throw new InvalidDataException("Invalid PKCS#8 Data");
		}
		byte[] array;
		if (num == 1)
		{
			array = new byte[num];
			array[0] = data[offset++];
			return array;
		}
		if (data[offset] == 0)
		{
			offset++;
			num--;
		}
		if (targetSize != 0)
		{
			if (num > targetSize)
			{
				throw new InvalidDataException("Invalid PKCS#8 Data");
			}
			array = new byte[targetSize];
		}
		else
		{
			array = new byte[num];
		}
		Buffer.BlockCopy(data, offset, array, array.Length - num, num);
		offset += num;
		return array;
	}

	private static void ConsumeFullPayloadTag(byte[] data, ref int offset, byte tagValue)
	{
		if (data[offset++] != tagValue)
		{
			throw new InvalidDataException("Invalid PKCS#8 Data");
		}
		int num = ReadLength(data, ref offset);
		if (data.Length - offset != num)
		{
			throw new InvalidDataException("Invalid PKCS#8 Data");
		}
	}

	private static void ConsumeMatch(byte[] data, ref int offset, byte[] toMatch)
	{
		if (data.Length - offset > toMatch.Length && data.Skip(offset).Take(toMatch.Length).SequenceEqual(toMatch))
		{
			offset += toMatch.Length;
			return;
		}
		throw new InvalidDataException("Invalid PKCS#8 Data");
	}

	public static RSA DecodeRSAPkcs8(byte[] pkcs8Bytes)
	{
		int offset = 0;
		ConsumeFullPayloadTag(pkcs8Bytes, ref offset, 48);
		ConsumeMatch(pkcs8Bytes, ref offset, s_derIntegerZero);
		ConsumeMatch(pkcs8Bytes, ref offset, s_rsaAlgorithmId);
		ConsumeFullPayloadTag(pkcs8Bytes, ref offset, 4);
		ConsumeFullPayloadTag(pkcs8Bytes, ref offset, 48);
		ConsumeMatch(pkcs8Bytes, ref offset, s_derIntegerZero);
		RSAParameters parameters = default(RSAParameters);
		parameters.Modulus = ReadUnsignedInteger(pkcs8Bytes, ref offset);
		parameters.Exponent = ReadUnsignedInteger(pkcs8Bytes, ref offset);
		parameters.D = ReadUnsignedInteger(pkcs8Bytes, ref offset, parameters.Modulus.Length);
		int targetSize = (parameters.Modulus.Length + 1) / 2;
		parameters.P = ReadUnsignedInteger(pkcs8Bytes, ref offset, targetSize);
		parameters.Q = ReadUnsignedInteger(pkcs8Bytes, ref offset, targetSize);
		parameters.DP = ReadUnsignedInteger(pkcs8Bytes, ref offset, targetSize);
		parameters.DQ = ReadUnsignedInteger(pkcs8Bytes, ref offset, targetSize);
		parameters.InverseQ = ReadUnsignedInteger(pkcs8Bytes, ref offset, targetSize);
		if (offset != pkcs8Bytes.Length)
		{
			throw new InvalidDataException("Invalid PKCS#8 Data");
		}
		RSA rSA = RSA.Create();
		rSA.ImportParameters(parameters);
		return rSA;
	}
}

using System;
using System.Security.Cryptography;

namespace ExcelDataReader.Core.OfficeCrypto;

internal class XorManaged : SymmetricAlgorithm
{
	internal class XorTransform : ICryptoTransform, IDisposable
	{
		public int InputBlockSize => 1024;

		public int OutputBlockSize => 1024;

		public bool CanTransformMultipleBlocks => false;

		public bool CanReuseTransform => false;

		public int XorArrayIndex { get; set; }

		private byte[] XorArray { get; }

		public XorTransform(byte[] key, int xorArrayIndex)
		{
			XorArray = key;
			XorArrayIndex = xorArrayIndex;
		}

		public void Dispose()
		{
		}

		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			for (int i = 0; i < inputCount; i++)
			{
				byte b = inputBuffer[inputOffset + i];
				b = RotateLeft(b, 3);
				b ^= XorArray[XorArrayIndex % 16];
				outputBuffer[outputOffset + i] = b;
				XorArrayIndex++;
			}
			return inputCount;
		}

		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			byte[] array = new byte[inputCount];
			TransformBlock(inputBuffer, inputOffset, inputCount, array, 0);
			return array;
		}
	}

	private static byte[] padArray = new byte[15]
	{
		187, 255, 255, 186, 255, 255, 185, 128, 0, 190,
		15, 0, 191, 15, 0
	};

	private static ushort[] initialCode = new ushort[15]
	{
		57840, 7439, 52380, 33984, 4364, 3600, 61902, 12606, 6258, 57657,
		54287, 34041, 10252, 43370, 20163
	};

	private static ushort[] xorMatrix = new ushort[105]
	{
		44796, 19929, 39858, 10053, 20106, 40212, 10761, 31585, 63170, 64933,
		60267, 50935, 40399, 11199, 17763, 35526, 1453, 2906, 5812, 11624,
		23248, 885, 1770, 3540, 7080, 14160, 28320, 56640, 55369, 41139,
		20807, 41614, 21821, 43642, 17621, 28485, 56970, 44341, 19019, 38038,
		14605, 29210, 60195, 50791, 40175, 10751, 21502, 43004, 24537, 18387,
		36774, 3949, 7898, 15796, 31592, 63184, 47201, 24803, 49606, 37805,
		14203, 28406, 56812, 17824, 35648, 1697, 3394, 6788, 13576, 27152,
		43601, 17539, 35078, 557, 1114, 2228, 4456, 30388, 60776, 51953,
		34243, 7079, 14158, 28316, 14128, 28256, 56512, 43425, 17251, 34502,
		7597, 13105, 26210, 52420, 35241, 883, 1766, 3532, 4129, 8258,
		16516, 33032, 4657, 9314, 18628
	};

	public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
	{
		return new XorTransform(rgbKey, 0);
	}

	public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
	{
		throw new NotImplementedException();
	}

	public override void GenerateIV()
	{
		throw new NotImplementedException();
	}

	public override void GenerateKey()
	{
		throw new NotImplementedException();
	}

	internal static ushort CreatePasswordVerifier_Method1(byte[] passwordBytes)
	{
		byte[] array = CryptoHelpers.Combine(new byte[1] { (byte)passwordBytes.Length }, passwordBytes);
		ushort num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			byte b = array[array.Length - 1 - i];
			ushort num2 = (ushort)(((num & 0x4000u) != 0) ? 1u : 0u);
			ushort num3 = (ushort)(num * 2);
			num3 = (ushort)(num3 & 0x7FFFu);
			num = (ushort)((ushort)(num2 | num3) ^ b);
		}
		return (ushort)(num ^ 0xCE4Bu);
	}

	internal static ushort CreateXorKey_Method1(byte[] passwordBytes)
	{
		ushort num = initialCode[passwordBytes.Length - 1];
		int num2 = 104;
		for (int i = 0; i < passwordBytes.Length; i++)
		{
			byte b = passwordBytes[passwordBytes.Length - 1 - i];
			for (int j = 0; j < 7; j++)
			{
				if ((b & 0x40u) != 0)
				{
					num ^= xorMatrix[num2];
				}
				b *= 2;
				num2--;
			}
		}
		return num;
	}

	internal static byte[] CreateXorArray_Method1(byte[] passwordBytes)
	{
		_ = passwordBytes.Length;
		byte[] array = new byte[16];
		Array.Copy(passwordBytes, 0, array, 0, passwordBytes.Length);
		Array.Copy(padArray, 0, array, passwordBytes.Length, padArray.Length - passwordBytes.Length + 1);
		ushort num = CreateXorKey_Method1(passwordBytes);
		byte[] array2 = new byte[2]
		{
			(byte)(num & 0xFFu),
			(byte)((uint)(num >> 8) & 0xFFu)
		};
		int shift = 2;
		for (int i = 0; i < array.Length; i++)
		{
			array[i] ^= array2[i & 1];
			array[i] = RotateLeft(array[i], shift);
		}
		return array;
	}

	private static byte RotateLeft(byte b, int shift)
	{
		return (byte)((uint)((b << shift) | (b >> 8 - shift)) & 0xFFu);
	}
}

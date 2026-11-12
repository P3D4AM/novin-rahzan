using System;
using System.Security.Cryptography;

namespace ExcelDataReader.Core.OfficeCrypto;

internal class RC4Managed : SymmetricAlgorithm
{
	internal class RC4Transform : ICryptoTransform, IDisposable
	{
		private readonly byte[] _s = new byte[256];

		private int _index1;

		private int _index2;

		public int InputBlockSize => 1024;

		public int OutputBlockSize => 1024;

		public bool CanTransformMultipleBlocks => false;

		public bool CanReuseTransform => false;

		public byte[] Key { get; }

		public RC4Transform(byte[] key)
		{
			Key = key;
			for (int i = 0; i < _s.Length; i++)
			{
				_s[i] = (byte)i;
			}
			int j = 0;
			int num = 0;
			for (; j < 256; j++)
			{
				num = (num + key[j % key.Length] + _s[j]) & 0xFF;
				Swap(_s, j, num);
			}
		}

		public void Dispose()
		{
		}

		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			for (int i = 0; i < inputCount; i++)
			{
				byte b = Output();
				outputBuffer[outputOffset + i] = (byte)(inputBuffer[inputOffset + i] ^ b);
			}
			return inputCount;
		}

		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			byte[] array = new byte[inputCount];
			TransformBlock(inputBuffer, inputOffset, inputCount, array, 0);
			return array;
		}

		private static void Swap(byte[] s, int i, int j)
		{
			byte b = s[i];
			s[i] = s[j];
			s[j] = b;
		}

		private byte Output()
		{
			_index1 = (_index1 + 1) & 0xFF;
			_index2 = (_index2 + _s[_index1]) & 0xFF;
			Swap(_s, _index1, _index2);
			return _s[(_s[_index1] + _s[_index2]) & 0xFF];
		}
	}

	public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
	{
		return new RC4Transform(rgbKey);
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
}

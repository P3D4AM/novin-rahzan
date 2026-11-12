using System;
using System.Security.Cryptography;
using System.Text;
using SharpCompress.Crypto;

namespace SharpCompress.Common.Rar;

internal class RarRijndael : IDisposable
{
	internal const int CRYPTO_BLOCK_SIZE = 16;

	private readonly string _password;

	private readonly byte[] _salt;

	private byte[] _aesInitializationVector;

	private RijndaelEngine _rijndael;

	private RarRijndael(string password, byte[] salt)
	{
		_password = password;
		_salt = salt;
	}

	private void Initialize()
	{
		_rijndael = new RijndaelEngine();
		_aesInitializationVector = new byte[16];
		int num = 2 * _password.Length;
		byte[] array = new byte[num + 8];
		byte[] bytes = Encoding.UTF8.GetBytes(_password);
		for (int i = 0; i < _password.Length; i++)
		{
			array[i * 2] = bytes[i];
			array[i * 2 + 1] = 0;
		}
		for (int j = 0; j < _salt.Length; j++)
		{
			array[j + num] = _salt[j];
		}
		byte[] array2 = new byte[(array.Length + 3) * 262144];
		byte[] array3;
		for (int k = 0; k < 262144; k++)
		{
			array.CopyTo(array2, k * (array.Length + 3));
			array2[k * (array.Length + 3) + array.Length] = (byte)k;
			array2[k * (array.Length + 3) + array.Length + 1] = (byte)(k >> 8);
			array2[k * (array.Length + 3) + array.Length + 2] = (byte)(k >> 16);
			if (k % 16384 == 0)
			{
				array3 = SHA1.Create().ComputeHash(array2, 0, (k + 1) * (array.Length + 3));
				_aesInitializationVector[k / 16384] = array3[19];
			}
		}
		array3 = SHA1.Create().ComputeHash(array2);
		byte[] array4 = new byte[16];
		for (int l = 0; l < 4; l++)
		{
			for (int m = 0; m < 4; m++)
			{
				array4[l * 4 + m] = (byte)((((array3[l * 4] * 16777216) & 0xFF000000u) | (uint)((array3[l * 4 + 1] * 65536) & 0xFF0000) | (uint)((array3[l * 4 + 2] * 256) & 0xFF00) | (uint)(array3[l * 4 + 3] & 0xFF)) >> m * 8);
			}
		}
		_rijndael.Init(forEncryption: false, new KeyParameter(array4));
	}

	public static RarRijndael InitializeFrom(string password, byte[] salt)
	{
		RarRijndael rarRijndael = new RarRijndael(password, salt);
		rarRijndael.Initialize();
		return rarRijndael;
	}

	public byte[] ProcessBlock(ReadOnlySpan<byte> cipherText)
	{
		Span<byte> output = stackalloc byte[16];
		byte[] array = new byte[16];
		_rijndael.ProcessBlock(cipherText, output);
		for (int i = 0; i < 16; i++)
		{
			array[i] = (byte)(output[i] ^ _aesInitializationVector[i % 16]);
		}
		for (int j = 0; j < _aesInitializationVector.Length; j++)
		{
			_aesInitializationVector[j] = cipherText[j];
		}
		return array;
	}

	public void Dispose()
	{
	}
}

using System;
using System.IO;
using System.Security.Cryptography;

namespace ExcelDataReader.Core.OfficeCrypto;

internal static class CryptoHelpers
{
	public static byte[] HashBytes(byte[] bytes, HashIdentifier hashAlgorithm)
	{
		HashAlgorithm hashAlgorithm2 = hashAlgorithm switch
		{
			HashIdentifier.SHA512 => SHA512.Create(), 
			HashIdentifier.SHA384 => SHA384.Create(), 
			HashIdentifier.SHA256 => SHA256.Create(), 
			HashIdentifier.SHA1 => SHA1.Create(), 
			HashIdentifier.MD5 => MD5.Create(), 
			_ => throw new InvalidOperationException("Unsupported hash algorithm"), 
		};
		using (hashAlgorithm2)
		{
			return hashAlgorithm2.ComputeHash(bytes);
		}
	}

	public static byte[] Combine(params byte[][] arrays)
	{
		int num = 0;
		for (int i = 0; i < arrays.Length; i++)
		{
			num += arrays[i].Length;
		}
		byte[] array = new byte[num];
		int num2 = 0;
		foreach (byte[] array2 in arrays)
		{
			Buffer.BlockCopy(array2, 0, array, num2, array2.Length);
			num2 += array2.Length;
		}
		return array;
	}

	public static SymmetricAlgorithm CreateCipher(CipherIdentifier identifier, int keySize, int blockSize, CipherMode mode)
	{
		return identifier switch
		{
			CipherIdentifier.RC4 => new RC4Managed(), 
			CipherIdentifier.DES3 => InitCipher(TripleDES.Create(), keySize, blockSize, mode), 
			CipherIdentifier.RC2 => InitCipher(RC2.Create(), keySize, blockSize, mode), 
			CipherIdentifier.DES => InitCipher(DES.Create(), keySize, blockSize, mode), 
			CipherIdentifier.AES => InitCipher(new RijndaelManaged(), keySize, blockSize, mode), 
			_ => throw new InvalidOperationException("Unsupported encryption method: " + identifier), 
		};
	}

	public static SymmetricAlgorithm InitCipher(SymmetricAlgorithm cipher, int keySize, int blockSize, CipherMode mode)
	{
		cipher.KeySize = keySize;
		cipher.BlockSize = blockSize;
		cipher.Mode = mode;
		cipher.Padding = PaddingMode.Zeros;
		return cipher;
	}

	public static byte[] DecryptBytes(SymmetricAlgorithm algo, byte[] bytes, byte[] key, byte[] iv)
	{
		using ICryptoTransform transform = algo.CreateDecryptor(key, iv);
		return DecryptBytes(transform, bytes);
	}

	public static byte[] DecryptBytes(ICryptoTransform transform, byte[] bytes)
	{
		int num = bytes.Length;
		using MemoryStream stream = new MemoryStream(bytes, 0, num);
		using CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
		byte[] array = new byte[num];
		cryptoStream.Read(array, 0, num);
		return array;
	}
}

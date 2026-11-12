using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ExcelDataReader.Core.OfficeCrypto;

internal class RC4Encryption : EncryptionInfo
{
	public byte[] Salt { get; }

	public byte[] EncryptedVerifier { get; }

	public byte[] EncryptedVerifierHash { get; }

	public override bool IsXor => false;

	public RC4Encryption(byte[] bytes)
	{
		Salt = new byte[16];
		EncryptedVerifier = new byte[16];
		EncryptedVerifierHash = new byte[16];
		Array.Copy(bytes, 4, Salt, 0, 16);
		Array.Copy(bytes, 20, EncryptedVerifier, 0, 16);
		Array.Copy(bytes, 36, EncryptedVerifierHash, 0, 16);
	}

	public static byte[] GenerateSecretKey(string password, byte[] salt)
	{
		if (password.Length > 16)
		{
			password = password.Substring(0, 16);
		}
		byte[] array = CryptoHelpers.HashBytes(Encoding.Unicode.GetBytes(password), HashIdentifier.MD5);
		Array.Resize(ref array, 5);
		array = CryptoHelpers.Combine(array, salt, array, salt, array, salt, array, salt, array, salt, array, salt, array, salt, array, salt, array, salt, array, salt, array, salt, array, salt, array, salt, array, salt, array, salt, array, salt);
		array = CryptoHelpers.HashBytes(array, HashIdentifier.MD5);
		Array.Resize(ref array, 5);
		return array;
	}

	public override SymmetricAlgorithm CreateCipher()
	{
		return CryptoHelpers.CreateCipher(CipherIdentifier.RC4, 0, 0, (CipherMode)0);
	}

	public override Stream CreateEncryptedPackageStream(Stream stream, byte[] secretKey)
	{
		throw new NotImplementedException();
	}

	public override byte[] GenerateBlockKey(int blockNumber, byte[] secretKey)
	{
		return CryptoHelpers.HashBytes(CryptoHelpers.Combine(secretKey, BitConverter.GetBytes(blockNumber)), HashIdentifier.MD5);
	}

	public override byte[] GenerateSecretKey(string password)
	{
		return GenerateSecretKey(password, Salt);
	}

	public override bool VerifyPassword(string password)
	{
		byte[] secretKey = GenerateSecretKey(password);
		byte[] rgbKey = GenerateBlockKey(0, secretKey);
		using SymmetricAlgorithm symmetricAlgorithm = CryptoHelpers.CreateCipher(CipherIdentifier.RC4, 0, 0, (CipherMode)0);
		using ICryptoTransform transform = symmetricAlgorithm.CreateDecryptor(rgbKey, null);
		byte[] bytes = CryptoHelpers.DecryptBytes(transform, EncryptedVerifier);
		byte[] array = CryptoHelpers.DecryptBytes(transform, EncryptedVerifierHash);
		byte[] array2 = CryptoHelpers.HashBytes(bytes, HashIdentifier.MD5);
		for (int i = 0; i < 16; i++)
		{
			if (array[i] != array2[i])
			{
				return false;
			}
		}
		return true;
	}
}

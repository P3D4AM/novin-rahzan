using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ExcelDataReader.Core.OfficeCrypto;

internal class StandardEncryption : EncryptionInfo
{
	private enum StandardProvider
	{
		Default = 0,
		RC4 = 1,
		AES = 24
	}

	private enum StandardCipher
	{
		Default = 0,
		AES128 = 26126,
		AES192 = 26127,
		AES256 = 26128,
		RC4 = 26625
	}

	private enum StandardHash
	{
		Default = 0,
		SHA1 = 32772
	}

	private enum EncryptionHeaderFlags : uint
	{
		CryptoAPI = 4u,
		DocProps = 8u,
		External = 0x10u,
		AES = 0x20u
	}

	private const int AesBlockSize = 128;

	private const int RC4BlockSize = 8;

	public CipherIdentifier CipherAlgorithm { get; set; }

	public HashIdentifier HashAlgorithm { get; set; }

	public int BlockSize { get; set; }

	public int KeySize { get; set; }

	public string CSPName { get; set; }

	public byte[] SaltValue { get; set; }

	public byte[] Verifier { get; set; }

	public byte[] VerifierHash { get; set; }

	public int VerifierHashBytesNeeded { get; set; }

	public override bool IsXor => false;

	private EncryptionHeaderFlags Flags { get; set; }

	public StandardEncryption(byte[] bytes)
	{
		Flags = (EncryptionHeaderFlags)BitConverter.ToUInt32(bytes, 4);
		int num = BitConverter.ToInt32(bytes, 8);
		StandardCipher standardCipher = (StandardCipher)BitConverter.ToUInt32(bytes, 20);
		StandardHash standardHash = (StandardHash)BitConverter.ToUInt32(bytes, 24);
		if ((Flags & EncryptionHeaderFlags.External) == 0 && (standardHash == StandardHash.Default || standardHash == StandardHash.SHA1))
		{
			HashAlgorithm = HashIdentifier.SHA1;
		}
		KeySize = BitConverter.ToInt32(bytes, 28);
		BitConverter.ToUInt32(bytes, 32);
		CSPName = Encoding.Unicode.GetString(bytes, 44, num - 44 + 12);
		int num2 = BitConverter.ToInt32(bytes, 12 + num);
		SaltValue = new byte[num2];
		Array.Copy(bytes, 12 + num + 4, SaltValue, 0, num2);
		Verifier = new byte[16];
		Array.Copy(bytes, 12 + num + 4 + num2, Verifier, 0, 16);
		VerifierHashBytesNeeded = BitConverter.ToInt32(bytes, 12 + num + 4 + num2 + 16);
		int num3 = (((Flags & EncryptionHeaderFlags.AES) != 0) ? 32 : 20);
		switch (standardCipher)
		{
		case StandardCipher.RC4:
			BlockSize = 8;
			num3 = 20;
			break;
		case StandardCipher.AES128:
		case StandardCipher.AES192:
		case StandardCipher.AES256:
			BlockSize = 128;
			num3 = 32;
			break;
		}
		VerifierHash = new byte[num3];
		Array.Copy(bytes, 12 + num + 4 + num2 + 16 + 4, VerifierHash, 0, num3);
		if ((Flags & EncryptionHeaderFlags.External) != 0)
		{
			return;
		}
		switch (standardCipher)
		{
		case StandardCipher.Default:
			if ((Flags & EncryptionHeaderFlags.AES) != 0)
			{
				CipherAlgorithm = CipherIdentifier.AES;
			}
			else if ((Flags & EncryptionHeaderFlags.CryptoAPI) != 0)
			{
				CipherAlgorithm = CipherIdentifier.RC4;
			}
			break;
		case StandardCipher.AES128:
		case StandardCipher.AES192:
		case StandardCipher.AES256:
			CipherAlgorithm = CipherIdentifier.AES;
			break;
		case StandardCipher.RC4:
			CipherAlgorithm = CipherIdentifier.RC4;
			break;
		}
	}

	public override SymmetricAlgorithm CreateCipher()
	{
		return CryptoHelpers.CreateCipher(CipherAlgorithm, KeySize, BlockSize, CipherMode.ECB);
	}

	public override Stream CreateEncryptedPackageStream(Stream stream, byte[] secretKey)
	{
		return new StandardEncryptedPackageStream(stream, secretKey, this);
	}

	public override byte[] GenerateBlockKey(int blockNumber, byte[] secretKey)
	{
		if ((Flags & EncryptionHeaderFlags.AES) != 0)
		{
			throw new Exception("Block key for ECMA-376 Standard Encryption not implemented");
		}
		if ((Flags & EncryptionHeaderFlags.CryptoAPI) != 0)
		{
			byte[] bytes = CryptoHelpers.Combine(secretKey, BitConverter.GetBytes(blockNumber));
			bytes = CryptoHelpers.HashBytes(bytes, HashAlgorithm);
			Array.Resize(ref bytes, KeySize / 8);
			return bytes;
		}
		throw new InvalidOperationException("Unknown encryption type");
	}

	public override byte[] GenerateSecretKey(string password)
	{
		if ((Flags & EncryptionHeaderFlags.AES) != 0)
		{
			return GenerateEcma376SecretKey(password, SaltValue, HashAlgorithm, KeySize, VerifierHashBytesNeeded);
		}
		if ((Flags & EncryptionHeaderFlags.CryptoAPI) != 0)
		{
			return GenerateCryptoApiSecretKey(password, SaltValue, HashAlgorithm, KeySize);
		}
		throw new InvalidOperationException("Unknown encryption type");
	}

	public override bool VerifyPassword(string password)
	{
		byte[] array = GenerateSecretKey(password);
		byte[] rgbKey = (((Flags & EncryptionHeaderFlags.AES) != 0) ? array : GenerateBlockKey(0, array));
		using SymmetricAlgorithm symmetricAlgorithm = CryptoHelpers.CreateCipher(CipherAlgorithm, KeySize, BlockSize, CipherMode.ECB);
		using ICryptoTransform transform = symmetricAlgorithm.CreateDecryptor(rgbKey, SaltValue);
		byte[] bytes = CryptoHelpers.DecryptBytes(transform, Verifier);
		byte[] array2 = CryptoHelpers.DecryptBytes(transform, VerifierHash);
		byte[] array3 = CryptoHelpers.HashBytes(bytes, HashAlgorithm);
		for (int i = 0; i < 16; i++)
		{
			if (array2[i] != array3[i])
			{
				return false;
			}
		}
		return true;
	}

	private static byte[] GenerateCryptoApiSecretKey(string password, byte[] saltValue, HashIdentifier hashAlgorithm, int keySize)
	{
		return CryptoHelpers.HashBytes(CryptoHelpers.Combine(saltValue, Encoding.Unicode.GetBytes(password)), hashAlgorithm);
	}

	private static byte[] GenerateEcma376SecretKey(string password, byte[] saltValue, HashIdentifier hashAlgorithm, int keySize, int verifierHashSize)
	{
		byte[] array = CryptoHelpers.HashBytes(CryptoHelpers.Combine(saltValue, Encoding.Unicode.GetBytes(password)), hashAlgorithm);
		for (int i = 0; i < 50000; i++)
		{
			array = CryptoHelpers.HashBytes(CryptoHelpers.Combine(BitConverter.GetBytes(i), array), hashAlgorithm);
		}
		array = CryptoHelpers.HashBytes(CryptoHelpers.Combine(array, BitConverter.GetBytes(0)), hashAlgorithm);
		array = DeriveKey(array, hashAlgorithm, keySize, verifierHashSize);
		Array.Resize(ref array, keySize / 8);
		return array;
	}

	private static byte[] DeriveKey(byte[] hashValue, HashIdentifier hashAlgorithm, int keySize, int verifierHashSize)
	{
		byte[] array = new byte[64];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)((i < hashValue.Length) ? (0x36u ^ hashValue[i]) : 54u);
		}
		byte[] array2 = CryptoHelpers.HashBytes(array, hashAlgorithm);
		if (verifierHashSize > keySize / 8)
		{
			return array2;
		}
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = (byte)((j < hashValue.Length) ? (0x5Cu ^ hashValue[j]) : 92u);
		}
		byte[] array3 = CryptoHelpers.HashBytes(array, hashAlgorithm);
		return CryptoHelpers.Combine(array2, array3);
	}
}

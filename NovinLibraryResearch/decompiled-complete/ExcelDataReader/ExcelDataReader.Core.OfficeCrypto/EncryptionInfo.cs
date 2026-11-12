using System;
using System.IO;
using System.Security.Cryptography;

namespace ExcelDataReader.Core.OfficeCrypto;

internal abstract class EncryptionInfo
{
	public abstract bool IsXor { get; }

	public static EncryptionInfo Create(ushort xorEncryptionKey, ushort xorHashValue)
	{
		return new XorEncryption
		{
			EncryptionKey = xorEncryptionKey,
			HashValue = xorHashValue
		};
	}

	public static EncryptionInfo Create(byte[] bytes)
	{
		ushort num = BitConverter.ToUInt16(bytes, 0);
		ushort num2 = BitConverter.ToUInt16(bytes, 2);
		if (num == 1 && num2 == 1)
		{
			return new RC4Encryption(bytes);
		}
		if ((num == 2 || num == 3 || num == 4) && num2 == 2)
		{
			return new StandardEncryption(bytes);
		}
		if ((num == 3 || num == 4) && num2 == 3)
		{
			throw new InvalidOperationException("Extensible Encryption not supported");
		}
		if (num == 4 && num2 == 4)
		{
			return new AgileEncryption(bytes);
		}
		throw new InvalidOperationException("Unsupported EncryptionInfo version " + num + "." + num2);
	}

	public abstract byte[] GenerateSecretKey(string password);

	public abstract byte[] GenerateBlockKey(int blockNumber, byte[] secretKey);

	public abstract Stream CreateEncryptedPackageStream(Stream stream, byte[] secretKey);

	public abstract bool VerifyPassword(string password);

	public abstract SymmetricAlgorithm CreateCipher();
}

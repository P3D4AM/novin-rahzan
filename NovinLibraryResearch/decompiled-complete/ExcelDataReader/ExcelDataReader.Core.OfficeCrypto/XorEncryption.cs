using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ExcelDataReader.Core.OfficeCrypto;

internal class XorEncryption : EncryptionInfo
{
	public ushort EncryptionKey { get; set; }

	public ushort HashValue { get; set; }

	public override bool IsXor => true;

	public override SymmetricAlgorithm CreateCipher()
	{
		return new XorManaged();
	}

	public override Stream CreateEncryptedPackageStream(Stream stream, byte[] secretKey)
	{
		throw new NotImplementedException();
	}

	public override byte[] GenerateBlockKey(int blockNumber, byte[] secretKey)
	{
		return secretKey;
	}

	public override byte[] GenerateSecretKey(string password)
	{
		return XorManaged.CreateXorArray_Method1(Encoding.ASCII.GetBytes(password.Substring(0, Math.Min(password.Length, 15))));
	}

	public override bool VerifyPassword(string password)
	{
		return XorManaged.CreatePasswordVerifier_Method1(Encoding.ASCII.GetBytes(password.Substring(0, Math.Min(password.Length, 15)))) == HashValue;
	}
}

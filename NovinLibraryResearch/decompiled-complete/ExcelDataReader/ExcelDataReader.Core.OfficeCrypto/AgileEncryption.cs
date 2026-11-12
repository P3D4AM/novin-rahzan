using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace ExcelDataReader.Core.OfficeCrypto;

internal class AgileEncryption : EncryptionInfo
{
	private const string NEncryption = "encryption";

	private const string NKeyData = "keyData";

	private const string NKeyEncryptors = "keyEncryptors";

	private const string NKeyEncryptor = "keyEncryptor";

	private const string NEncryptedKey = "encryptedKey";

	private const string NsEncryption = "http://schemas.microsoft.com/office/2006/encryption";

	private const string NsPassword = "http://schemas.microsoft.com/office/2006/keyEncryptor/password";

	public CipherIdentifier CipherAlgorithm { get; set; }

	public CipherMode CipherChaining { get; set; }

	public HashIdentifier HashAlgorithm { get; set; }

	public int KeyBits { get; set; }

	public int BlockSize { get; set; }

	public int HashSize { get; set; }

	public byte[] SaltValue { get; set; }

	public byte[] PasswordSaltValue { get; set; }

	public CipherIdentifier PasswordCipherAlgorithm { get; set; }

	public CipherMode PasswordCipherChaining { get; set; }

	public HashIdentifier PasswordHashAlgorithm { get; set; }

	public byte[] PasswordEncryptedKeyValue { get; set; }

	public byte[] PasswordEncryptedVerifierHashInput { get; set; }

	public byte[] PasswordEncryptedVerifierHashValue { get; set; }

	public int PasswordSpinCount { get; set; }

	public int PasswordKeyBits { get; set; }

	public int PasswordBlockSize { get; set; }

	public override bool IsXor => false;

	public AgileEncryption(byte[] bytes)
	{
		using MemoryStream input = new MemoryStream(bytes, 8, bytes.Length - 8);
		using XmlReader xmlReader = XmlReader.Create(input);
		ReadXmlEncryptionInfoStream(xmlReader);
	}

	public override SymmetricAlgorithm CreateCipher()
	{
		return CryptoHelpers.CreateCipher(CipherAlgorithm, KeyBits, BlockSize * 8, CipherChaining);
	}

	public override byte[] GenerateSecretKey(string password)
	{
		using SymmetricAlgorithm cipher = CryptoHelpers.CreateCipher(PasswordCipherAlgorithm, PasswordKeyBits, PasswordBlockSize * 8, PasswordCipherChaining);
		return GenerateSecretKey(password, PasswordSaltValue, PasswordHashAlgorithm, PasswordEncryptedKeyValue, PasswordSpinCount, PasswordKeyBits, cipher);
	}

	public override byte[] GenerateBlockKey(int blockNumber, byte[] secretKey)
	{
		byte[] array = CryptoHelpers.HashBytes(CryptoHelpers.Combine(secretKey, BitConverter.GetBytes(blockNumber)), HashAlgorithm);
		Array.Resize(ref array, BlockSize);
		return array;
	}

	public override Stream CreateEncryptedPackageStream(Stream stream, byte[] secretKey)
	{
		return new AgileEncryptedPackageStream(stream, secretKey, SaltValue, this);
	}

	public override bool VerifyPassword(string password)
	{
		byte[] array = HashPassword(password, PasswordSaltValue, PasswordHashAlgorithm, PasswordSpinCount);
		byte[] array2 = CryptoHelpers.HashBytes(CryptoHelpers.Combine(array, new byte[8] { 254, 167, 210, 118, 59, 75, 158, 121 }), PasswordHashAlgorithm);
		Array.Resize(ref array2, PasswordKeyBits / 8);
		byte[] array3 = CryptoHelpers.HashBytes(CryptoHelpers.Combine(array, new byte[8] { 215, 170, 15, 109, 48, 97, 52, 78 }), PasswordHashAlgorithm);
		Array.Resize(ref array3, PasswordKeyBits / 8);
		using SymmetricAlgorithm algo = CryptoHelpers.CreateCipher(PasswordCipherAlgorithm, PasswordKeyBits, PasswordBlockSize * 8, PasswordCipherChaining);
		byte[] bytes = CryptoHelpers.DecryptBytes(algo, PasswordEncryptedVerifierHashInput, array2, PasswordSaltValue);
		byte[] array4 = CryptoHelpers.DecryptBytes(algo, PasswordEncryptedVerifierHashValue, array3, PasswordSaltValue);
		byte[] array5 = CryptoHelpers.HashBytes(bytes, PasswordHashAlgorithm);
		for (int i = 0; i < Math.Min(array4.Length, array5.Length); i++)
		{
			if (array4[i] != array5[i])
			{
				return false;
			}
		}
		return true;
	}

	private static byte[] GenerateSecretKey(string password, byte[] saltValue, HashIdentifier hashAlgorithm, byte[] encryptedKeyValue, int spinCount, int keyBits, SymmetricAlgorithm cipher)
	{
		byte[] array = new byte[8] { 20, 110, 11, 231, 171, 172, 208, 214 };
		byte[] array2 = HashPassword(password, saltValue, hashAlgorithm, spinCount);
		array2 = CryptoHelpers.HashBytes(CryptoHelpers.Combine(array2, array), hashAlgorithm);
		int num = array2.Length;
		Array.Resize(ref array2, keyBits / 8);
		for (int i = num; i < keyBits / 8; i++)
		{
			array2[i] = 54;
		}
		byte[] array3 = CryptoHelpers.DecryptBytes(cipher, encryptedKeyValue, array2, saltValue);
		Array.Resize(ref array3, keyBits / 8);
		return array3;
	}

	private static byte[] HashPassword(string password, byte[] saltValue, HashIdentifier hashAlgorithm, int spinCount)
	{
		byte[] array = CryptoHelpers.HashBytes(CryptoHelpers.Combine(saltValue, Encoding.Unicode.GetBytes(password)), hashAlgorithm);
		for (int i = 0; i < spinCount; i++)
		{
			array = CryptoHelpers.HashBytes(CryptoHelpers.Combine(BitConverter.GetBytes(i), array), hashAlgorithm);
		}
		return array;
	}

	private HashIdentifier ParseHash(string value)
	{
		return (HashIdentifier)Enum.Parse(typeof(HashIdentifier), value);
	}

	private CipherIdentifier ParseCipher(string value, int blockBits)
	{
		return value switch
		{
			"AES" => CipherIdentifier.AES, 
			"DES" => CipherIdentifier.DES, 
			"3DES" => CipherIdentifier.DES3, 
			"RC2" => CipherIdentifier.RC2, 
			_ => throw new ArgumentException("value", "Unknown encryption: " + value), 
		};
	}

	private CipherMode ParseCipherMode(string value)
	{
		if (value == "ChainingModeCBC")
		{
			return CipherMode.CBC;
		}
		if (value == "ChainingModeCFB")
		{
			return CipherMode.CFB;
		}
		throw new ArgumentException("Invalid CipherMode " + value);
	}

	private void ReadXmlEncryptionInfoStream(XmlReader xmlReader)
	{
		if (!xmlReader.IsStartElement("encryption", "http://schemas.microsoft.com/office/2006/encryption") || !XmlReaderHelper.ReadFirstContent(xmlReader))
		{
			return;
		}
		while (!xmlReader.EOF)
		{
			if (xmlReader.IsStartElement("keyData", "http://schemas.microsoft.com/office/2006/encryption"))
			{
				string attribute = xmlReader.GetAttribute("cipherAlgorithm");
				string attribute2 = xmlReader.GetAttribute("cipherChaining");
				string attribute3 = xmlReader.GetAttribute("hashAlgorithm");
				string attribute4 = xmlReader.GetAttribute("saltValue");
				int.TryParse(xmlReader.GetAttribute("saltSize"), out var _);
				int.TryParse(xmlReader.GetAttribute("blockSize"), out var result2);
				int.TryParse(xmlReader.GetAttribute("keyBits"), out var result3);
				int.TryParse(xmlReader.GetAttribute("hashSize"), out var result4);
				SaltValue = Convert.FromBase64String(attribute4);
				HashSize = result4;
				KeyBits = result3;
				BlockSize = result2;
				CipherAlgorithm = ParseCipher(attribute, result2 * 8);
				CipherChaining = ParseCipherMode(attribute2);
				HashAlgorithm = ParseHash(attribute3);
				xmlReader.Skip();
			}
			else if (xmlReader.IsStartElement("keyEncryptors", "http://schemas.microsoft.com/office/2006/encryption"))
			{
				ReadKeyEncryptors(xmlReader);
			}
			else if (!XmlReaderHelper.SkipContent(xmlReader))
			{
				break;
			}
		}
	}

	private void ReadKeyEncryptors(XmlReader xmlReader)
	{
		if (!XmlReaderHelper.ReadFirstContent(xmlReader))
		{
			return;
		}
		while (!xmlReader.EOF)
		{
			if (xmlReader.IsStartElement("keyEncryptor", "http://schemas.microsoft.com/office/2006/encryption"))
			{
				ReadKeyEncryptor(xmlReader);
			}
			else if (!XmlReaderHelper.SkipContent(xmlReader))
			{
				break;
			}
		}
	}

	private void ReadKeyEncryptor(XmlReader xmlReader)
	{
		if (!XmlReaderHelper.ReadFirstContent(xmlReader))
		{
			return;
		}
		while (!xmlReader.EOF)
		{
			if (xmlReader.IsStartElement("encryptedKey", "http://schemas.microsoft.com/office/2006/keyEncryptor/password"))
			{
				string attribute = xmlReader.GetAttribute("cipherAlgorithm");
				string attribute2 = xmlReader.GetAttribute("cipherChaining");
				string attribute3 = xmlReader.GetAttribute("hashAlgorithm");
				string attribute4 = xmlReader.GetAttribute("saltValue");
				string attribute5 = xmlReader.GetAttribute("encryptedVerifierHashInput");
				string attribute6 = xmlReader.GetAttribute("encryptedVerifierHashValue");
				string attribute7 = xmlReader.GetAttribute("encryptedKeyValue");
				int.TryParse(xmlReader.GetAttribute("spinCount"), out var result);
				int.TryParse(xmlReader.GetAttribute("saltSize"), out var _);
				int.TryParse(xmlReader.GetAttribute("blockSize"), out var result3);
				int.TryParse(xmlReader.GetAttribute("keyBits"), out var result4);
				int.TryParse(xmlReader.GetAttribute("hashSize"), out var _);
				PasswordSaltValue = Convert.FromBase64String(attribute4);
				PasswordCipherAlgorithm = ParseCipher(attribute, result3 * 8);
				PasswordCipherChaining = ParseCipherMode(attribute2);
				PasswordHashAlgorithm = ParseHash(attribute3);
				PasswordEncryptedKeyValue = Convert.FromBase64String(attribute7);
				PasswordEncryptedVerifierHashInput = Convert.FromBase64String(attribute5);
				PasswordEncryptedVerifierHashValue = Convert.FromBase64String(attribute6);
				PasswordSpinCount = result;
				PasswordKeyBits = result4;
				PasswordBlockSize = result3;
				xmlReader.Skip();
			}
			else if (!XmlReaderHelper.SkipContent(xmlReader))
			{
				break;
			}
		}
	}
}

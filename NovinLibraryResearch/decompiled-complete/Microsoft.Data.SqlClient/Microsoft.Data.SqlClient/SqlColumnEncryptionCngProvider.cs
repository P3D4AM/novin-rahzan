using System;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Data.SqlClient;

public class SqlColumnEncryptionCngProvider : SqlColumnEncryptionKeyStoreProvider
{
	public const string ProviderName = "MSSQL_CNG_STORE";

	private const string RSAEncryptionAlgorithmWithOAEP = "RSA_OAEP";

	private readonly byte[] _version = new byte[1] { 1 };

	public override byte[] DecryptColumnEncryptionKey(string masterKeyPath, string encryptionAlgorithm, byte[] encryptedColumnEncryptionKey)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		ValidateNonEmptyKeyPath(masterKeyPath, isSystemOp: true);
		if (encryptedColumnEncryptionKey == null)
		{
			throw SQL.NullEncryptedColumnEncryptionKey();
		}
		if (encryptedColumnEncryptionKey.Length == 0)
		{
			throw SQL.EmptyEncryptedColumnEncryptionKey();
		}
		ValidateEncryptionAlgorithm(encryptionAlgorithm, isSystemOp: true);
		RSACng rsaCngProvider = CreateRSACngProvider(masterKeyPath, isSystemOp: true);
		int keySize = GetKeySize(rsaCngProvider);
		if (encryptedColumnEncryptionKey[0] != _version[0])
		{
			throw SQL.InvalidAlgorithmVersionInEncryptedCEK(encryptedColumnEncryptionKey[0], _version[0]);
		}
		int num = _version.Length;
		ushort num2 = BitConverter.ToUInt16(encryptedColumnEncryptionKey, num);
		num += 2;
		ushort num3 = BitConverter.ToUInt16(encryptedColumnEncryptionKey, num);
		num += 2;
		num += num2;
		if (num3 != keySize)
		{
			throw SQL.InvalidCiphertextLengthInEncryptedCEKCng(num3, keySize, masterKeyPath);
		}
		int num4 = encryptedColumnEncryptionKey.Length - num - num3;
		if (num4 != keySize)
		{
			throw SQL.InvalidSignatureInEncryptedCEKCng(num4, keySize, masterKeyPath);
		}
		byte[] array = new byte[num3];
		Buffer.BlockCopy(encryptedColumnEncryptionKey, num, array, 0, array.Length);
		num += num3;
		byte[] array2 = new byte[num4];
		Buffer.BlockCopy(encryptedColumnEncryptionKey, num, array2, 0, array2.Length);
		SHA256Cng val = new SHA256Cng();
		byte[] hash;
		try
		{
			((HashAlgorithm)(object)val).TransformFinalBlock(encryptedColumnEncryptionKey, 0, encryptedColumnEncryptionKey.Length - array2.Length);
			hash = ((HashAlgorithm)(object)val).Hash;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (!RSAVerifySignature(hash, array2, rsaCngProvider))
		{
			throw SQL.InvalidSignature(masterKeyPath);
		}
		return RSADecrypt(rsaCngProvider, array);
	}

	public override byte[] EncryptColumnEncryptionKey(string masterKeyPath, string encryptionAlgorithm, byte[] columnEncryptionKey)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		ValidateNonEmptyKeyPath(masterKeyPath, isSystemOp: false);
		if (columnEncryptionKey == null)
		{
			throw SQL.NullColumnEncryptionKey();
		}
		if (columnEncryptionKey.Length == 0)
		{
			throw SQL.EmptyColumnEncryptionKey();
		}
		ValidateEncryptionAlgorithm(encryptionAlgorithm, isSystemOp: false);
		RSACng rsaCngProvider = CreateRSACngProvider(masterKeyPath, isSystemOp: false);
		int keySize = GetKeySize(rsaCngProvider);
		byte[] array = new byte[1] { _version[0] };
		byte[] bytes = Encoding.Unicode.GetBytes(masterKeyPath.ToLowerInvariant());
		byte[] bytes2 = BitConverter.GetBytes((short)bytes.Length);
		byte[] array2 = RSAEncrypt(rsaCngProvider, columnEncryptionKey);
		byte[] bytes3 = BitConverter.GetBytes((short)array2.Length);
		SHA256Cng val = new SHA256Cng();
		byte[] hash;
		try
		{
			((HashAlgorithm)(object)val).TransformBlock(array, 0, array.Length, array, 0);
			((HashAlgorithm)(object)val).TransformBlock(bytes2, 0, bytes2.Length, bytes2, 0);
			((HashAlgorithm)(object)val).TransformBlock(bytes3, 0, bytes3.Length, bytes3, 0);
			((HashAlgorithm)(object)val).TransformBlock(bytes, 0, bytes.Length, bytes, 0);
			((HashAlgorithm)(object)val).TransformFinalBlock(array2, 0, array2.Length);
			hash = ((HashAlgorithm)(object)val).Hash;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		byte[] array3 = RSASignHashedData(hash, rsaCngProvider);
		int num = array.Length + bytes3.Length + bytes2.Length + array2.Length + bytes.Length + array3.Length;
		byte[] array4 = new byte[num];
		int num2 = 0;
		Buffer.BlockCopy(array, 0, array4, num2, array.Length);
		num2 += array.Length;
		Buffer.BlockCopy(bytes2, 0, array4, num2, bytes2.Length);
		num2 += bytes2.Length;
		Buffer.BlockCopy(bytes3, 0, array4, num2, bytes3.Length);
		num2 += bytes3.Length;
		Buffer.BlockCopy(bytes, 0, array4, num2, bytes.Length);
		num2 += bytes.Length;
		Buffer.BlockCopy(array2, 0, array4, num2, array2.Length);
		num2 += array2.Length;
		Buffer.BlockCopy(array3, 0, array4, num2, array3.Length);
		return array4;
	}

	public override byte[] SignColumnMasterKeyMetadata(string masterKeyPath, bool allowEnclaveComputations)
	{
		throw new NotSupportedException();
	}

	public override bool VerifyColumnMasterKeyMetadata(string masterKeyPath, bool allowEnclaveComputations, byte[] signature)
	{
		throw new NotSupportedException();
	}

	private void ValidateEncryptionAlgorithm(string encryptionAlgorithm, bool isSystemOp)
	{
		if (encryptionAlgorithm == null)
		{
			throw SQL.NullKeyEncryptionAlgorithm(isSystemOp);
		}
		if (!string.Equals(encryptionAlgorithm, "RSA_OAEP", StringComparison.OrdinalIgnoreCase))
		{
			throw SQL.InvalidKeyEncryptionAlgorithm(encryptionAlgorithm, "RSA_OAEP", isSystemOp);
		}
	}

	private void ValidateNonEmptyKeyPath(string masterKeyPath, bool isSystemOp)
	{
		if (string.IsNullOrWhiteSpace(masterKeyPath))
		{
			if (masterKeyPath == null)
			{
				throw SQL.NullCngKeyPath(isSystemOp);
			}
			throw SQL.InvalidCngPath(masterKeyPath, isSystemOp);
		}
	}

	private byte[] RSAEncrypt(RSACng rsaCngProvider, byte[] columnEncryptionKey)
	{
		return rsaCngProvider.Encrypt(columnEncryptionKey, RSAEncryptionPadding.OaepSHA1);
	}

	private byte[] RSADecrypt(RSACng rsaCngProvider, byte[] encryptedColumnEncryptionKey)
	{
		return rsaCngProvider.Decrypt(encryptedColumnEncryptionKey, RSAEncryptionPadding.OaepSHA1);
	}

	private byte[] RSASignHashedData(byte[] dataToSign, RSACng rsaCngProvider)
	{
		return rsaCngProvider.SignData(dataToSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
	}

	private bool RSAVerifySignature(byte[] dataToVerify, byte[] signature, RSACng rsaCngProvider)
	{
		return rsaCngProvider.VerifyData(dataToVerify, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
	}

	private int GetKeySize(RSACng rsaCngProvider)
	{
		return rsaCngProvider.KeySize / 8;
	}

	private RSACng CreateRSACngProvider(string keyPath, bool isSystemOp)
	{
		GetCngProviderAndKeyId(keyPath, isSystemOp, out var cngProvider, out var keyIdentifier);
		CngProvider provider = new CngProvider(cngProvider);
		CngKey key;
		try
		{
			key = CngKey.Open(keyIdentifier, provider);
		}
		catch (CryptographicException)
		{
			throw SQL.InvalidCngKey(keyPath, cngProvider, keyIdentifier, isSystemOp);
		}
		return new RSACng(key);
	}

	private void GetCngProviderAndKeyId(string keyPath, bool isSystemOp, out string cngProvider, out string keyIdentifier)
	{
		int num = keyPath.IndexOf("/");
		if (num == -1)
		{
			throw SQL.InvalidCngPath(keyPath, isSystemOp);
		}
		cngProvider = keyPath.Substring(0, num);
		keyIdentifier = keyPath.Substring(num + 1, keyPath.Length - (num + 1));
		if (cngProvider.Length == 0)
		{
			throw SQL.EmptyCngName(keyPath, isSystemOp);
		}
		if (keyIdentifier.Length == 0)
		{
			throw SQL.EmptyCngKeyId(keyPath, isSystemOp);
		}
	}
}

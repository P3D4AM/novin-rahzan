using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace Microsoft.Data.SqlClient;

public class SqlColumnEncryptionCspProvider : SqlColumnEncryptionKeyStoreProvider
{
	public const string ProviderName = "MSSQL_CSP_PROVIDER";

	private const string RSAEncryptionAlgorithmWithOAEP = "RSA_OAEP";

	private const string HashingAlgorithm = "SHA256";

	private readonly byte[] _version = new byte[1] { 1 };

	public override byte[] DecryptColumnEncryptionKey(string masterKeyPath, string encryptionAlgorithm, byte[] encryptedColumnEncryptionKey)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		ValidateNonEmptyCSPKeyPath(masterKeyPath, isSystemOp: true);
		if (encryptedColumnEncryptionKey == null)
		{
			throw SQL.NullEncryptedColumnEncryptionKey();
		}
		if (encryptedColumnEncryptionKey.Length == 0)
		{
			throw SQL.EmptyEncryptedColumnEncryptionKey();
		}
		ValidateEncryptionAlgorithm(encryptionAlgorithm, isSystemOp: true);
		RSACryptoServiceProvider rscp = CreateRSACryptoProvider(masterKeyPath, isSystemOp: true);
		int keySize = GetKeySize(rscp);
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
			throw SQL.InvalidCiphertextLengthInEncryptedCEKCsp(num3, keySize, masterKeyPath);
		}
		int num4 = encryptedColumnEncryptionKey.Length - num - num3;
		if (num4 != keySize)
		{
			throw SQL.InvalidSignatureInEncryptedCEKCsp(num4, keySize, masterKeyPath);
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
		if (!RSAVerifySignature(hash, array2, rscp))
		{
			throw SQL.InvalidSignature(masterKeyPath);
		}
		return RSADecrypt(rscp, array);
	}

	public override byte[] EncryptColumnEncryptionKey(string masterKeyPath, string encryptionAlgorithm, byte[] columnEncryptionKey)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		ValidateNonEmptyCSPKeyPath(masterKeyPath, isSystemOp: false);
		if (columnEncryptionKey == null)
		{
			throw SQL.NullColumnEncryptionKey();
		}
		if (columnEncryptionKey.Length == 0)
		{
			throw SQL.EmptyColumnEncryptionKey();
		}
		ValidateEncryptionAlgorithm(encryptionAlgorithm, isSystemOp: false);
		RSACryptoServiceProvider rscp = CreateRSACryptoProvider(masterKeyPath, isSystemOp: false);
		int keySize = GetKeySize(rscp);
		byte[] array = new byte[1] { _version[0] };
		byte[] bytes = Encoding.Unicode.GetBytes(masterKeyPath.ToLowerInvariant());
		byte[] bytes2 = BitConverter.GetBytes((short)bytes.Length);
		byte[] array2 = RSAEncrypt(rscp, columnEncryptionKey);
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
		byte[] array3 = RSASignHashedData(hash, rscp);
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

	private void ValidateNonEmptyCSPKeyPath(string masterKeyPath, bool isSystemOp)
	{
		if (string.IsNullOrWhiteSpace(masterKeyPath))
		{
			if (masterKeyPath == null)
			{
				throw SQL.NullCspKeyPath(isSystemOp);
			}
			throw SQL.InvalidCspPath(masterKeyPath, isSystemOp);
		}
	}

	private byte[] RSAEncrypt(RSACryptoServiceProvider rscp, byte[] columnEncryptionKey)
	{
		return rscp.Encrypt(columnEncryptionKey, fOAEP: true);
	}

	private byte[] RSADecrypt(RSACryptoServiceProvider rscp, byte[] encryptedColumnEncryptionKey)
	{
		return rscp.Decrypt(encryptedColumnEncryptionKey, fOAEP: true);
	}

	private byte[] RSASignHashedData(byte[] dataToSign, RSACryptoServiceProvider rscp)
	{
		return rscp.SignData(dataToSign, "SHA256");
	}

	private bool RSAVerifySignature(byte[] dataToVerify, byte[] signature, RSACryptoServiceProvider rscp)
	{
		return rscp.VerifyData(dataToVerify, "SHA256", signature);
	}

	private int GetKeySize(RSACryptoServiceProvider rscp)
	{
		return rscp.KeySize / 8;
	}

	private RSACryptoServiceProvider CreateRSACryptoProvider(string keyPath, bool isSystemOp)
	{
		GetCspProviderAndKeyName(keyPath, isSystemOp, out var cspProviderName, out var keyIdentifier);
		int providerType = GetProviderType(cspProviderName, keyPath, isSystemOp);
		CspParameters cspParameters = new CspParameters(providerType, cspProviderName, keyIdentifier);
		cspParameters.Flags = CspProviderFlags.UseExistingKey;
		RSACryptoServiceProvider rSACryptoServiceProvider = null;
		try
		{
			return new RSACryptoServiceProvider(cspParameters);
		}
		catch (CryptographicException ex)
		{
			if (ex.HResult == -2146893802)
			{
				throw SQL.InvalidCspKeyIdentifier(keyIdentifier, keyPath, isSystemOp);
			}
			throw;
		}
	}

	private void GetCspProviderAndKeyName(string keyPath, bool isSystemOp, out string cspProviderName, out string keyIdentifier)
	{
		int num = keyPath.IndexOf("/");
		if (num == -1)
		{
			throw SQL.InvalidCspPath(keyPath, isSystemOp);
		}
		cspProviderName = keyPath.Substring(0, num);
		keyIdentifier = keyPath.Substring(num + 1, keyPath.Length - (num + 1));
		if (cspProviderName.Length == 0)
		{
			throw SQL.EmptyCspName(keyPath, isSystemOp);
		}
		if (keyIdentifier.Length == 0)
		{
			throw SQL.EmptyCspKeyId(keyPath, isSystemOp);
		}
	}

	private int GetProviderType(string providerName, string keyPath, bool isSystemOp)
	{
		string text = $"SOFTWARE\\Microsoft\\Cryptography\\Defaults\\Provider\\{providerName}";
		RegistryKey val = Registry.LocalMachine.OpenSubKey(text);
		if (val == null)
		{
			throw SQL.InvalidCspName(providerName, keyPath, isSystemOp);
		}
		int result = (int)val.GetValue("Type");
		val.Close();
		return result;
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Data.SqlClient;

internal static class SqlSecurityUtility
{
	private static readonly ColumnMasterKeyMetadataSignatureVerificationCache ColumnMasterKeyMetadataSignatureVerificationCache = ColumnMasterKeyMetadataSignatureVerificationCache.Instance;

	internal static void GetHMACWithSHA256(byte[] plainText, byte[] key, byte[] hash)
	{
		using HMACSHA256 hMACSHA = new HMACSHA256(key);
		byte[] src = hMACSHA.ComputeHash(plainText);
		Buffer.BlockCopy(src, 0, hash, 0, hash.Length);
	}

	internal static string GetSHA256Hash(byte[] input)
	{
		using SHA256 sHA = SHA256.Create();
		byte[] input2 = sHA.ComputeHash(input);
		return GetHexString(input2);
	}

	internal static void GenerateRandomBytes(byte[] randomBytes)
	{
		RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		rNGCryptoServiceProvider.GetBytes(randomBytes);
	}

	internal static bool CompareBytes(byte[] buffer1, byte[] buffer2, int buffer2Index, int lengthToCompare)
	{
		if (buffer1 == null || buffer2 == null)
		{
			return false;
		}
		if (buffer2.Length - buffer2Index < lengthToCompare)
		{
			return false;
		}
		for (int i = 0; i < buffer1.Length && i < lengthToCompare; i++)
		{
			if (buffer1[i] != buffer2[buffer2Index + i])
			{
				return false;
			}
		}
		return true;
	}

	internal static string GetHexString(byte[] input)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte b in input)
		{
			stringBuilder.AppendFormat(b.ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	internal static string GetCurrentFunctionName()
	{
		StackTrace stackTrace = new StackTrace();
		StackFrame frame = stackTrace.GetFrame(1);
		MethodBase method = frame.GetMethod();
		return $"{method.DeclaringType.Name}.{method.Name}";
	}

	private static string ValidateAndGetEncryptionAlgorithmName(byte cipherAlgorithmId, string cipherAlgorithmName)
	{
		if (cipherAlgorithmId == 0)
		{
			if (cipherAlgorithmName == null)
			{
				throw SQL.NullColumnEncryptionAlgorithm(SqlClientEncryptionAlgorithmFactoryList.GetInstance().GetRegisteredCipherAlgorithmNames());
			}
			return cipherAlgorithmName;
		}
		if (2 == cipherAlgorithmId)
		{
			return "AEAD_AES_256_CBC_HMAC_SHA256";
		}
		throw SQL.UnknownColumnEncryptionAlgorithmId(cipherAlgorithmId, GetRegisteredCipherAlgorithmIds());
	}

	private static string GetRegisteredCipherAlgorithmIds()
	{
		return "'1', '2'";
	}

	internal static byte[] EncryptWithKey(byte[] plainText, SqlCipherMetadata md, SqlConnection connection, SqlCommand command)
	{
		if (!md.IsAlgorithmInitialized())
		{
			DecryptSymmetricKey(md, connection, command);
		}
		byte[] array = md.CipherAlgorithm.EncryptData(plainText);
		if (array == null || array.Length == 0)
		{
			throw SQL.NullCipherText();
		}
		return array;
	}

	internal static string GetBytesAsString(byte[] buff, bool fLast, int countOfBytes)
	{
		int num = ((buff.Length > countOfBytes) ? countOfBytes : buff.Length);
		int num2 = 0;
		if (fLast)
		{
			num2 = buff.Length - num;
		}
		return BitConverter.ToString(buff, num2, num);
	}

	internal static byte[] DecryptWithKey(byte[] cipherText, SqlCipherMetadata md, SqlConnection connection, SqlCommand command)
	{
		if (!md.IsAlgorithmInitialized())
		{
			DecryptSymmetricKey(md, connection, command);
		}
		try
		{
			byte[] array = md.CipherAlgorithm.DecryptData(cipherText);
			if (array == null)
			{
				throw SQL.NullPlainText();
			}
			return array;
		}
		catch (Exception e)
		{
			string bytesAsString = GetBytesAsString(md.EncryptionKeyInfo.encryptedKey, fLast: true, 10);
			string bytesAsString2 = GetBytesAsString(cipherText, fLast: false, 10);
			throw SQL.ThrowDecryptionFailed(bytesAsString, bytesAsString2, e);
		}
	}

	internal static void DecryptSymmetricKey(SqlCipherMetadata md, SqlConnection connection, SqlCommand command)
	{
		SqlClientSymmetricKey sqlClientSymmetricKey = null;
		SqlEncryptionKeyInfo encryptionkeyInfoChosen = null;
		DecryptSymmetricKey(md.EncryptionInfo, out sqlClientSymmetricKey, out encryptionkeyInfoChosen, connection, command);
		md.CipherAlgorithm = null;
		SqlClientEncryptionAlgorithm encryptionAlgorithm = null;
		string algorithmName = ValidateAndGetEncryptionAlgorithmName(md.CipherAlgorithmId, md.CipherAlgorithmName);
		SqlClientEncryptionAlgorithmFactoryList.GetInstance().GetAlgorithm(sqlClientSymmetricKey, md.EncryptionType, algorithmName, out encryptionAlgorithm);
		md.CipherAlgorithm = encryptionAlgorithm;
		md.EncryptionKeyInfo = encryptionkeyInfoChosen;
	}

	internal static void DecryptSymmetricKey(SqlTceCipherInfoEntry sqlTceCipherInfoEntry, out SqlClientSymmetricKey sqlClientSymmetricKey, out SqlEncryptionKeyInfo encryptionkeyInfoChosen, SqlConnection connection, SqlCommand command)
	{
		sqlClientSymmetricKey = null;
		encryptionkeyInfoChosen = null;
		Exception ex = null;
		SqlSymmetricKeyCache instance = SqlSymmetricKeyCache.GetInstance();
		foreach (SqlEncryptionKeyInfo columnEncryptionKeyValue in sqlTceCipherInfoEntry.ColumnEncryptionKeyValues)
		{
			try
			{
				sqlClientSymmetricKey = (ShouldUseInstanceLevelProviderFlow(columnEncryptionKeyValue.keyStoreName, connection, command) ? GetKeyFromLocalProviders(columnEncryptionKeyValue, connection, command) : instance.GetKey(columnEncryptionKeyValue, connection, command));
				encryptionkeyInfoChosen = columnEncryptionKeyValue;
			}
			catch (Exception ex2)
			{
				ex = ex2;
				continue;
			}
			break;
		}
		if (sqlClientSymmetricKey == null)
		{
			throw ex;
		}
	}

	private static SqlClientSymmetricKey GetKeyFromLocalProviders(SqlEncryptionKeyInfo keyInfo, SqlConnection connection, SqlCommand command)
	{
		string dataSource = connection.DataSource;
		ThrowIfKeyPathIsNotTrustedForServer(dataSource, keyInfo.keyPath);
		if (!TryGetColumnEncryptionKeyStoreProvider(keyInfo.keyStoreName, out var provider, connection, command))
		{
			throw SQL.UnrecognizedKeyStoreProviderName(keyInfo.keyStoreName, SqlConnection.GetColumnEncryptionSystemKeyStoreProvidersNames(), GetListOfProviderNamesThatWereSearched(connection, command));
		}
		byte[] rootKey;
		try
		{
			rootKey = provider.DecryptColumnEncryptionKey(keyInfo.keyPath, keyInfo.algorithmName, keyInfo.encryptedKey);
		}
		catch (Exception e)
		{
			string bytesAsString = GetBytesAsString(keyInfo.encryptedKey, fLast: true, 10);
			throw SQL.KeyDecryptionFailed(keyInfo.keyStoreName, bytesAsString, e);
		}
		return new SqlClientSymmetricKey(rootKey);
	}

	internal static int GetBase64LengthFromByteLength(int byteLength)
	{
		return (int)((double)byteLength * 4.0 / 3.0) + 4;
	}

	internal static void VerifyColumnMasterKeySignature(string keyStoreName, string keyPath, bool isEnclaveEnabled, byte[] CMKSignature, SqlConnection connection, SqlCommand command)
	{
		bool flag = false;
		try
		{
			if (CMKSignature == null || CMKSignature.Length == 0)
			{
				throw SQL.ColumnMasterKeySignatureNotFound(keyPath);
			}
			ThrowIfKeyPathIsNotTrustedForServer(connection.DataSource, keyPath);
			if (!TryGetColumnEncryptionKeyStoreProvider(keyStoreName, out var provider, connection, command))
			{
				throw SQL.InvalidKeyStoreProviderName(keyStoreName, SqlConnection.GetColumnEncryptionSystemKeyStoreProvidersNames(), GetListOfProviderNamesThatWereSearched(connection, command));
			}
			if (ShouldUseInstanceLevelProviderFlow(keyStoreName, connection, command))
			{
				flag = provider.VerifyColumnMasterKeyMetadata(keyPath, isEnclaveEnabled, CMKSignature);
			}
			else
			{
				bool? signatureVerificationResult = ColumnMasterKeyMetadataSignatureVerificationCache.GetSignatureVerificationResult(keyStoreName, keyPath, isEnclaveEnabled, CMKSignature);
				if (!signatureVerificationResult.HasValue)
				{
					flag = provider.VerifyColumnMasterKeyMetadata(keyPath, isEnclaveEnabled, CMKSignature);
					ColumnMasterKeyMetadataSignatureVerificationCache.AddSignatureVerificationResult(keyStoreName, keyPath, isEnclaveEnabled, CMKSignature, flag);
				}
				else
				{
					flag = signatureVerificationResult.Value;
				}
			}
		}
		catch (Exception innerException)
		{
			throw SQL.UnableToVerifyColumnMasterKeySignature(innerException);
		}
		if (!flag)
		{
			throw SQL.ColumnMasterKeySignatureVerificationFailed(keyPath);
		}
	}

	private static bool ShouldUseInstanceLevelProviderFlow(string keyStoreName, SqlConnection connection, SqlCommand command)
	{
		if (InstanceLevelProvidersAreRegistered(connection, command))
		{
			return !keyStoreName.StartsWith("MSSQL_");
		}
		return false;
	}

	private static bool InstanceLevelProvidersAreRegistered(SqlConnection connection, SqlCommand command)
	{
		if (!connection.HasColumnEncryptionKeyStoreProvidersRegistered)
		{
			return command?.HasColumnEncryptionKeyStoreProvidersRegistered ?? false;
		}
		return true;
	}

	internal static void ThrowIfKeyPathIsNotTrustedForServer(string serverName, string keyPath)
	{
		if (SqlConnection.ColumnEncryptionTrustedMasterKeyPaths.TryGetValue(serverName, out var value) && (value == null || value.Count() == 0 || !value.Any((string trustedKeyPath) => trustedKeyPath.Equals(keyPath, StringComparison.InvariantCultureIgnoreCase))))
		{
			throw SQL.UntrustedKeyPath(keyPath, serverName);
		}
	}

	internal static bool TryGetColumnEncryptionKeyStoreProvider(string keyStoreName, out SqlColumnEncryptionKeyStoreProvider provider, SqlConnection connection, SqlCommand command)
	{
		if (SqlConnection.TryGetSystemColumnEncryptionKeyStoreProvider(keyStoreName, out provider))
		{
			return true;
		}
		if (command != null && command.HasColumnEncryptionKeyStoreProvidersRegistered)
		{
			return command.TryGetColumnEncryptionKeyStoreProvider(keyStoreName, out provider);
		}
		return connection.TryGetColumnEncryptionKeyStoreProvider(keyStoreName, out provider);
	}

	internal static List<string> GetListOfProviderNamesThatWereSearched(SqlConnection connection, SqlCommand command)
	{
		if (command != null && command.HasColumnEncryptionKeyStoreProvidersRegistered)
		{
			return command.GetColumnEncryptionCustomKeyStoreProvidersNames();
		}
		return connection.GetColumnEncryptionCustomKeyStoreProvidersNames();
	}
}

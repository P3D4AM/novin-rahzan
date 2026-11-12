using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Data.SqlClient;

internal sealed class EnclaveDelegate
{
	internal class RetryableEnclaveQueryExecutionException : Exception
	{
		internal RetryableEnclaveQueryExecutionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	private static readonly SqlAeadAes256CbcHmac256Factory s_sqlAeadAes256CbcHmac256Factory = new SqlAeadAes256CbcHmac256Factory();

	private static readonly EnclaveDelegate s_enclaveDelegate = new EnclaveDelegate();

	private readonly object _lock;

	private static readonly Dictionary<SqlConnectionAttestationProtocol, SqlColumnEncryptionEnclaveProvider> s_enclaveProviders = new Dictionary<SqlConnectionAttestationProtocol, SqlColumnEncryptionEnclaveProvider>();

	public static EnclaveDelegate Instance => s_enclaveDelegate;

	private EnclaveDelegate()
	{
		_lock = new object();
	}

	private byte[] GetUintBytes(string enclaveType, int intValue, string variableName)
	{
		try
		{
			uint value = Convert.ToUInt32(intValue);
			return BitConverter.GetBytes(value);
		}
		catch (Exception innerException)
		{
			throw SQL.InvalidAttestationParameterUnableToConvertToUnsignedInt(variableName, intValue, enclaveType, innerException);
		}
	}

	private List<ColumnEncryptionKeyInfo> GetDecryptedKeysToBeSentToEnclave(ConcurrentDictionary<int, SqlTceCipherInfoEntry> keysTobeSentToEnclave, string serverName, SqlConnection connection, SqlCommand command)
	{
		List<ColumnEncryptionKeyInfo> list = new List<ColumnEncryptionKeyInfo>();
		foreach (SqlTceCipherInfoEntry value in keysTobeSentToEnclave.Values)
		{
			SqlSecurityUtility.DecryptSymmetricKey(value, out var sqlClientSymmetricKey, out var _, connection, command);
			if (sqlClientSymmetricKey == null)
			{
				throw SQL.NullArgumentInternal("sqlClientSymmetricKey", "EnclaveDelegate", "GetDecryptedKeysToBeSentToEnclave");
			}
			if (value.ColumnEncryptionKeyValues == null)
			{
				throw SQL.NullArgumentInternal("ColumnEncryptionKeyValues", "EnclaveDelegate", "GetDecryptedKeysToBeSentToEnclave");
			}
			if (value.ColumnEncryptionKeyValues.Count <= 0)
			{
				throw SQL.ColumnEncryptionKeysNotFound();
			}
			list.Add(new ColumnEncryptionKeyInfo(sqlClientSymmetricKey.RootKey, value.ColumnEncryptionKeyValues[0].databaseId, value.ColumnEncryptionKeyValues[0].cekMdVersion, value.ColumnEncryptionKeyValues[0].cekId));
		}
		return list;
	}

	private byte[] GenerateBytePackageForKeys(long enclaveSessionCounter, byte[] queryStringHashBytes, List<ColumnEncryptionKeyInfo> keys)
	{
		byte[] array = Guid.NewGuid().ToByteArray();
		byte[] bytes = BitConverter.GetBytes(enclaveSessionCounter);
		int num = array.Length;
		num += bytes.Length;
		num += queryStringHashBytes.Length;
		foreach (ColumnEncryptionKeyInfo key in keys)
		{
			num += key.GetLengthForSerialization();
		}
		byte[] array2 = new byte[num];
		int num2 = 0;
		Buffer.BlockCopy(array, 0, array2, num2, array.Length);
		num2 += array.Length;
		Buffer.BlockCopy(bytes, 0, array2, num2, bytes.Length);
		num2 += bytes.Length;
		Buffer.BlockCopy(queryStringHashBytes, 0, array2, num2, queryStringHashBytes.Length);
		num2 += queryStringHashBytes.Length;
		foreach (ColumnEncryptionKeyInfo key2 in keys)
		{
			num2 = key2.SerializeToBuffer(array2, num2);
		}
		return array2;
	}

	private byte[] EncryptBytePackage(byte[] bytePackage, byte[] sessionKey, string serverName)
	{
		if (sessionKey == null)
		{
			throw SQL.NullArgumentInternal("sessionKey", "EnclaveDelegate", "EncryptBytePackage");
		}
		if (sessionKey.Length == 0)
		{
			throw SQL.EmptyArgumentInternal("sessionKey", "EnclaveDelegate", "EncryptBytePackage");
		}
		try
		{
			SqlClientSymmetricKey encryptionKey = new SqlClientSymmetricKey(sessionKey);
			SqlClientEncryptionAlgorithm sqlClientEncryptionAlgorithm = s_sqlAeadAes256CbcHmac256Factory.Create(encryptionKey, SqlClientEncryptionType.Randomized, "AEAD_AES_256_CBC_HMAC_SHA256");
			return sqlClientEncryptionAlgorithm.EncryptData(bytePackage);
		}
		catch (Exception innerException)
		{
			throw SQL.FailedToEncryptRegisterRulesBytePackage(innerException);
		}
	}

	private byte[] CombineByteArrays(byte[] arr1, byte[] arr2)
	{
		int num = arr1.Length + arr2.Length;
		byte[] array = new byte[num];
		Buffer.BlockCopy(arr1, 0, array, 0, arr1.Length);
		Buffer.BlockCopy(arr2, 0, array, arr1.Length, arr2.Length);
		return array;
	}

	private byte[] CombineByteArrays(byte[] arr1, byte[] arr2, byte[] arr3, byte[] arr4, byte[] arr5)
	{
		int num = arr1.Length + arr2.Length + arr3.Length + arr4.Length + arr5.Length;
		byte[] array = new byte[num];
		Buffer.BlockCopy(arr1, 0, array, 0, arr1.Length);
		int num2 = arr1.Length;
		Buffer.BlockCopy(arr2, 0, array, num2, arr2.Length);
		num2 += arr2.Length;
		Buffer.BlockCopy(arr3, 0, array, num2, arr3.Length);
		num2 += arr3.Length;
		Buffer.BlockCopy(arr4, 0, array, num2, arr4.Length);
		num2 += arr4.Length;
		Buffer.BlockCopy(arr5, 0, array, num2, arr5.Length);
		return array;
	}

	private byte[] ComputeQueryStringHash(string queryString)
	{
		if (string.IsNullOrWhiteSpace(queryString))
		{
			if (queryString == null)
			{
				throw SQL.NullArgumentInternal("queryString", "EnclaveDelegate", "ComputeQueryStringHash");
			}
			throw SQL.EmptyArgumentInternal("queryString", "EnclaveDelegate", "ComputeQueryStringHash");
		}
		byte[] bytes = Encoding.Unicode.GetBytes(queryString);
		using SHA256 sHA = SHA256.Create();
		sHA.TransformFinalBlock(bytes, 0, bytes.Length);
		return sHA.Hash;
	}

	internal void CreateEnclaveSession(SqlConnectionAttestationProtocol attestationProtocol, string enclaveType, EnclaveSessionParameters enclaveSessionParameters, byte[] attestationInfo, SqlEnclaveAttestationParameters attestationParameters, byte[] customData, int customDataLength)
	{
		lock (_lock)
		{
			SqlColumnEncryptionEnclaveProvider enclaveProvider = GetEnclaveProvider(attestationProtocol, enclaveType);
			enclaveProvider.GetEnclaveSession(enclaveSessionParameters, generateCustomData: false, out var sqlEnclaveSession, out var counter, out var _, out var _);
			if (sqlEnclaveSession == null)
			{
				enclaveProvider.CreateEnclaveSession(attestationInfo, attestationParameters.ClientDiffieHellmanKey, enclaveSessionParameters, customData, customDataLength, out sqlEnclaveSession, out counter);
				if (sqlEnclaveSession == null)
				{
					throw SQL.NullEnclaveSessionReturnedFromProvider(enclaveType, enclaveSessionParameters.AttestationUrl);
				}
			}
		}
	}

	internal void GetEnclaveSession(SqlConnectionAttestationProtocol attestationProtocol, string enclaveType, EnclaveSessionParameters enclaveSessionParameters, bool generateCustomData, out SqlEnclaveSession sqlEnclaveSession, out byte[] customData, out int customDataLength)
	{
		GetEnclaveSession(attestationProtocol, enclaveType, enclaveSessionParameters, generateCustomData, out sqlEnclaveSession, out var _, out customData, out customDataLength, throwIfNull: false);
	}

	private void GetEnclaveSession(SqlConnectionAttestationProtocol attestationProtocol, string enclaveType, EnclaveSessionParameters enclaveSessionParameters, bool generateCustomData, out SqlEnclaveSession sqlEnclaveSession, out long counter, out byte[] customData, out int customDataLength, bool throwIfNull)
	{
		SqlColumnEncryptionEnclaveProvider enclaveProvider = GetEnclaveProvider(attestationProtocol, enclaveType);
		enclaveProvider.GetEnclaveSession(enclaveSessionParameters, generateCustomData, out sqlEnclaveSession, out counter, out customData, out customDataLength);
		if (throwIfNull && sqlEnclaveSession == null)
		{
			throw SQL.NullEnclaveSessionDuringQueryExecution(enclaveType, enclaveSessionParameters.AttestationUrl);
		}
	}

	internal void InvalidateEnclaveSession(SqlConnectionAttestationProtocol attestationProtocol, string enclaveType, EnclaveSessionParameters enclaveSessionParameters, SqlEnclaveSession enclaveSession)
	{
		SqlColumnEncryptionEnclaveProvider enclaveProvider = GetEnclaveProvider(attestationProtocol, enclaveType);
		enclaveProvider.InvalidateEnclaveSession(enclaveSessionParameters, enclaveSession);
	}

	private SqlColumnEncryptionEnclaveProvider GetEnclaveProvider(SqlConnectionAttestationProtocol attestationProtocol, string enclaveType)
	{
		if (!s_enclaveProviders.TryGetValue(attestationProtocol, out var value))
		{
			switch (attestationProtocol)
			{
			case SqlConnectionAttestationProtocol.AAS:
			{
				AzureAttestationEnclaveProvider value3 = new AzureAttestationEnclaveProvider();
				s_enclaveProviders[attestationProtocol] = value3;
				value = s_enclaveProviders[attestationProtocol];
				break;
			}
			case SqlConnectionAttestationProtocol.HGS:
			{
				HostGuardianServiceEnclaveProvider value2 = new HostGuardianServiceEnclaveProvider();
				s_enclaveProviders[attestationProtocol] = value2;
				value = s_enclaveProviders[attestationProtocol];
				break;
			}
			}
		}
		if (value == null)
		{
			throw SQL.EnclaveProviderNotFound(enclaveType, ConvertAttestationProtocolToString(attestationProtocol));
		}
		return value;
	}

	internal EnclavePackage GenerateEnclavePackage(SqlConnectionAttestationProtocol attestationProtocol, ConcurrentDictionary<int, SqlTceCipherInfoEntry> keysToBeSentToEnclave, string queryText, string enclaveType, EnclaveSessionParameters enclaveSessionParameters, SqlConnection connection, SqlCommand command)
	{
		SqlEnclaveSession sqlEnclaveSession;
		long counter;
		try
		{
			GetEnclaveSession(attestationProtocol, enclaveType, enclaveSessionParameters, generateCustomData: false, out sqlEnclaveSession, out counter, out var _, out var _, throwIfNull: true);
		}
		catch (Exception ex)
		{
			throw new RetryableEnclaveQueryExecutionException(ex.Message, ex);
		}
		List<ColumnEncryptionKeyInfo> decryptedKeysToBeSentToEnclave = GetDecryptedKeysToBeSentToEnclave(keysToBeSentToEnclave, enclaveSessionParameters.ServerName, connection, command);
		byte[] queryStringHashBytes = ComputeQueryStringHash(queryText);
		byte[] bytePackage = GenerateBytePackageForKeys(counter, queryStringHashBytes, decryptedKeysToBeSentToEnclave);
		byte[] sessionKey = sqlEnclaveSession.GetSessionKey();
		byte[] arr = EncryptBytePackage(bytePackage, sessionKey, enclaveSessionParameters.ServerName);
		byte[] bytes = BitConverter.GetBytes(sqlEnclaveSession.SessionId);
		byte[] enclavePackageBytes = CombineByteArrays(bytes, arr);
		return new EnclavePackage(enclavePackageBytes, sqlEnclaveSession);
	}

	internal SqlEnclaveAttestationParameters GetAttestationParameters(SqlConnectionAttestationProtocol attestationProtocol, string enclaveType, string attestationUrl, byte[] customData, int customDataLength)
	{
		SqlColumnEncryptionEnclaveProvider enclaveProvider = GetEnclaveProvider(attestationProtocol, enclaveType);
		return enclaveProvider.GetAttestationParameters(attestationUrl, customData, customDataLength);
	}

	internal byte[] GetSerializedAttestationParameters(SqlEnclaveAttestationParameters sqlEnclaveAttestationParameters, string enclaveType)
	{
		byte[] array = null;
		byte[] array2 = null;
		byte[] array3 = null;
		int protocol = sqlEnclaveAttestationParameters.Protocol;
		array = GetUintBytes(enclaveType, protocol, "attestationProtocol");
		if (array == null)
		{
			throw SQL.NullArgumentInternal("attestationProtocolBytes", "EnclaveDelegate", "GetSerializedAttestationParameters");
		}
		byte[] input = sqlEnclaveAttestationParameters.GetInput();
		array2 = GetUintBytes(enclaveType, input.Length, "attestationProtocolInputLength");
		if (array2 == null)
		{
			throw SQL.NullArgumentInternal("attestationProtocolInputLengthBytes", "EnclaveDelegate", "GetSerializedAttestationParameters");
		}
		byte[] eCDiffieHellmanPublicKeyBlob = KeyConverter.GetECDiffieHellmanPublicKeyBlob(sqlEnclaveAttestationParameters.ClientDiffieHellmanKey);
		array3 = GetUintBytes(enclaveType, eCDiffieHellmanPublicKeyBlob.Length, "clientDHPublicKeyLength");
		if (array3 == null)
		{
			throw SQL.NullArgumentInternal("clientDHPublicKeyLengthBytes", "EnclaveDelegate", "GetSerializedAttestationParameters");
		}
		return CombineByteArrays(array, array2, input, array3, eCDiffieHellmanPublicKeyBlob);
	}

	private string ConvertAttestationProtocolToString(SqlConnectionAttestationProtocol attestationProtocol)
	{
		return attestationProtocol switch
		{
			SqlConnectionAttestationProtocol.AAS => "AAS", 
			SqlConnectionAttestationProtocol.HGS => "HGS", 
			_ => "NotSpecified", 
		};
	}
}

using System;
using System.Collections.Concurrent;
using System.Text;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlClientEncryptionAlgorithmFactoryList
{
	private readonly ConcurrentDictionary<string, SqlClientEncryptionAlgorithmFactory> _encryptionAlgoFactoryList;

	private static readonly SqlClientEncryptionAlgorithmFactoryList _singletonInstance = new SqlClientEncryptionAlgorithmFactoryList();

	private SqlClientEncryptionAlgorithmFactoryList()
	{
		_encryptionAlgoFactoryList = new ConcurrentDictionary<string, SqlClientEncryptionAlgorithmFactory>(4 * Environment.ProcessorCount, 2);
		_encryptionAlgoFactoryList.TryAdd("AEAD_AES_256_CBC_HMAC_SHA256", new SqlAeadAes256CbcHmac256Factory());
	}

	internal static SqlClientEncryptionAlgorithmFactoryList GetInstance()
	{
		return _singletonInstance;
	}

	internal string GetRegisteredCipherAlgorithmNames()
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		foreach (string key in _encryptionAlgoFactoryList.Keys)
		{
			if (flag)
			{
				stringBuilder.Append("'");
				flag = false;
			}
			else
			{
				stringBuilder.Append(", '");
			}
			stringBuilder.Append(key);
			stringBuilder.Append("'");
		}
		return stringBuilder.ToString();
	}

	internal void GetAlgorithm(SqlClientSymmetricKey key, byte type, string algorithmName, out SqlClientEncryptionAlgorithm encryptionAlgorithm)
	{
		encryptionAlgorithm = null;
		SqlClientEncryptionAlgorithmFactory value = null;
		if (!_encryptionAlgoFactoryList.TryGetValue(algorithmName, out value))
		{
			throw SQL.UnknownColumnEncryptionAlgorithm(algorithmName, GetInstance().GetRegisteredCipherAlgorithmNames());
		}
		encryptionAlgorithm = value.Create(key, (SqlClientEncryptionType)type, algorithmName);
	}
}

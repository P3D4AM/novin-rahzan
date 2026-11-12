using System;
using System.Collections.Concurrent;
using System.Text;

namespace Microsoft.Data.SqlClient;

internal class SqlAeadAes256CbcHmac256Factory : SqlClientEncryptionAlgorithmFactory
{
	private readonly ConcurrentDictionary<string, SqlAeadAes256CbcHmac256Algorithm> _encryptionAlgorithms = new ConcurrentDictionary<string, SqlAeadAes256CbcHmac256Algorithm>(4 * Environment.ProcessorCount, 2);

	internal override SqlClientEncryptionAlgorithm Create(SqlClientSymmetricKey encryptionKey, SqlClientEncryptionType encryptionType, string encryptionAlgorithm)
	{
		if (encryptionType != SqlClientEncryptionType.Deterministic && encryptionType != SqlClientEncryptionType.Randomized)
		{
			throw SQL.InvalidEncryptionType("AEAD_AES_256_CBC_HMAC_SHA256", encryptionType, SqlClientEncryptionType.Deterministic, SqlClientEncryptionType.Randomized);
		}
		StringBuilder stringBuilder = new StringBuilder(Convert.ToBase64String(encryptionKey.RootKey), SqlSecurityUtility.GetBase64LengthFromByteLength(encryptionKey.RootKey.Length) + 4);
		stringBuilder.Append(":");
		stringBuilder.Append((int)encryptionType);
		stringBuilder.Append(":");
		stringBuilder.Append((byte)1);
		string key = stringBuilder.ToString();
		if (!_encryptionAlgorithms.TryGetValue(key, out var value))
		{
			SqlAeadAes256CbcHmac256EncryptionKey encryptionKey2 = new SqlAeadAes256CbcHmac256EncryptionKey(encryptionKey.RootKey, "AEAD_AES_256_CBC_HMAC_SHA256");
			value = new SqlAeadAes256CbcHmac256Algorithm(encryptionKey2, encryptionType, 1);
			_encryptionAlgorithms.TryAdd(key, value);
		}
		return value;
	}
}

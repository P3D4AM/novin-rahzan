using System;
using System.Collections.Specialized;
using System.Runtime.Caching;
using System.Text;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlSymmetricKeyCache
{
	private readonly MemoryCache _cache;

	private static readonly SqlSymmetricKeyCache _singletonInstance = new SqlSymmetricKeyCache();

	private SqlSymmetricKeyCache()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		_cache = new MemoryCache("ColumnEncryptionKeyCache", (NameValueCollection)null);
	}

	internal static SqlSymmetricKeyCache GetInstance()
	{
		return _singletonInstance;
	}

	internal SqlClientSymmetricKey GetKey(SqlEncryptionKeyInfo keyInfo, SqlConnection connection, SqlCommand command)
	{
		string dataSource = connection.DataSource;
		StringBuilder stringBuilder = new StringBuilder(dataSource, dataSource.Length + SqlSecurityUtility.GetBase64LengthFromByteLength(keyInfo.encryptedKey.Length) + keyInfo.keyStoreName.Length + 2);
		stringBuilder.Append(":");
		stringBuilder.Append(Convert.ToBase64String(keyInfo.encryptedKey));
		stringBuilder.Append(":");
		stringBuilder.Append(keyInfo.keyStoreName);
		string text = stringBuilder.ToString();
		SqlClientSymmetricKey sqlClientSymmetricKey = ((ObjectCache)_cache).Get(text, (string)null) as SqlClientSymmetricKey;
		if (sqlClientSymmetricKey == null)
		{
			SqlSecurityUtility.ThrowIfKeyPathIsNotTrustedForServer(dataSource, keyInfo.keyPath);
			if (!SqlSecurityUtility.TryGetColumnEncryptionKeyStoreProvider(keyInfo.keyStoreName, out var provider, connection, command))
			{
				throw SQL.UnrecognizedKeyStoreProviderName(keyInfo.keyStoreName, SqlConnection.GetColumnEncryptionSystemKeyStoreProvidersNames(), SqlSecurityUtility.GetListOfProviderNamesThatWereSearched(connection, command));
			}
			byte[] rootKey;
			try
			{
				provider.ColumnEncryptionKeyCacheTtl = new TimeSpan(0L);
				rootKey = provider.DecryptColumnEncryptionKey(keyInfo.keyPath, keyInfo.algorithmName, keyInfo.encryptedKey);
			}
			catch (Exception e)
			{
				string bytesAsString = SqlSecurityUtility.GetBytesAsString(keyInfo.encryptedKey, fLast: true, 10);
				throw SQL.KeyDecryptionFailed(keyInfo.keyStoreName, bytesAsString, e);
			}
			sqlClientSymmetricKey = new SqlClientSymmetricKey(rootKey);
			if (SqlConnection.ColumnEncryptionKeyCacheTtl != TimeSpan.Zero)
			{
				DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow.Add(SqlConnection.ColumnEncryptionKeyCacheTtl);
				((ObjectCache)_cache).Add(text, (object)sqlClientSymmetricKey, dateTimeOffset, (string)null);
			}
		}
		return sqlClientSymmetricKey;
	}
}

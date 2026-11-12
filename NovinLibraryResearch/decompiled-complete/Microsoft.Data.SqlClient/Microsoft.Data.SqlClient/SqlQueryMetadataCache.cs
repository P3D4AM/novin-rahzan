using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Runtime.Caching;
using System.Text;
using System.Threading;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlQueryMetadataCache
{
	private const int CacheSize = 2000;

	private const int CacheTrimThreshold = 300;

	private readonly MemoryCache _cache;

	private static readonly SqlQueryMetadataCache _singletonInstance = new SqlQueryMetadataCache();

	private int _inTrim;

	private long _cacheHits;

	private long _cacheMisses;

	private SqlQueryMetadataCache()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		_cache = new MemoryCache("SqlQueryMetadataCache", (NameValueCollection)null);
		_inTrim = 0;
		_cacheHits = 0L;
		_cacheMisses = 0L;
	}

	internal static SqlQueryMetadataCache GetInstance()
	{
		return _singletonInstance;
	}

	internal bool GetQueryMetadataIfExists(SqlCommand sqlCommand)
	{
		if (!SqlConnection.ColumnEncryptionQueryMetadataCacheEnabled)
		{
			return false;
		}
		Tuple<string, string> cacheLookupKeysFromSqlCommand = GetCacheLookupKeysFromSqlCommand(sqlCommand);
		string text = cacheLookupKeysFromSqlCommand?.Item1;
		string text2 = cacheLookupKeysFromSqlCommand?.Item2;
		if (text == null)
		{
			IncrementCacheMisses();
			return false;
		}
		if (!(((ObjectCache)_cache).Get(text, (string)null) is Dictionary<string, SqlCipherMetadata> dictionary))
		{
			IncrementCacheMisses();
			return false;
		}
		foreach (SqlParameter parameter in sqlCommand.Parameters)
		{
			if (!dictionary.TryGetValue(parameter.ParameterNameFixed, out var value))
			{
				foreach (SqlParameter parameter2 in sqlCommand.Parameters)
				{
					parameter2.CipherMetadata = null;
				}
				IncrementCacheMisses();
				return false;
			}
			parameter.CipherMetadata = value;
		}
		foreach (SqlParameter parameter3 in sqlCommand.Parameters)
		{
			SqlCipherMetadata sqlCipherMetadata = null;
			if (parameter3.CipherMetadata != null)
			{
				sqlCipherMetadata = new SqlCipherMetadata(parameter3.CipherMetadata.EncryptionInfo, 0, parameter3.CipherMetadata.CipherAlgorithmId, parameter3.CipherMetadata.CipherAlgorithmName, parameter3.CipherMetadata.EncryptionType, parameter3.CipherMetadata.NormalizationRuleVersion);
			}
			parameter3.CipherMetadata = sqlCipherMetadata;
			if (sqlCipherMetadata == null)
			{
				continue;
			}
			try
			{
				SqlSecurityUtility.DecryptSymmetricKey(sqlCipherMetadata, sqlCommand.Connection, sqlCommand);
			}
			catch (Exception ex)
			{
				InvalidateCacheEntry(sqlCommand);
				if (ex is SqlException || ex is ArgumentException || ex is ArgumentNullException)
				{
					foreach (SqlParameter parameter4 in sqlCommand.Parameters)
					{
						parameter4.CipherMetadata = null;
					}
					IncrementCacheMisses();
					return false;
				}
				throw;
			}
		}
		if (((ObjectCache)_cache).Get(text2, (string)null) is ConcurrentDictionary<int, SqlTceCipherInfoEntry> keysToBeSentToEnclave)
		{
			sqlCommand.keysToBeSentToEnclave = CreateCopyOfEnclaveKeys(keysToBeSentToEnclave);
		}
		IncrementCacheHits();
		return true;
	}

	internal void AddQueryMetadata(SqlCommand sqlCommand, bool ignoreQueriesWithReturnValueParams)
	{
		if (!SqlConnection.ColumnEncryptionQueryMetadataCacheEnabled)
		{
			return;
		}
		if (sqlCommand.CommandType == CommandType.StoredProcedure)
		{
			foreach (SqlParameter parameter in sqlCommand.Parameters)
			{
				if (parameter.Direction == ParameterDirection.ReturnValue && ignoreQueriesWithReturnValueParams)
				{
					sqlCommand.CachingQueryMetadataPostponed = true;
					return;
				}
			}
		}
		Tuple<string, string> cacheLookupKeysFromSqlCommand = GetCacheLookupKeysFromSqlCommand(sqlCommand);
		string text = cacheLookupKeysFromSqlCommand?.Item1;
		string text2 = cacheLookupKeysFromSqlCommand?.Item2;
		if (text == null)
		{
			return;
		}
		Dictionary<string, SqlCipherMetadata> dictionary = new Dictionary<string, SqlCipherMetadata>(sqlCommand.Parameters.Count);
		foreach (SqlParameter parameter2 in sqlCommand.Parameters)
		{
			SqlCipherMetadata value = null;
			if (parameter2.CipherMetadata != null)
			{
				value = new SqlCipherMetadata(parameter2.CipherMetadata.EncryptionInfo, 0, parameter2.CipherMetadata.CipherAlgorithmId, parameter2.CipherMetadata.CipherAlgorithmName, parameter2.CipherMetadata.EncryptionType, parameter2.CipherMetadata.NormalizationRuleVersion);
			}
			dictionary.Add(parameter2.ParameterNameFixed, value);
		}
		long count = ((ObjectCache)_cache).GetCount((string)null);
		if (count > 2300 && Interlocked.CompareExchange(ref _inTrim, 1, 0) == 0)
		{
			try
			{
				_cache.Trim((int)((double)(count - 2000) / (double)count * 100.0));
			}
			finally
			{
				Interlocked.CompareExchange(ref _inTrim, 0, 1);
			}
		}
		((ObjectCache)_cache).Set(text, (object)dictionary, DateTimeOffset.UtcNow.AddHours(10.0), (string)null);
		if (sqlCommand.requiresEnclaveComputations)
		{
			ConcurrentDictionary<int, SqlTceCipherInfoEntry> concurrentDictionary = CreateCopyOfEnclaveKeys(sqlCommand.keysToBeSentToEnclave);
			((ObjectCache)_cache).Set(text2, (object)concurrentDictionary, DateTimeOffset.UtcNow.AddHours(10.0), (string)null);
		}
	}

	internal void InvalidateCacheEntry(SqlCommand sqlCommand)
	{
		Tuple<string, string> cacheLookupKeysFromSqlCommand = GetCacheLookupKeysFromSqlCommand(sqlCommand);
		string text = cacheLookupKeysFromSqlCommand?.Item1;
		string text2 = cacheLookupKeysFromSqlCommand?.Item2;
		if (text != null)
		{
			((ObjectCache)_cache).Remove(text, (string)null);
			((ObjectCache)_cache).Remove(text2, (string)null);
		}
	}

	private void IncrementCacheHits()
	{
		Interlocked.Increment(ref _cacheHits);
	}

	private void IncrementCacheMisses()
	{
		Interlocked.Increment(ref _cacheMisses);
	}

	private void ResetCacheCounts()
	{
		_cacheHits = 0L;
		_cacheMisses = 0L;
	}

	private Tuple<string, string> GetCacheLookupKeysFromSqlCommand(SqlCommand sqlCommand)
	{
		SqlConnection connection = sqlCommand.Connection;
		if (connection == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder(connection.DataSource, connection.DataSource.Length + 128 + sqlCommand.CommandText.Length + 6);
		stringBuilder.Append(":::");
		stringBuilder.Append(connection.Database.PadRight(128));
		stringBuilder.Append(":::");
		stringBuilder.Append(sqlCommand.CommandText);
		string item = stringBuilder.ToString();
		string item2 = stringBuilder.Append(":::enclaveKeys").ToString();
		return Tuple.Create(item, item2);
	}

	private ConcurrentDictionary<int, SqlTceCipherInfoEntry> CreateCopyOfEnclaveKeys(ConcurrentDictionary<int, SqlTceCipherInfoEntry> keysToBeSentToEnclave)
	{
		ConcurrentDictionary<int, SqlTceCipherInfoEntry> concurrentDictionary = new ConcurrentDictionary<int, SqlTceCipherInfoEntry>();
		foreach (KeyValuePair<int, SqlTceCipherInfoEntry> item in keysToBeSentToEnclave)
		{
			int key = item.Key;
			SqlTceCipherInfoEntry value = item.Value;
			SqlTceCipherInfoEntry sqlTceCipherInfoEntry = new SqlTceCipherInfoEntry(key);
			foreach (SqlEncryptionKeyInfo columnEncryptionKeyValue in value.ColumnEncryptionKeyValues)
			{
				sqlTceCipherInfoEntry.Add(columnEncryptionKeyValue.encryptedKey, columnEncryptionKeyValue.databaseId, columnEncryptionKeyValue.cekId, columnEncryptionKeyValue.cekVersion, columnEncryptionKeyValue.cekMdVersion, columnEncryptionKeyValue.keyPath, columnEncryptionKeyValue.keyStoreName, columnEncryptionKeyValue.algorithmName);
			}
			concurrentDictionary.TryAdd(key, sqlTceCipherInfoEntry);
		}
		return concurrentDictionary;
	}
}

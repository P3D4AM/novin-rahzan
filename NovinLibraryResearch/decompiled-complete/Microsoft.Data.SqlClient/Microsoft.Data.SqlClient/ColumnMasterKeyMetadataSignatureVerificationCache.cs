using System;
using System.Collections.Specialized;
using System.Runtime.Caching;
using System.Text;
using System.Threading;

namespace Microsoft.Data.SqlClient;

internal class ColumnMasterKeyMetadataSignatureVerificationCache
{
	private const int _cacheSize = 2000;

	private const int _cacheTrimThreshold = 300;

	private const string _className = "ColumnMasterKeyMetadataSignatureVerificationCache";

	private const string _getSignatureVerificationResultMethodName = "GetSignatureVerificationResult";

	private const string _addSignatureVerificationResultMethodName = "AddSignatureVerificationResult";

	private const string _masterkeypathArgumentName = "masterKeyPath";

	private const string _keyStoreNameArgumentName = "keyStoreName";

	private const string _signatureName = "signature";

	private const string _cacheLookupKeySeparator = ":";

	private static readonly ColumnMasterKeyMetadataSignatureVerificationCache _signatureVerificationCache = new ColumnMasterKeyMetadataSignatureVerificationCache();

	private readonly MemoryCache _cache;

	private int _inTrim;

	internal static ColumnMasterKeyMetadataSignatureVerificationCache Instance => _signatureVerificationCache;

	private ColumnMasterKeyMetadataSignatureVerificationCache()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		_cache = new MemoryCache("ColumnMasterKeyMetadataSignatureVerificationCache", (NameValueCollection)null);
		_inTrim = 0;
	}

	internal bool? GetSignatureVerificationResult(string keyStoreName, string masterKeyPath, bool allowEnclaveComputations, byte[] signature)
	{
		ValidateStringArgumentNotNullOrEmpty(masterKeyPath, "masterKeyPath", "GetSignatureVerificationResult");
		ValidateStringArgumentNotNullOrEmpty(keyStoreName, "keyStoreName", "GetSignatureVerificationResult");
		ValidateSignatureNotNullOrEmpty(signature, "GetSignatureVerificationResult");
		string cacheLookupKey = GetCacheLookupKey(masterKeyPath, allowEnclaveComputations, signature, keyStoreName);
		return ((ObjectCache)_cache).Get(cacheLookupKey, (string)null) as bool?;
	}

	internal void AddSignatureVerificationResult(string keyStoreName, string masterKeyPath, bool allowEnclaveComputations, byte[] signature, bool result)
	{
		ValidateStringArgumentNotNullOrEmpty(masterKeyPath, "masterKeyPath", "AddSignatureVerificationResult");
		ValidateStringArgumentNotNullOrEmpty(keyStoreName, "keyStoreName", "AddSignatureVerificationResult");
		ValidateSignatureNotNullOrEmpty(signature, "AddSignatureVerificationResult");
		string cacheLookupKey = GetCacheLookupKey(masterKeyPath, allowEnclaveComputations, signature, keyStoreName);
		TrimCacheIfNeeded();
		((ObjectCache)_cache).Set(cacheLookupKey, (object)result, DateTimeOffset.UtcNow.AddDays(10.0), (string)null);
	}

	private void ValidateSignatureNotNullOrEmpty(byte[] signature, string methodName)
	{
		if (signature == null || signature.Length == 0)
		{
			if (signature == null)
			{
				throw SQL.NullArgumentInternal("signature", "ColumnMasterKeyMetadataSignatureVerificationCache", methodName);
			}
			throw SQL.EmptyArgumentInternal("signature", "ColumnMasterKeyMetadataSignatureVerificationCache", methodName);
		}
	}

	private void ValidateStringArgumentNotNullOrEmpty(string stringArgValue, string stringArgName, string methodName)
	{
		if (string.IsNullOrWhiteSpace(stringArgValue))
		{
			if (stringArgValue == null)
			{
				throw SQL.NullArgumentInternal(stringArgName, "ColumnMasterKeyMetadataSignatureVerificationCache", methodName);
			}
			throw SQL.EmptyArgumentInternal(stringArgName, "ColumnMasterKeyMetadataSignatureVerificationCache", methodName);
		}
	}

	private void TrimCacheIfNeeded()
	{
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
	}

	private string GetCacheLookupKey(string masterKeyPath, bool allowEnclaveComputations, byte[] signature, string keyStoreName)
	{
		StringBuilder stringBuilder = new StringBuilder(keyStoreName, keyStoreName.Length + masterKeyPath.Length + SqlSecurityUtility.GetBase64LengthFromByteLength(signature.Length) + 3 + 10);
		stringBuilder.Append(":");
		stringBuilder.Append(masterKeyPath);
		stringBuilder.Append(":");
		stringBuilder.Append(allowEnclaveComputations);
		stringBuilder.Append(":");
		stringBuilder.Append(Convert.ToBase64String(signature));
		stringBuilder.Append(":");
		return stringBuilder.ToString();
	}
}

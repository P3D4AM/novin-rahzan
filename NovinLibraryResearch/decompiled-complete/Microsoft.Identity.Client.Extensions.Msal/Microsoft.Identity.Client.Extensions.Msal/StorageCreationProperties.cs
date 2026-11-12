using System.Collections.Generic;
using System.IO;

namespace Microsoft.Identity.Client.Extensions.Msal;

public class StorageCreationProperties
{
	public readonly string CacheFileName;

	public readonly string CacheDirectory;

	public readonly string MacKeyChainServiceName;

	public readonly string MacKeyChainAccountName;

	public readonly string KeyringSchemaName;

	public readonly string KeyringCollection;

	public readonly string KeyringSecretLabel;

	public readonly KeyValuePair<string, string> KeyringAttribute1;

	public readonly KeyValuePair<string, string> KeyringAttribute2;

	public readonly int LockRetryDelay;

	public readonly bool UseLinuxUnencryptedFallback;

	public readonly int LockRetryCount;

	public string CacheFilePath { get; }

	public string ClientId { get; }

	internal StorageCreationProperties(string cacheFileName, string cacheDirectory, string macKeyChainServiceName, string macKeyChainAccountName, bool useLinuxPlaintextFallback, string keyringSchemaName, string keyringCollection, string keyringSecretLabel, KeyValuePair<string, string> keyringAttribute1, KeyValuePair<string, string> keyringAttribute2, string clientId, int lockRetryDelay, int lockRetryCount)
	{
		CacheFileName = cacheFileName;
		CacheDirectory = cacheDirectory;
		CacheFilePath = Path.Combine(CacheDirectory, CacheFileName);
		MacKeyChainServiceName = macKeyChainServiceName;
		MacKeyChainAccountName = macKeyChainAccountName;
		UseLinuxUnencryptedFallback = useLinuxPlaintextFallback;
		KeyringSchemaName = keyringSchemaName;
		KeyringCollection = keyringCollection;
		KeyringSecretLabel = keyringSecretLabel;
		KeyringAttribute1 = keyringAttribute1;
		KeyringAttribute2 = keyringAttribute2;
		ClientId = clientId;
		LockRetryDelay = lockRetryDelay;
		LockRetryCount = lockRetryCount;
	}
}

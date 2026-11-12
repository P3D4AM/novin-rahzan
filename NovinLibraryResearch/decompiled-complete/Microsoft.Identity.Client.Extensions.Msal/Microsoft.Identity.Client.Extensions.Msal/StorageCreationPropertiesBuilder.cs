using System;
using System.Collections.Generic;

namespace Microsoft.Identity.Client.Extensions.Msal;

public class StorageCreationPropertiesBuilder
{
	private readonly string _cacheFileName;

	private readonly string _cacheDirectory;

	private readonly string _clientId;

	private string _macKeyChainServiceName;

	private string _macKeyChainAccountName;

	private string _keyringSchemaName;

	private string _keyringCollection;

	private string _keyringSecretLabel;

	private KeyValuePair<string, string> _keyringAttribute1;

	private KeyValuePair<string, string> _keyringAttribute2;

	private int _lockRetryDelay = 100;

	private int _lockRetryCount = 600;

	private bool _useLinuxPlaintextFallback;

	public StorageCreationPropertiesBuilder(string cacheFileName, string cacheDirectory, string clientId)
	{
		_cacheFileName = cacheFileName;
		_cacheDirectory = cacheDirectory;
		_clientId = clientId;
	}

	public StorageCreationProperties Build()
	{
		return new StorageCreationProperties(_cacheFileName, _cacheDirectory, _macKeyChainServiceName, _macKeyChainAccountName, _useLinuxPlaintextFallback, _keyringSchemaName, _keyringCollection, _keyringSecretLabel, _keyringAttribute1, _keyringAttribute2, _clientId, _lockRetryDelay, _lockRetryCount);
	}

	public StorageCreationPropertiesBuilder WithMacKeyChain(string serviceName, string accountName)
	{
		_macKeyChainServiceName = serviceName;
		_macKeyChainAccountName = accountName;
		return this;
	}

	public StorageCreationPropertiesBuilder CustomizeLockRetry(int lockRetryDelay, int lockRetryCount)
	{
		if (lockRetryDelay < 1)
		{
			throw new ArgumentOutOfRangeException("lockRetryDelay");
		}
		if (lockRetryCount < 1)
		{
			throw new ArgumentOutOfRangeException("lockRetryCount");
		}
		_lockRetryCount = lockRetryCount;
		_lockRetryDelay = lockRetryDelay;
		return this;
	}

	public StorageCreationPropertiesBuilder WithLinuxKeyring(string schemaName, string collection, string secretLabel, KeyValuePair<string, string> attribute1, KeyValuePair<string, string> attribute2)
	{
		if (string.IsNullOrEmpty(schemaName))
		{
			throw new ArgumentNullException("schemaName");
		}
		if (_useLinuxPlaintextFallback)
		{
			throw new ArgumentException("WithLinuxUnprotectedFile and WithLinuxKeyring are mutually exclusive. Only one storage mechanism per OS is supported. ");
		}
		_keyringSchemaName = schemaName;
		_keyringCollection = collection;
		_keyringSecretLabel = secretLabel;
		_keyringAttribute1 = attribute1;
		_keyringAttribute2 = attribute2;
		return this;
	}

	public StorageCreationPropertiesBuilder WithLinuxUnprotectedFile()
	{
		if (!string.IsNullOrEmpty(_keyringSchemaName))
		{
			throw new ArgumentException("WithLinuxUnprotectedFile and WithLinuxKeyring are mutually exclusive. Only one storage mechanism per OS is supported. ");
		}
		_useLinuxPlaintextFallback = true;
		return this;
	}
}

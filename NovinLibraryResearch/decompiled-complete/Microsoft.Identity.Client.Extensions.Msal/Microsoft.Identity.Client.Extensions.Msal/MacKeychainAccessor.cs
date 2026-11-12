using System;

namespace Microsoft.Identity.Client.Extensions.Msal;

internal class MacKeychainAccessor : ICacheAccessor
{
	private readonly string _cacheFilePath;

	private readonly string _keyChainServiceName;

	private readonly string _keyChainAccountName;

	private readonly TraceSourceLogger _logger;

	public MacKeychainAccessor(string cacheFilePath, string keyChainServiceName, string keyChainAccountName, TraceSourceLogger logger)
	{
		if (string.IsNullOrWhiteSpace(cacheFilePath))
		{
			throw new ArgumentNullException("cacheFilePath");
		}
		if (string.IsNullOrWhiteSpace(keyChainServiceName))
		{
			throw new ArgumentNullException("keyChainServiceName");
		}
		if (string.IsNullOrWhiteSpace(keyChainAccountName))
		{
			throw new ArgumentNullException("keyChainAccountName");
		}
		_cacheFilePath = cacheFilePath;
		_keyChainServiceName = keyChainServiceName;
		_keyChainAccountName = keyChainAccountName;
		_logger = logger ?? throw new ArgumentNullException("logger");
	}

	public void Clear()
	{
		_logger.LogInformation("Clearing cache");
		FileIOWithRetries.DeleteCacheFile(_cacheFilePath, _logger);
		_logger.LogInformation("Before delete mac keychain");
		MacKeyChain.DeleteKey(_keyChainServiceName, _keyChainAccountName);
		_logger.LogInformation("After delete mac keychain");
	}

	public byte[] Read()
	{
		_logger.LogInformation("ReadDataCore");
		_logger.LogInformation("ReadDataCore, Before reading from mac keychain");
		byte[] array = MacKeyChain.RetrieveKey(_keyChainServiceName, _keyChainAccountName, _logger);
		_logger.LogInformation($"ReadDataCore, read '{array?.Length}' bytes from the keychain");
		return array;
	}

	public void Write(byte[] data)
	{
		_logger.LogInformation("Before write to mac keychain");
		MacKeyChain.WriteKey(_keyChainServiceName, _keyChainAccountName, data);
		_logger.LogInformation("After write to mac keychain");
		FileIOWithRetries.TouchFile(_cacheFilePath, _logger);
	}

	public ICacheAccessor CreateForPersistenceValidation()
	{
		return new MacKeychainAccessor(_cacheFilePath + ".test", _keyChainServiceName + "test", _keyChainAccountName + "test", _logger);
	}
}

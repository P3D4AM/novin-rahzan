using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Microsoft.Identity.Client.Extensions.Msal;

internal class MsalCacheStorage
{
	private readonly TraceSourceLogger _logger;

	public const string PersistenceValidationDummyData = "msal_persistence_test";

	private static readonly Lazy<TraceSourceLogger> s_staticLogger = new Lazy<TraceSourceLogger>(() => new TraceSourceLogger(EnvUtils.GetNewTraceSource("MsalCacheHelperSingleton")));

	internal ICacheAccessor CacheAccessor { get; }

	public StorageCreationProperties StorageCreationProperties { get; }

	public string CacheFilePath => StorageCreationProperties.CacheFilePath;

	public static MsalCacheStorage Create(StorageCreationProperties creationProperties, TraceSource logger = null)
	{
		TraceSourceLogger logger2 = ((logger == null) ? s_staticLogger.Value : new TraceSourceLogger(logger));
		ICacheAccessor cacheAccessor;
		if (SharedUtilities.IsWindowsPlatform())
		{
			cacheAccessor = new DpApiEncryptedFileAccessor(creationProperties.CacheFilePath, logger2);
		}
		else if (SharedUtilities.IsMacPlatform())
		{
			cacheAccessor = new MacKeychainAccessor(creationProperties.CacheFilePath, creationProperties.MacKeyChainServiceName, creationProperties.MacKeyChainAccountName, logger2);
		}
		else
		{
			if (!SharedUtilities.IsLinuxPlatform())
			{
				throw new PlatformNotSupportedException();
			}
			if (creationProperties.UseLinuxUnencryptedFallback)
			{
				cacheAccessor = new FileAccessor(creationProperties.CacheFilePath, logger2);
			}
			else
			{
				string cacheFilePath = creationProperties.CacheFilePath;
				string keyringCollection = creationProperties.KeyringCollection;
				string keyringSchemaName = creationProperties.KeyringSchemaName;
				string keyringSecretLabel = creationProperties.KeyringSecretLabel;
				KeyValuePair<string, string> keyringAttribute = creationProperties.KeyringAttribute1;
				string key = keyringAttribute.Key;
				keyringAttribute = creationProperties.KeyringAttribute1;
				string value = keyringAttribute.Value;
				keyringAttribute = creationProperties.KeyringAttribute2;
				string key2 = keyringAttribute.Key;
				keyringAttribute = creationProperties.KeyringAttribute2;
				cacheAccessor = new LinuxKeyringAccessor(cacheFilePath, keyringCollection, keyringSchemaName, keyringSecretLabel, key, value, key2, keyringAttribute.Value, logger2);
			}
		}
		return new MsalCacheStorage(creationProperties, cacheAccessor, logger2);
	}

	internal MsalCacheStorage(StorageCreationProperties creationProperties, ICacheAccessor cacheAccessor, TraceSourceLogger logger)
	{
		StorageCreationProperties = creationProperties;
		_logger = logger;
		CacheAccessor = cacheAccessor;
		_logger.LogInformation("Initialized 'MsalCacheStorage'");
	}

	public byte[] ReadData(bool ignoreExceptions = true)
	{
		bool flag = File.Exists(CacheFilePath);
		_logger.LogInformation($"ReadData Cache file exists '{flag}'");
		byte[] array = null;
		try
		{
			_logger.LogInformation("Reading Data");
			array = CacheAccessor.Read();
			_logger.LogInformation($"Got '{array?.Length}' bytes from file storage");
		}
		catch (Exception arg)
		{
			_logger.LogError(string.Format("An exception was encountered while reading data from the {0} : {1}", "MsalCacheStorage", arg));
			if (!ignoreExceptions)
			{
				throw;
			}
		}
		return array ?? new byte[0];
	}

	public void WriteData(byte[] data, bool ignoreExceptions = true)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		try
		{
			_logger.LogInformation($"Got '{data?.Length}' bytes to write to storage");
			CacheAccessor.Write(data);
		}
		catch (Exception arg)
		{
			_logger.LogError(string.Format("An exception was encountered while writing data to {0} : {1}", "MsalCacheStorage", arg));
			if (!ignoreExceptions)
			{
				throw;
			}
		}
	}

	public void Clear(bool ignoreExceptions = true)
	{
		try
		{
			_logger.LogInformation("Clearing the cache file");
			CacheAccessor.Clear();
		}
		catch (Exception arg)
		{
			_logger.LogError(string.Format("An exception was encountered while clearing data from {0} : {1}", "MsalCacheStorage", arg));
			if (!ignoreExceptions)
			{
				throw;
			}
		}
	}

	public void VerifyPersistence()
	{
		ICacheAccessor cacheAccessor = CacheAccessor.CreateForPersistenceValidation();
		try
		{
			_logger.LogInformation("[Verify Persistence] Writing Data ");
			cacheAccessor.Write(Encoding.UTF8.GetBytes("msal_persistence_test"));
			_logger.LogInformation("[Verify Persistence] Reading Data ");
			byte[] array = cacheAccessor.Read();
			if (array == null || array.Length == 0)
			{
				throw new MsalCachePersistenceException("Persistence check failed. Data was written but it could not be read. Possible cause: on Linux, LibSecret is installed but D-Bus isn't running because it cannot be started over SSH.");
			}
			string @string = Encoding.UTF8.GetString(array);
			if (!string.Equals("msal_persistence_test", @string, StringComparison.Ordinal))
			{
				throw new MsalCachePersistenceException("Persistence check failed. Data written msal_persistence_test is different from data read " + @string);
			}
		}
		catch (Exception ex) when (!(ex is MsalCachePersistenceException))
		{
			throw new MsalCachePersistenceException("Persistence check failed. Inspect inner exception for details", ex);
		}
		finally
		{
			try
			{
				_logger.LogInformation("[Verify Persistence] Clearing data");
				cacheAccessor.Clear();
			}
			catch (Exception ex2)
			{
				_logger.LogError("[Verify Persistence] Could not clear the test data: " + ex2);
			}
		}
	}
}

using System;
using System.Security.Cryptography;

namespace Microsoft.Identity.Client.Extensions.Msal;

internal class DpApiEncryptedFileAccessor : ICacheAccessor
{
	private readonly string _cacheFilePath;

	private readonly TraceSourceLogger _logger;

	private readonly ICacheAccessor _unencryptedFileAccessor;

	public DpApiEncryptedFileAccessor(string cacheFilePath, TraceSourceLogger logger)
	{
		if (string.IsNullOrEmpty(cacheFilePath))
		{
			throw new ArgumentNullException("cacheFilePath");
		}
		_cacheFilePath = cacheFilePath;
		_logger = logger ?? throw new ArgumentNullException("logger");
		_unencryptedFileAccessor = new FileAccessor(_cacheFilePath, _logger);
	}

	public void Clear()
	{
		_logger.LogInformation("Clearing cache");
		_unencryptedFileAccessor.Clear();
	}

	public ICacheAccessor CreateForPersistenceValidation()
	{
		return new DpApiEncryptedFileAccessor(_cacheFilePath + ".test", _logger);
	}

	public byte[] Read()
	{
		byte[] array = _unencryptedFileAccessor.Read();
		if (array != null && array.Length != 0)
		{
			_logger.LogInformation("Unprotecting the data");
			array = ProtectedData.Unprotect(array, (byte[])null, (DataProtectionScope)0);
		}
		return array;
	}

	public void Write(byte[] data)
	{
		if (data.Length != 0)
		{
			_logger.LogInformation("Protecting the data");
			data = ProtectedData.Protect(data, (byte[])null, (DataProtectionScope)0);
		}
		_unencryptedFileAccessor.Write(data);
	}
}

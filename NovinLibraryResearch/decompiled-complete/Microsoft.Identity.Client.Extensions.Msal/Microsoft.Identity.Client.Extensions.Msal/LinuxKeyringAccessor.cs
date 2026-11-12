using System;
using System.Runtime.InteropServices;

namespace Microsoft.Identity.Client.Extensions.Msal;

internal class LinuxKeyringAccessor : ICacheAccessor
{
	private readonly TraceSourceLogger _logger;

	private IntPtr _libsecretSchema = IntPtr.Zero;

	private readonly string _cacheFilePath;

	private readonly string _keyringCollection;

	private readonly string _keyringSchemaName;

	private readonly string _keyringSecretLabel;

	private readonly string _attributeKey1;

	private readonly string _attributeValue1;

	private readonly string _attributeKey2;

	private readonly string _attributeValue2;

	public LinuxKeyringAccessor(string cacheFilePath, string keyringCollection, string keyringSchemaName, string keyringSecretLabel, string attributeKey1, string attributeValue1, string attributeKey2, string attributeValue2, TraceSourceLogger logger)
	{
		if (string.IsNullOrWhiteSpace(cacheFilePath))
		{
			throw new ArgumentNullException("cacheFilePath");
		}
		if (string.IsNullOrWhiteSpace(attributeKey1))
		{
			throw new ArgumentNullException("attributeKey1");
		}
		if (string.IsNullOrWhiteSpace(attributeValue1))
		{
			throw new ArgumentNullException("attributeValue1");
		}
		if (string.IsNullOrWhiteSpace(attributeKey2))
		{
			throw new ArgumentNullException("attributeKey2");
		}
		if (string.IsNullOrWhiteSpace(attributeValue2))
		{
			throw new ArgumentNullException("attributeValue2");
		}
		_cacheFilePath = cacheFilePath;
		_keyringCollection = keyringCollection;
		_keyringSchemaName = keyringSchemaName;
		_keyringSecretLabel = keyringSecretLabel;
		_attributeKey1 = attributeKey1;
		_attributeValue1 = attributeValue1;
		_attributeKey2 = attributeKey2;
		_attributeValue2 = attributeValue2;
		_logger = logger ?? throw new ArgumentNullException("logger");
	}

	public ICacheAccessor CreateForPersistenceValidation()
	{
		return new LinuxKeyringAccessor(_cacheFilePath + ".test", _keyringCollection, _keyringSchemaName, "MSAL Persistence Test", _attributeKey1, "test", _attributeKey2, "test", _logger);
	}

	public void Clear()
	{
		_logger.LogInformation("Clearing cache");
		FileIOWithRetries.DeleteCacheFile(_cacheFilePath, _logger);
		_logger.LogInformation("Before deleting secret from Linux keyring");
		IntPtr error = IntPtr.Zero;
		Libsecret.secret_password_clear_sync(GetLibsecretSchema(), IntPtr.Zero, out error, _attributeKey1, _attributeValue1, _attributeKey2, _attributeValue2, IntPtr.Zero);
		if (error != IntPtr.Zero)
		{
			try
			{
				GError gError = (GError)Marshal.PtrToStructure(error, typeof(GError));
				_logger.LogError(string.Format("An error was encountered while clearing secret from keyring in the {0} domain:'{1}' code:'{2}' message:'{3}'", "MsalCacheStorage", gError.Domain, gError.Code, gError.Message));
			}
			catch (Exception arg)
			{
				_logger.LogError(string.Format("An exception was encountered while processing libsecret error information during clearing secret in the {0} ex:'{1}'", "MsalCacheStorage", arg));
			}
		}
		_logger.LogInformation("After deleting secret from linux keyring");
	}

	public byte[] Read()
	{
		_logger.LogInformation("ReadDataCore");
		_logger.LogInformation("ReadDataCore, Before reading from linux keyring");
		byte[] array = null;
		IntPtr error = IntPtr.Zero;
		string text = Libsecret.secret_password_lookup_sync(GetLibsecretSchema(), IntPtr.Zero, out error, _attributeKey1, _attributeValue1, _attributeKey2, _attributeValue2, IntPtr.Zero);
		if (error != IntPtr.Zero)
		{
			try
			{
				GError gError = (GError)Marshal.PtrToStructure(error, typeof(GError));
				_logger.LogError(string.Format("An error was encountered while reading secret from keyring in the {0} domain:'{1}' code:'{2}' message:'{3}'", "MsalCacheStorage", gError.Domain, gError.Code, gError.Message));
			}
			catch (Exception arg)
			{
				_logger.LogError(string.Format("An exception was encountered while processing libsecret error information during reading in the {0} ex:'{1}'", "MsalCacheStorage", arg));
			}
		}
		else if (string.IsNullOrEmpty(text))
		{
			_logger.LogError("No matching secret found in the keyring");
		}
		else
		{
			_logger.LogInformation("Base64 decoding the secret string");
			array = Convert.FromBase64String(text);
			_logger.LogInformation($"ReadDataCore, read '{array?.Length}' bytes from the keyring");
		}
		return array;
	}

	public void Write(byte[] data)
	{
		_logger.LogInformation("Before saving to linux keyring");
		IntPtr error = IntPtr.Zero;
		Libsecret.secret_password_store_sync(GetLibsecretSchema(), _keyringCollection, _keyringSecretLabel, Convert.ToBase64String(data), IntPtr.Zero, out error, _attributeKey1, _attributeValue1, _attributeKey2, _attributeValue2, IntPtr.Zero);
		if (error != IntPtr.Zero)
		{
			try
			{
				GError gError = (GError)Marshal.PtrToStructure(error, typeof(GError));
				_logger.LogError(string.Format("An error was encountered while saving secret to keyring in the {0} domain:'{1}' code:'{2}' message:'{3}'", "MsalCacheStorage", gError.Domain, gError.Code, gError.Message));
			}
			catch (Exception arg)
			{
				_logger.LogError(string.Format("An exception was encountered while processing libsecret error information during saving in the {0} ex:'{1}'", "MsalCacheStorage", arg));
			}
		}
		_logger.LogInformation("After saving to linux keyring");
		FileIOWithRetries.TouchFile(_cacheFilePath, _logger);
	}

	private IntPtr GetLibsecretSchema()
	{
		if (_libsecretSchema == IntPtr.Zero)
		{
			_logger.LogInformation("Before creating libsecret schema");
			_libsecretSchema = Libsecret.secret_schema_new(_keyringSchemaName, 2, _attributeKey1, 0, _attributeKey2, 0, IntPtr.Zero);
			if (_libsecretSchema == IntPtr.Zero)
			{
				_logger.LogError("Failed to create libsecret schema from the MsalCacheStorage");
			}
			_logger.LogInformation("After creating libsecret schema");
		}
		return _libsecretSchema;
	}
}

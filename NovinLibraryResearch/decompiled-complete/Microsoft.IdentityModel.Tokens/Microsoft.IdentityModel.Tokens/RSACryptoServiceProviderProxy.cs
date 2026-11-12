using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class RSACryptoServiceProviderProxy : RSA
{
	private const int PROV_RSA_AES = 24;

	private const int PROV_RSA_FULL = 1;

	private const int PROV_RSA_SCHANNEL = 12;

	private bool _disposed;

	private bool _disposeRsa;

	private RSACryptoServiceProvider _rsa;

	public override string SignatureAlgorithm => _rsa.SignatureAlgorithm;

	public override string KeyExchangeAlgorithm => _rsa.KeyExchangeAlgorithm;

	public RSACryptoServiceProviderProxy(RSACryptoServiceProvider rsa)
	{
		if (rsa == null)
		{
			throw LogHelper.LogArgumentNullException("rsa");
		}
		bool num = rsa.CspKeyContainerInfo.ProviderType == 1 || rsa.CspKeyContainerInfo.ProviderType == 12;
		bool flag = Type.GetType("Mono.Runtime") != null;
		if (num && !rsa.CspKeyContainerInfo.HardwareDevice)
		{
			CspParameters cspParameters = new CspParameters
			{
				ProviderType = 24,
				KeyContainerName = rsa.CspKeyContainerInfo.KeyContainerName,
				KeyNumber = (int)rsa.CspKeyContainerInfo.KeyNumber
			};
			if (rsa.CspKeyContainerInfo.MachineKeyStore)
			{
				cspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
			}
			cspParameters.Flags |= CspProviderFlags.UseExistingKey;
			try
			{
				_rsa = new RSACryptoServiceProvider(cspParameters);
				_disposeRsa = true;
				return;
			}
			catch (CryptographicException) when (flag)
			{
				_rsa = rsa;
				return;
			}
		}
		_rsa = rsa;
	}

	public byte[] Decrypt(byte[] input, bool fOAEP)
	{
		if (input == null || input.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("input");
		}
		return _rsa.Decrypt(input, fOAEP);
	}

	public override byte[] DecryptValue(byte[] input)
	{
		if (input == null || input.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("input");
		}
		return _rsa.DecryptValue(input);
	}

	public byte[] Encrypt(byte[] input, bool fOAEP)
	{
		if (input == null || input.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("input");
		}
		return _rsa.Encrypt(input, fOAEP);
	}

	public override byte[] EncryptValue(byte[] input)
	{
		if (input == null || input.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("input");
		}
		return _rsa.EncryptValue(input);
	}

	public byte[] SignData(byte[] input, object hash)
	{
		if (input == null || input.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("input");
		}
		if (hash == null)
		{
			throw LogHelper.LogArgumentNullException("hash");
		}
		return _rsa.SignData(input, hash);
	}

	public bool VerifyData(byte[] input, object hash, byte[] signature)
	{
		if (input == null || input.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("input");
		}
		if (hash == null)
		{
			throw LogHelper.LogArgumentNullException("hash");
		}
		if (signature == null || signature.Length == 0)
		{
			throw LogHelper.LogArgumentNullException("signature");
		}
		return _rsa.VerifyData(input, hash, signature);
	}

	public override RSAParameters ExportParameters(bool includePrivateParameters)
	{
		return _rsa.ExportParameters(includePrivateParameters);
	}

	public override void ImportParameters(RSAParameters parameters)
	{
		_rsa.ImportParameters(parameters);
	}

	protected override void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			_disposed = true;
			if (disposing && _disposeRsa)
			{
				_rsa.Dispose();
			}
		}
		base.Dispose(disposing);
	}
}

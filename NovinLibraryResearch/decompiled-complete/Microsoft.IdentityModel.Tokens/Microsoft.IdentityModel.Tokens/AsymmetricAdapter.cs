using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

internal class AsymmetricAdapter : IDisposable
{
	private bool _useRSAOeapPadding;

	private RSAEncryptionPadding _rsaEncryptionPadding;

	private bool _disposeCryptoOperators;

	private bool _disposed;

	private SignDelegate SignatureFunction = SignatureFunctionNotFound;

	private VerifyDelegate VerifyFunction = VerifyFunctionNotFound;

	private ECDsaSecurityKey ECDsaSecurityKey { get; set; }

	private HashAlgorithm HashAlgorithm { get; set; }

	private HashAlgorithmName HashAlgorithmName { get; set; }

	private RSASignaturePadding RSASignaturePadding { get; set; }

	private RSA RSA { get; set; }

	private RSACryptoServiceProviderProxy RsaCryptoServiceProviderProxy { get; set; }

	internal AsymmetricAdapter(SecurityKey key, string algorithm, HashAlgorithm hashAlgorithm, HashAlgorithmName hashAlgorithmName, bool requirePrivateKey)
		: this(key, algorithm, hashAlgorithm, requirePrivateKey)
	{
		HashAlgorithmName = hashAlgorithmName;
	}

	internal AsymmetricAdapter(SecurityKey key, string algorithm, bool requirePrivateKey)
		: this(key, algorithm, null, requirePrivateKey)
	{
	}

	internal AsymmetricAdapter(SecurityKey key, string algorithm, HashAlgorithm hashAlgorithm, bool requirePrivateKey)
	{
		HashAlgorithm = hashAlgorithm;
		if (key is RsaSecurityKey rsaSecurityKey)
		{
			InitializeUsingRsaSecurityKey(rsaSecurityKey, algorithm);
		}
		else if (key is X509SecurityKey x509SecurityKey)
		{
			InitializeUsingX509SecurityKey(x509SecurityKey, algorithm, requirePrivateKey);
		}
		else if (key is JsonWebKey webKey)
		{
			if (!JsonWebKeyConverter.TryConvertToSecurityKey(webKey, out var key2))
			{
				return;
			}
			if (key2 is RsaSecurityKey rsaSecurityKey2)
			{
				InitializeUsingRsaSecurityKey(rsaSecurityKey2, algorithm);
				return;
			}
			if (key2 is X509SecurityKey x509SecurityKey2)
			{
				InitializeUsingX509SecurityKey(x509SecurityKey2, algorithm, requirePrivateKey);
				return;
			}
			if (!(key2 is ECDsaSecurityKey ecdsaSecurityKey))
			{
				throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10684: Unable to convert the JsonWebKey to an AsymmetricSecurityKey. Algorithm: '{0}', Key: '{1}'.", algorithm, key)));
			}
			InitializeUsingEcdsaSecurityKey(ecdsaSecurityKey);
		}
		else
		{
			if (!(key is ECDsaSecurityKey eCDsaSecurityKey))
			{
				throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10684: Unable to convert the JsonWebKey to an AsymmetricSecurityKey. Algorithm: '{0}', Key: '{1}'.", algorithm, key)));
			}
			ECDsaSecurityKey = eCDsaSecurityKey;
			SignatureFunction = SignWithECDsa;
			VerifyFunction = VerifyWithECDsa;
		}
	}

	private void InitializeUsingRsaSecurityKey(RsaSecurityKey rsaSecurityKey, string algorithm)
	{
		if (rsaSecurityKey.Rsa != null)
		{
			InitializeUsingRsa(rsaSecurityKey.Rsa, algorithm);
			return;
		}
		RSA rSA = RSA.Create();
		rSA.ImportParameters(rsaSecurityKey.Parameters);
		InitializeUsingRsa(rSA, algorithm);
		_disposeCryptoOperators = true;
	}

	private void InitializeUsingX509SecurityKey(X509SecurityKey x509SecurityKey, string algorithm, bool requirePrivateKey)
	{
		if (requirePrivateKey)
		{
			InitializeUsingRsa(x509SecurityKey.PrivateKey as RSA, algorithm);
		}
		else
		{
			InitializeUsingRsa(x509SecurityKey.PublicKey as RSA, algorithm);
		}
	}

	private void InitializeUsingEcdsaSecurityKey(ECDsaSecurityKey ecdsaSecurityKey)
	{
		ECDsaSecurityKey = ecdsaSecurityKey;
		SignatureFunction = SignWithECDsa;
		VerifyFunction = VerifyWithECDsa;
	}

	internal byte[] Decrypt(byte[] data)
	{
		if (RsaCryptoServiceProviderProxy != null)
		{
			return RsaCryptoServiceProviderProxy.Decrypt(data, _useRSAOeapPadding);
		}
		return RSA.Decrypt(data, _rsaEncryptionPadding);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		_disposed = true;
		if (disposing && _disposeCryptoOperators)
		{
			if (ECDsaSecurityKey != null)
			{
				ECDsaSecurityKey.ECDsa.Dispose();
			}
			if (RsaCryptoServiceProviderProxy != null)
			{
				RsaCryptoServiceProviderProxy.Dispose();
			}
			if (RSA != null)
			{
				RSA.Dispose();
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	internal byte[] Encrypt(byte[] data)
	{
		if (RsaCryptoServiceProviderProxy != null)
		{
			return RsaCryptoServiceProviderProxy.Encrypt(data, _useRSAOeapPadding);
		}
		return RSA.Encrypt(data, _rsaEncryptionPadding);
	}

	private void InitializeUsingRsa(RSA rsa, string algorithm)
	{
		if (algorithm.Equals("PS256", StringComparison.Ordinal) || algorithm.Equals("http://www.w3.org/2007/05/xmldsig-more#sha256-rsa-MGF1", StringComparison.Ordinal) || algorithm.Equals("PS384", StringComparison.Ordinal) || algorithm.Equals("http://www.w3.org/2007/05/xmldsig-more#sha384-rsa-MGF1", StringComparison.Ordinal) || algorithm.Equals("PS512", StringComparison.Ordinal) || algorithm.Equals("http://www.w3.org/2007/05/xmldsig-more#sha512-rsa-MGF1", StringComparison.Ordinal))
		{
			RSASignaturePadding = RSASignaturePadding.Pss;
		}
		else
		{
			RSASignaturePadding = RSASignaturePadding.Pkcs1;
		}
		_useRSAOeapPadding = algorithm.Equals("RSA-OAEP", StringComparison.Ordinal) || algorithm.Equals("http://www.w3.org/2001/04/xmlenc#rsa-oaep", StringComparison.Ordinal);
		if (rsa is RSACryptoServiceProvider rsa2)
		{
			RsaCryptoServiceProviderProxy = new RSACryptoServiceProviderProxy(rsa2);
			SignatureFunction = SignWithRsaCryptoServiceProviderProxy;
			VerifyFunction = VerifyWithRsaCryptoServiceProviderProxy;
			_disposeCryptoOperators = true;
		}
		else
		{
			_rsaEncryptionPadding = ((algorithm.Equals("RSA-OAEP", StringComparison.Ordinal) || algorithm.Equals("http://www.w3.org/2001/04/xmlenc#rsa-oaep", StringComparison.Ordinal)) ? RSAEncryptionPadding.OaepSHA1 : RSAEncryptionPadding.Pkcs1);
			RSA = rsa;
			SignatureFunction = SignWithRsa;
			VerifyFunction = VerifyWithRsa;
		}
	}

	internal byte[] Sign(byte[] bytes)
	{
		return SignatureFunction(bytes);
	}

	private byte[] SignWithECDsa(byte[] bytes)
	{
		return ECDsaSecurityKey.ECDsa.SignHash(HashAlgorithm.ComputeHash(bytes));
	}

	private byte[] SignWithRsa(byte[] bytes)
	{
		return RSA.SignHash(HashAlgorithm.ComputeHash(bytes), HashAlgorithmName, RSASignaturePadding);
	}

	internal byte[] SignWithRsaCryptoServiceProviderProxy(byte[] bytes)
	{
		return RsaCryptoServiceProviderProxy.SignData(bytes, HashAlgorithm);
	}

	internal static byte[] SignatureFunctionNotFound(byte[] bytes)
	{
		throw LogHelper.LogExceptionMessage(new CryptographicException("IDX10685: Unable to Sign, Internal SignFunction is not available."));
	}

	internal bool Verify(byte[] bytes, byte[] signature)
	{
		return VerifyFunction(bytes, signature);
	}

	private bool VerifyWithECDsa(byte[] bytes, byte[] signature)
	{
		return ECDsaSecurityKey.ECDsa.VerifyHash(HashAlgorithm.ComputeHash(bytes), signature);
	}

	private bool VerifyWithRsa(byte[] bytes, byte[] signature)
	{
		return RSA.VerifyHash(HashAlgorithm.ComputeHash(bytes), signature, HashAlgorithmName, RSASignaturePadding);
	}

	private bool VerifyWithRsaCryptoServiceProviderProxy(byte[] bytes, byte[] signature)
	{
		return RsaCryptoServiceProviderProxy.VerifyData(bytes, HashAlgorithm, signature);
	}

	internal static bool VerifyFunctionNotFound(byte[] bytes, byte[] signature)
	{
		throw LogHelper.LogExceptionMessage(new NotSupportedException("IDX10686: Unable to Verify, Internal VerifyFunction is not available."));
	}
}

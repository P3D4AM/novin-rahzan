using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class RsaSecurityKey : AsymmetricSecurityKey
{
	private bool? _hasPrivateKey;

	private bool _foundPrivateKeyDetermined;

	private PrivateKeyStatus _foundPrivateKey;

	[Obsolete("HasPrivateKey method is deprecated, please use FoundPrivateKey instead.")]
	public override bool HasPrivateKey
	{
		get
		{
			if (!_hasPrivateKey.HasValue)
			{
				try
				{
					byte[] data = new byte[20];
					Rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
					_hasPrivateKey = true;
				}
				catch (CryptographicException)
				{
					_hasPrivateKey = false;
				}
			}
			return _hasPrivateKey.Value;
		}
	}

	public override PrivateKeyStatus PrivateKeyStatus
	{
		get
		{
			if (_foundPrivateKeyDetermined)
			{
				return _foundPrivateKey;
			}
			_foundPrivateKeyDetermined = true;
			if (Rsa != null)
			{
				try
				{
					RSAParameters rSAParameters = Rsa.ExportParameters(includePrivateParameters: true);
					if (rSAParameters.D != null && rSAParameters.DP != null && rSAParameters.DQ != null && rSAParameters.P != null && rSAParameters.Q != null && rSAParameters.InverseQ != null)
					{
						_foundPrivateKey = PrivateKeyStatus.Exists;
					}
					else
					{
						_foundPrivateKey = PrivateKeyStatus.DoesNotExist;
					}
				}
				catch (Exception)
				{
					_foundPrivateKey = PrivateKeyStatus.Unknown;
					return _foundPrivateKey;
				}
			}
			else if (Parameters.D != null && Parameters.DP != null && Parameters.DQ != null && Parameters.P != null && Parameters.Q != null && Parameters.InverseQ != null)
			{
				_foundPrivateKey = PrivateKeyStatus.Exists;
			}
			else
			{
				_foundPrivateKey = PrivateKeyStatus.DoesNotExist;
			}
			return _foundPrivateKey;
		}
	}

	public override int KeySize
	{
		get
		{
			if (Rsa != null)
			{
				return Rsa.KeySize;
			}
			if (Parameters.Modulus != null)
			{
				return Parameters.Modulus.Length * 8;
			}
			return 0;
		}
	}

	public RSAParameters Parameters { get; private set; }

	public RSA Rsa { get; private set; }

	internal RsaSecurityKey(JsonWebKey webKey)
		: base(webKey)
	{
		IntializeWithRsaParameters(webKey.CreateRsaParameters());
		webKey.ConvertedSecurityKey = this;
	}

	public RsaSecurityKey(RSAParameters rsaParameters)
	{
		IntializeWithRsaParameters(rsaParameters);
	}

	internal void IntializeWithRsaParameters(RSAParameters rsaParameters)
	{
		if (rsaParameters.Modulus == null)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10700: {0} is unable to use 'rsaParameters'. {1} is null.", this, "Modulus")));
		}
		if (rsaParameters.Exponent == null)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant("IDX10700: {0} is unable to use 'rsaParameters'. {1} is null.", this, "Exponent")));
		}
		_hasPrivateKey = rsaParameters.D != null && rsaParameters.DP != null && rsaParameters.DQ != null && rsaParameters.P != null && rsaParameters.Q != null && rsaParameters.InverseQ != null;
		_foundPrivateKey = ((!_hasPrivateKey.Value) ? PrivateKeyStatus.DoesNotExist : PrivateKeyStatus.Exists);
		_foundPrivateKeyDetermined = true;
		Parameters = rsaParameters;
	}

	public RsaSecurityKey(RSA rsa)
	{
		Rsa = rsa ?? throw LogHelper.LogArgumentNullException("rsa");
	}

	public override bool CanComputeJwkThumbprint()
	{
		if (Rsa == null)
		{
			if (Parameters.Exponent != null)
			{
				return Parameters.Modulus != null;
			}
			return false;
		}
		return true;
	}

	public override byte[] ComputeJwkThumbprint()
	{
		RSAParameters rSAParameters = Parameters;
		if (rSAParameters.Exponent == null || rSAParameters.Modulus == null)
		{
			rSAParameters = Rsa.ExportParameters(includePrivateParameters: false);
		}
		return Utility.GenerateSha256Hash("{\"e\":\"" + Base64UrlEncoder.Encode(rSAParameters.Exponent) + "\",\"kty\":\"RSA\",\"n\":\"" + Base64UrlEncoder.Encode(rSAParameters.Modulus) + "\"}");
	}
}

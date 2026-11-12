using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class SigningCredentials
{
	private string _algorithm;

	private string _digest;

	private SecurityKey _key;

	public string Algorithm
	{
		get
		{
			return _algorithm;
		}
		private set
		{
			if (string.IsNullOrEmpty(value))
			{
				throw LogHelper.LogArgumentNullException("algorithm");
			}
			_algorithm = value;
		}
	}

	public string Digest
	{
		get
		{
			return _digest;
		}
		private set
		{
			if (string.IsNullOrEmpty(value))
			{
				throw LogHelper.LogArgumentNullException("digest");
			}
			_digest = value;
		}
	}

	public CryptoProviderFactory CryptoProviderFactory { get; set; }

	public SecurityKey Key
	{
		get
		{
			return _key;
		}
		private set
		{
			_key = value ?? throw LogHelper.LogArgumentNullException("key");
		}
	}

	public string Kid => Key.KeyId;

	protected SigningCredentials(X509Certificate2 certificate)
	{
		if (certificate == null)
		{
			throw LogHelper.LogArgumentNullException("certificate");
		}
		Key = new X509SecurityKey(certificate);
		Algorithm = "RS256";
	}

	protected SigningCredentials(X509Certificate2 certificate, string algorithm)
	{
		if (certificate == null)
		{
			throw LogHelper.LogArgumentNullException("certificate");
		}
		Key = new X509SecurityKey(certificate);
		Algorithm = algorithm;
	}

	public SigningCredentials(SecurityKey key, string algorithm)
	{
		Key = key;
		Algorithm = algorithm;
	}

	public SigningCredentials(SecurityKey key, string algorithm, string digest)
	{
		Key = key;
		Algorithm = algorithm;
		Digest = digest;
	}
}

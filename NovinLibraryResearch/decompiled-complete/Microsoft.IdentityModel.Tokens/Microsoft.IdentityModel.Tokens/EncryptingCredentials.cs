using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class EncryptingCredentials
{
	private string _alg;

	private string _enc;

	private SecurityKey _key;

	public string Alg
	{
		get
		{
			return _alg;
		}
		private set
		{
			if (string.IsNullOrEmpty(value))
			{
				throw LogHelper.LogArgumentNullException("alg");
			}
			_alg = value;
		}
	}

	public string Enc
	{
		get
		{
			return _enc;
		}
		private set
		{
			if (string.IsNullOrEmpty(value))
			{
				throw LogHelper.LogArgumentNullException("enc");
			}
			_enc = value;
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

	protected EncryptingCredentials(X509Certificate2 certificate, string alg, string enc)
	{
		if (certificate == null)
		{
			throw LogHelper.LogArgumentNullException("certificate");
		}
		Key = new X509SecurityKey(certificate);
		Alg = alg;
		Enc = enc;
	}

	public EncryptingCredentials(SecurityKey key, string alg, string enc)
	{
		Key = key;
		Alg = alg;
		Enc = enc;
	}

	public EncryptingCredentials(SymmetricSecurityKey key, string enc)
		: this(key, "none", enc)
	{
	}
}

using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Tokens;

public class X509SigningCredentials : SigningCredentials
{
	public X509Certificate2 Certificate { get; private set; }

	public X509SigningCredentials(X509Certificate2 certificate)
		: base(certificate)
	{
		Certificate = certificate;
	}

	public X509SigningCredentials(X509Certificate2 certificate, string algorithm)
		: base(certificate, algorithm)
	{
		Certificate = certificate;
	}
}

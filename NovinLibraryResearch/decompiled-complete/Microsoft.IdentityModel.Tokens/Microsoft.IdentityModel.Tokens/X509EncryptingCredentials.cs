using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Tokens;

public class X509EncryptingCredentials : EncryptingCredentials
{
	public X509Certificate2 Certificate { get; private set; }

	public X509EncryptingCredentials(X509Certificate2 certificate)
		: this(certificate, "http://www.w3.org/2001/04/xmlenc#rsa-oaep", "A128CBC-HS256")
	{
	}

	public X509EncryptingCredentials(X509Certificate2 certificate, string keyWrapAlgorithm, string dataEncryptionAlgorithm)
		: base(certificate, keyWrapAlgorithm, dataEncryptionAlgorithm)
	{
		Certificate = certificate;
	}
}

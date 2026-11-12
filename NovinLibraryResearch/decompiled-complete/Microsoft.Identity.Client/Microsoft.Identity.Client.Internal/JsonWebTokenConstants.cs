namespace Microsoft.Identity.Client.Internal;

internal class JsonWebTokenConstants
{
	internal class Algorithms
	{
		public const string RsaSha256 = "RS256";

		public const string None = "none";
	}

	internal class ReservedClaims
	{
		public const string Audience = "aud";

		public const string Issuer = "iss";

		public const string Subject = "sub";

		public const string NotBefore = "nbf";

		public const string ExpiresOn = "exp";

		public const string JwtIdentifier = "jti";
	}

	internal static class ReservedHeaderParameters
	{
		public const string Algorithm = "alg";

		public const string Type = "typ";

		public const string KeyId = "kid";

		public const string X509CertificateThumbprint = "x5t";

		public const string X509CertificatePublicCertValue = "x5c";
	}

	public const uint JwtToAadLifetimeInSeconds = 600u;

	public const string JWTHeaderType = "JWT";
}

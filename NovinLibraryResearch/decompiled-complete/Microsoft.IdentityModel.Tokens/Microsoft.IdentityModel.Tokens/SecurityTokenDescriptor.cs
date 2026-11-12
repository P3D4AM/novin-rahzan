using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Microsoft.IdentityModel.Tokens;

public class SecurityTokenDescriptor
{
	public string Audience { get; set; }

	public string CompressionAlgorithm { get; set; }

	public EncryptingCredentials EncryptingCredentials { get; set; }

	public DateTime? Expires { get; set; }

	public string Issuer { get; set; }

	public DateTime? IssuedAt { get; set; }

	public DateTime? NotBefore { get; set; }

	public string TokenType { get; set; }

	public IDictionary<string, object> Claims { get; set; }

	public IDictionary<string, object> AdditionalHeaderClaims { get; set; }

	public SigningCredentials SigningCredentials { get; set; }

	public ClaimsIdentity Subject { get; set; }
}

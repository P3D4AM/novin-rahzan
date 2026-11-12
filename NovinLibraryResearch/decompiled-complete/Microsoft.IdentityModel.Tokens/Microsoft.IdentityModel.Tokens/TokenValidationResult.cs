using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Microsoft.IdentityModel.Tokens;

public class TokenValidationResult
{
	private Lazy<IDictionary<string, object>> _claims => new Lazy<IDictionary<string, object>>(() => TokenUtilities.CreateDictionaryFromClaims(ClaimsIdentity?.Claims));

	public IDictionary<string, object> Claims => _claims.Value;

	public ClaimsIdentity ClaimsIdentity { get; set; }

	public Exception Exception { get; set; }

	public string Issuer { get; set; }

	public bool IsValid { get; set; }

	public IDictionary<string, object> PropertyBag { get; } = new Dictionary<string, object>(StringComparer.Ordinal);


	public SecurityToken SecurityToken { get; set; }

	public CallContext TokenContext { get; set; }

	public string TokenType { get; set; }
}
